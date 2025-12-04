using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TinhocOnline.Models;
using TinhocOnline.Models.ViewModels;

namespace TinhocOnline.Areas.Student.Controllers
{
    [Area("Student")]
    public class ExamController : Controller
    {
        private readonly DataContext _context;

        public ExamController(DataContext context)
        {
            _context = context;
        }

        // GET: Student/Exam - Danh sách đề thi công khai
        public async Task<IActionResult> Index()
        {
            var studentId = HttpContext.Session.GetInt32("UserId");
            if (studentId == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            // Lấy đề thi published HOẶC đề do chính học sinh tạo (draft/published)
            var exams = await _context.Exams
                .Include(e => e.ExamType)
                .Include(e => e.Creator)
                .Include(e => e.StudentExams.Where(se => se.StudentId == studentId))
                .Where(e => e.Status == "published" || e.CreatedBy == studentId)
                .OrderByDescending(e => e.ExamId)
                .ToListAsync();

            return View(exams);
        }

        // GET: Student/Exam/CreatePractice - Tạo đề ôn tập
        public async Task<IActionResult> CreatePractice()
        {
            var studentId = HttpContext.Session.GetInt32("UserId");
            if (studentId == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            // Load ExamTypes
            ViewBag.ExamTypes = new SelectList(
                await _context.ExamTypes.Where(et => et.Status == "active").ToListAsync(),
                "ExamTypeId",
                "TypeName"
            );

            // Load Topics
            var topics = await _context.Topics.Where(t => t.Status == "active").ToListAsync();
            ViewBag.Topics = topics;

            var model = new CreateExamViewModel
            {
                CreatedBy = studentId.Value,
                TotalQuestions = 50,
                EasyPercentage = 40,
                MediumPercentage = 30,
                HardPercentage = 30,
                Duration = 45,
                PassingScore = 5.0M,
                Status = "draft" // Học sinh chỉ tạo draft
            };

            return View(model);
        }

        // POST: Student/Exam/CreatePractice
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePractice(CreateExamViewModel model)
        {
            var studentId = HttpContext.Session.GetInt32("UserId");
            if (studentId == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            // Validate tổng tỷ lệ độ khó = 100%
            var totalDifficulty = model.EasyPercentage + model.MediumPercentage + model.HardPercentage;
            if (totalDifficulty != 100)
            {
                ModelState.AddModelError("", $"Tổng tỷ lệ độ khó phải bằng 100% (hiện tại: {totalDifficulty}%)");
            }

            // Validate phải chọn ít nhất 1 topic trong chế độ custom
            if (model.CreateMode == "custom" && (model.SelectedTopicIds == null || !model.SelectedTopicIds.Any()))
            {
                ModelState.AddModelError("", "Vui lòng chọn ít nhất 1 chủ đề");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.ExamName = $"Đề ôn tập - {DateTime.Now:dd/MM/yyyy}";
                    model.Status = "draft"; // Force draft cho học sinh
                    model.CreatedBy = studentId.Value;

                    // Tạo Exam
                    var exam = new Exam
                    {
                        ExamName = model.ExamName,
                        ExamTypeId = model.ExamTypeId,
                        GradeLevel = model.GradeLevel,
                        Duration = model.Duration,
                        TotalQuestions = model.TotalQuestions,
                        EasyPercentage = model.EasyPercentage,
                        MediumPercentage = model.MediumPercentage,
                        HardPercentage = model.HardPercentage,
                        ShuffleQuestions = model.ShuffleQuestions,
                        ShuffleAnswers = model.ShuffleAnswers,
                        PassingScore = model.PassingScore,
                        CreatedBy = studentId.Value,
                        Status = "draft"
                    };

                    _context.Exams.Add(exam);
                    await _context.SaveChangesAsync();

                    // Xử lý ma trận chủ đề
                    Dictionary<int, decimal> topicMatrix;

                    if (model.CreateMode == "quick")
                    {
                        // Chế độ nhanh: Phân đều theo tất cả topics
                        var activeTopics = await _context.Topics.Where(t => t.Status == "active").ToListAsync();
                        topicMatrix = new Dictionary<int, decimal>();
                        decimal percentage = Math.Round(100m / activeTopics.Count, 2);

                        foreach (var topic in activeTopics)
                        {
                            topicMatrix[topic.TopicId] = percentage;
                        }
                    }
                    else
                    {
                        // Chế độ custom: Phân đều theo topics đã chọn
                        topicMatrix = new Dictionary<int, decimal>();
                        if (model.SelectedTopicIds != null && model.SelectedTopicIds.Any())
                        {
                            decimal percentage = Math.Round(100m / model.SelectedTopicIds.Count, 2);

                            foreach (var topicId in model.SelectedTopicIds)
                            {
                                topicMatrix[topicId] = percentage;
                            }
                        }
                    }

                    // Sinh câu hỏi cho đề thi
                    var success = await GenerateExamQuestions(exam.ExamId, topicMatrix, model);

                    if (!success)
                    {
                        // Xóa exam nếu không đủ câu hỏi
                        _context.Exams.Remove(exam);
                        await _context.SaveChangesAsync();

                        TempData["ErrorMessage"] = "Không đủ câu hỏi trong ngân hàng để tạo đề thi. Vui lòng giảm số câu hoặc chọn lại cấu hình.";

                        ViewBag.ExamTypes = new SelectList(
                            await _context.ExamTypes.Where(et => et.Status == "active").ToListAsync(),
                            "ExamTypeId",
                            "TypeName"
                        );
                        ViewBag.Topics = await _context.Topics.Where(t => t.Status == "active").ToListAsync();

                        return View(model);
                    }

                    TempData["SuccessMessage"] = "Tạo đề ôn tập thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Lỗi: {ex.Message}");
                }
            }

            // Reload data nếu validation fail
            ViewBag.ExamTypes = new SelectList(
                await _context.ExamTypes.Where(et => et.Status == "active").ToListAsync(),
                "ExamTypeId",
                "TypeName"
            );
            ViewBag.Topics = await _context.Topics.Where(t => t.Status == "active").ToListAsync();

            return View(model);
        }

        // Thuật toán sinh câu hỏi (giống Teacher)
        private async Task<bool> GenerateExamQuestions(int examId, Dictionary<int, decimal> topicMatrix, CreateExamViewModel model)
        {
            var questions = new List<ExamQuestion>();
            var order = 1;

            // Tính số câu cho từng topic trước (để xử lý làm tròn)
            var topicQuestionCounts = new Dictionary<int, int>();
            int totalAllocated = 0;

            foreach (var topicEntry in topicMatrix.OrderByDescending(kv => kv.Value))
            {
                var topicId = topicEntry.Key;
                var topicPercentage = topicEntry.Value;
                var questionsForTopic = (int)Math.Round(model.TotalQuestions * topicPercentage / 100);

                topicQuestionCounts[topicId] = questionsForTopic;
                totalAllocated += questionsForTopic;
            }

            // Điều chỉnh nếu tổng không khớp do làm tròn
            int difference = model.TotalQuestions - totalAllocated;
            if (difference != 0)
            {
                // Phân bổ đều các câu dư vào các topic
                var topicIds = topicMatrix.Keys.ToList();
                int topicIndex = 0;

                // Nếu dư câu: thêm lần lượt vào từng topic
                // Nếu thiếu câu: bớt lần lượt từ từng topic
                while (difference != 0)
                {
                    var topicId = topicIds[topicIndex % topicIds.Count];

                    if (difference > 0)
                    {
                        topicQuestionCounts[topicId]++;
                        difference--;
                    }
                    else if (difference < 0 && topicQuestionCounts[topicId] > 0)
                    {
                        topicQuestionCounts[topicId]--;
                        difference++;
                    }

                    topicIndex++;

                    // Tránh vòng lặp vô hạn
                    if (topicIndex > topicIds.Count * 100) break;
                }
            }

            foreach (var topicEntry in topicMatrix)
            {
                var topicId = topicEntry.Key;
                var topicPercentage = topicEntry.Value;
                var questionsForTopic = topicQuestionCounts[topicId];

                if (questionsForTopic == 0) continue;

                var easyCount = (int)Math.Round(questionsForTopic * model.EasyPercentage / 100);
                var mediumCount = (int)Math.Round(questionsForTopic * model.MediumPercentage / 100);
                var hardCount = questionsForTopic - easyCount - mediumCount;

                var query = _context.Questions
                    .Where(q => q.TopicId == topicId && q.Status == "active");

                // Bỏ filter GradeLevel để lấy đủ câu hỏi
                // GradeLevel chỉ dùng tham khảo, không filter
                if (!string.IsNullOrEmpty(model.GradeLevel))
                {
                    // query = query.Where(q => q.GradeLevel == model.GradeLevel || q.GradeLevel == null);
                    // Đã disable filter để có đủ câu hỏi
                }

                var easyQuestions = await query
                    .Where(q => q.DifficultyLevel == "easy")
                    .OrderBy(q => Guid.NewGuid())
                    .Take(easyCount)
                    .ToListAsync();

                var mediumQuestions = await query
                    .Where(q => q.DifficultyLevel == "medium")
                    .OrderBy(q => Guid.NewGuid())
                    .Take(mediumCount)
                    .ToListAsync();

                var hardQuestions = await query
                    .Where(q => q.DifficultyLevel == "hard")
                    .OrderBy(q => Guid.NewGuid())
                    .Take(hardCount)
                    .ToListAsync();

                if (easyQuestions.Count < easyCount ||
                    mediumQuestions.Count < mediumCount ||
                    hardQuestions.Count < hardCount)
                {
                    return false;
                }

                foreach (var q in easyQuestions.Concat(mediumQuestions).Concat(hardQuestions))
                {
                    questions.Add(new ExamQuestion
                    {
                        ExamId = examId,
                        QuestionId = q.QuestionId,
                        QuestionOrder = order++
                    });
                }

                var examTopic = new ExamTopic
                {
                    ExamId = examId,
                    TopicId = topicId,
                    QuestionCount = questionsForTopic
                };
                _context.ExamTopics.Add(examTopic);
            }

            if (model.ShuffleQuestions)
            {
                var shuffled = questions.OrderBy(q => Guid.NewGuid()).ToList();
                for (int i = 0; i < shuffled.Count; i++)
                {
                    shuffled[i].QuestionOrder = i + 1;
                }
                questions = shuffled;
            }

            _context.ExamQuestions.AddRange(questions);
            await _context.SaveChangesAsync();

            return true;
        }

        // Sinh câu hỏi riêng cho từng học sinh
        private async Task<bool> GenerateQuestionsForStudent(int examId, int studentExamId)
        {
            // Load exam với ExamTopics
            var exam = await _context.Exams
                .Include(e => e.ExamTopics)
                    .ThenInclude(et => et.Topic)
                .FirstOrDefaultAsync(e => e.ExamId == examId);

            if (exam == null || exam.ExamTopics == null || !exam.ExamTopics.Any())
            {
                return false;
            }

            var studentExamQuestions = new List<StudentExamQuestion>();
            var order = 1;

            foreach (var examTopic in exam.ExamTopics)
            {
                var questionsForTopic = examTopic.QuestionCount;

                if (questionsForTopic == 0) continue;

                // Tính số câu cho từng độ khó
                var easyCount = (int)Math.Round(questionsForTopic * exam.EasyPercentage / 100);
                var mediumCount = (int)Math.Round(questionsForTopic * exam.MediumPercentage / 100);
                var hardCount = questionsForTopic - easyCount - mediumCount;

                var query = _context.Questions
                    .Where(q => q.TopicId == examTopic.TopicId && q.Status == "active");

                // Lấy câu hỏi theo độ khó
                var easyQuestions = await query
                    .Where(q => q.DifficultyLevel == "easy")
                    .OrderBy(q => Guid.NewGuid())
                    .Take(easyCount)
                    .ToListAsync();

                var mediumQuestions = await query
                    .Where(q => q.DifficultyLevel == "medium")
                    .OrderBy(q => Guid.NewGuid())
                    .Take(mediumCount)
                    .ToListAsync();

                var hardQuestions = await query
                    .Where(q => q.DifficultyLevel == "hard")
                    .OrderBy(q => Guid.NewGuid())
                    .Take(hardCount)
                    .ToListAsync();

                // Kiểm tra đủ câu hỏi không
                if (easyQuestions.Count < easyCount ||
                    mediumQuestions.Count < mediumCount ||
                    hardQuestions.Count < hardCount)
                {
                    return false;
                }

                // Thêm vào danh sách
                foreach (var q in easyQuestions.Concat(mediumQuestions).Concat(hardQuestions))
                {
                    studentExamQuestions.Add(new StudentExamQuestion
                    {
                        StudentExamId = studentExamId,
                        QuestionId = q.QuestionId,
                        QuestionOrder = order++
                    });
                }
            }

            // Xáo trộn câu hỏi nếu cần
            if (exam.ShuffleQuestions)
            {
                var shuffled = studentExamQuestions.OrderBy(seq => Guid.NewGuid()).ToList();
                for (int i = 0; i < shuffled.Count; i++)
                {
                    shuffled[i].QuestionOrder = i + 1;
                }
                studentExamQuestions = shuffled;
            }

            _context.StudentExamQuestions.AddRange(studentExamQuestions);
            await _context.SaveChangesAsync();

            return true;
        }

        // GET: Student/Exam/BeforeExam/5 - Màn hình chuẩn bị thi
        public async Task<IActionResult> BeforeExam(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentId = HttpContext.Session.GetInt32("UserId");
            if (studentId == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            // Lấy thông tin đề thi với các quan hệ cần thiết
            var exam = await _context.Exams
                .Include(e => e.ExamType)
                .Include(e => e.Creator)
                .Include(e => e.ExamTopics)
                    .ThenInclude(et => et.Topic)
                .FirstOrDefaultAsync(e => e.ExamId == id);

            if (exam == null)
            {
                return NotFound();
            }

            // Kiểm tra thời gian thi
            if (exam.StartDate.HasValue && DateTime.Now < exam.StartDate.Value)
            {
                TempData["ErrorMessage"] = $"Chưa đến giờ thi! Đề thi bắt đầu lúc {exam.StartDate.Value:dd/MM/yyyy HH:mm}";
                return RedirectToAction(nameof(Index));
            }

            if (exam.EndDate.HasValue && DateTime.Now > exam.EndDate.Value)
            {
                TempData["ErrorMessage"] = $"Đã hết hạn thi! Đề thi kết thúc lúc {exam.EndDate.Value:dd/MM/yyyy HH:mm}";
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra quyền xem đề thi
            // Chỉ cho phép xem nếu: đề published HOẶC do chính học sinh tạo
            if (exam.Status != "published" && exam.CreatedBy != studentId)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền truy cập đề thi này!";
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra đã làm bài chưa (chỉ với đề published/bắt buộc)
            if (exam.Status == "published")
            {
                var existingAttempt = await _context.StudentExams
                    .FirstOrDefaultAsync(se => se.ExamId == id && se.StudentId == studentId);

                if (existingAttempt != null)
                {
                    TempData["ErrorMessage"] = "Bạn đã làm bài thi này rồi!";
                    return RedirectToAction(nameof(Index));
                }
            }

            // Truyền thông tin kiểm tra loại đề (bắt buộc hay không)
            ViewBag.IsRequired = exam.ExamType?.TypeName?.Contains("Bắt buộc") == true ||
                                 exam.ExamType?.TypeName?.Contains("Required") == true;

            return View(exam);
        }

        // GET: Student/Exam/TakeExam/5 - Làm bài thi
        public async Task<IActionResult> TakeExam(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentId = HttpContext.Session.GetInt32("UserId");
            if (studentId == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            var exam = await _context.Exams
                .Include(e => e.ExamType)
                .Include(e => e.ExamTopics)
                    .ThenInclude(et => et.Topic)
                .FirstOrDefaultAsync(e => e.ExamId == id);

            if (exam == null)
            {
                return NotFound();
            }

            // Kiểm tra đã làm bài chưa (chỉ với đề published)
            StudentExam studentExam;
            if (exam.Status == "published")
            {
                var existingAttempt = await _context.StudentExams
                    .FirstOrDefaultAsync(se => se.ExamId == id && se.StudentId == studentId);

                if (existingAttempt != null)
                {
                    TempData["ErrorMessage"] = "Bạn đã làm bài thi này rồi!";
                    return RedirectToAction(nameof(Index));
                }

                // Tạo StudentExam record
                studentExam = new StudentExam
                {
                    ExamId = id.Value,
                    StudentId = studentId.Value,
                    StartTime = DateTime.Now,
                    Status = "in-progress"
                };
                _context.StudentExams.Add(studentExam);
                await _context.SaveChangesAsync();

                // Sinh câu hỏi riêng cho học sinh
                var success = await GenerateQuestionsForStudent(id.Value, studentExam.StudentExamId);
                if (!success)
                {
                    _context.StudentExams.Remove(studentExam);
                    await _context.SaveChangesAsync();
                    TempData["ErrorMessage"] = "Không đủ câu hỏi trong ngân hàng để tạo đề thi. Vui lòng liên hệ giáo viên.";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                // Với đề draft (học sinh tự tạo), kiểm tra hoặc tạo mới
                studentExam = await _context.StudentExams
                    .FirstOrDefaultAsync(se => se.ExamId == id && se.StudentId == studentId);

                if (studentExam == null)
                {
                    studentExam = new StudentExam
                    {
                        ExamId = id.Value,
                        StudentId = studentId.Value,
                        StartTime = DateTime.Now,
                        Status = "in-progress"
                    };
                    _context.StudentExams.Add(studentExam);
                    await _context.SaveChangesAsync();

                    var success = await GenerateQuestionsForStudent(id.Value, studentExam.StudentExamId);
                    if (!success)
                    {
                        _context.StudentExams.Remove(studentExam);
                        await _context.SaveChangesAsync();
                        TempData["ErrorMessage"] = "Không đủ câu hỏi trong ngân hàng để tạo đề thi.";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            // Load câu hỏi từ StudentExamQuestions
            var examWithQuestions = await _context.Exams
                .Include(e => e.ExamType)
                .Include(e => e.StudentExams.Where(se => se.StudentExamId == studentExam.StudentExamId))
                    .ThenInclude(se => se.StudentExamQuestions.OrderBy(seq => seq.QuestionOrder))
                        .ThenInclude(seq => seq.Question)
                            .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(e => e.ExamId == id);

            ViewBag.StudentExamId = studentExam.StudentExamId;
            return View(examWithQuestions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitExam(int studentExamId, IFormCollection form)
        {
            var studentId = HttpContext.Session.GetInt32("UserId");
            if (studentId == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            // Load StudentExam với tất cả questions và answers
            var studentExam = await _context.StudentExams
                .Include(se => se.Exam)
                .Include(se => se.StudentExamQuestions)
                    .ThenInclude(seq => seq.Question)
                        .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(se => se.StudentExamId == studentExamId && se.StudentId == studentId);

            if (studentExam == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy bài thi.";
                return RedirectToAction(nameof(Index));
            }

            if (studentExam.Status == "completed")
            {
                TempData["ErrorMessage"] = "Bài thi này đã được nộp trước đó.";
                return RedirectToAction("ViewResults", new { id = studentExamId });
            }

            // Parse submitted answers từ form
            var submittedAnswers = new Dictionary<int, int>(); // Key: StudentExamQuestionId, Value: AnswerId
            foreach (var key in form.Keys)
            {
                if (key.StartsWith("answer_"))
                {
                    var studentExamQuestionIdStr = key.Substring("answer_".Length);
                    if (int.TryParse(studentExamQuestionIdStr, out int studentExamQuestionId) && 
                        int.TryParse(form[key], out int answerId))
                    {
                        submittedAnswers[studentExamQuestionId] = answerId;
                    }
                }
            }

            // Calculate score
            int correctAnswers = 0;
            int totalQuestions = studentExam.StudentExamQuestions.Count;

            foreach (var studentExamQuestion in studentExam.StudentExamQuestions)
            {
                if (submittedAnswers.TryGetValue(studentExamQuestion.StudentExamQuestionId, out int submittedAnswerId))
                {
                    var correctAnswer = studentExamQuestion.Question.Answers.FirstOrDefault(a => a.IsCorrect);
                    bool isCorrect = correctAnswer != null && correctAnswer.AnswerId == submittedAnswerId;
                    
                    if (isCorrect)
                    {
                        correctAnswers++;
                    }

                    // Save student answer
                    var studentAnswer = new StudentAnswer
                    {
                        StudentExamId = studentExamId,
                        QuestionId = studentExamQuestion.QuestionId,
                        AnswerId = submittedAnswerId,
                        IsCorrect = isCorrect
                    };
                    _context.StudentAnswers.Add(studentAnswer);
                }
            }

            // Calculate final score (out of 10)
            decimal score = totalQuestions > 0 ? Math.Round((decimal)correctAnswers / totalQuestions * 10, 2) : 0;

            // Update StudentExam
            studentExam.EndTime = DateTime.Now;
            studentExam.SubmittedAt = DateTime.Now;
            studentExam.Score = score;
            studentExam.Status = "completed";

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Đã nộp bài thành công! Điểm số: {score}/10 ({correctAnswers}/{totalQuestions} câu đúng)";
            return RedirectToAction("ViewResults", new { id = studentExamId });
        }

        // GET: Student/Exam/ViewResults/5
        public async Task<IActionResult> ViewResults(int? id)
        {
            var studentId = HttpContext.Session.GetInt32("UserId");
            if (studentId == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            if (id == null)
            {
                return NotFound();
            }

            // Load StudentExam với tất cả thông tin cần thiết
            var studentExam = await _context.StudentExams
                .Include(se => se.Exam)
                    .ThenInclude(e => e.ExamType)
                .Include(se => se.StudentExamQuestions)
                    .ThenInclude(seq => seq.Question)
                        .ThenInclude(q => q.Answers)
                .Include(se => se.Student)
                .FirstOrDefaultAsync(se => se.StudentExamId == id && se.StudentId == studentId);

            if (studentExam == null)
            {
                return NotFound();
            }

            // Load submitted answers
            var studentAnswers = await _context.StudentAnswers
                .Include(sa => sa.Answer)
                .Where(sa => sa.StudentExamId == id)
                .ToListAsync();

            ViewBag.StudentAnswers = studentAnswers;

            return View(studentExam);
        }

        // GET: Student/Exam/History - Lịch sử làm bài
        public async Task<IActionResult> History()
        {
            var studentId = HttpContext.Session.GetInt32("UserId");
            if (studentId == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            // Load tất cả bài thi đã làm của học sinh
            var studentExams = await _context.StudentExams
                .Include(se => se.Exam)
                    .ThenInclude(e => e.ExamType)
                .Include(se => se.Exam)
                    .ThenInclude(e => e.Creator)
                .Where(se => se.StudentId == studentId && se.Status == "completed")
                .OrderByDescending(se => se.SubmittedAt)
                .ToListAsync();

            return View(studentExams);
        }
    }
}
