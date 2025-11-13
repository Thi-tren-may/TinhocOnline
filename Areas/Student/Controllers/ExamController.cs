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
                .Where(e => e.Status == "published" || e.CreatedBy == studentId.Value)
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
                .Include(e => e.ExamQuestions)
                .FirstOrDefaultAsync(e => e.ExamId == id);

            if (exam == null)
            {
                return NotFound();
            }

            // Kiểm tra quyền xem đề thi
            // Chỉ cho phép xem nếu: đề published HOẶC do chính học sinh tạo
            if (exam.Status != "published" && exam.CreatedBy != studentId.Value)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền truy cập đề thi này!";
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra đã làm bài chưa (chỉ với đề published/bắt buộc)
            if (exam.Status == "published")
            {
                var existingAttempt = await _context.StudentExams
                    .FirstOrDefaultAsync(se => se.ExamId == id && se.StudentId == studentId.Value);

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
                .Include(e => e.ExamQuestions)
                    .ThenInclude(eq => eq.Question)
                        .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(e => e.ExamId == id);

            if (exam == null)
            {
                return NotFound();
            }

            // Kiểm tra đã làm bài chưa (chỉ với đề published)
            if (exam.Status == "published")
            {
                var existingAttempt = await _context.StudentExams
                    .FirstOrDefaultAsync(se => se.ExamId == id && se.StudentId == studentId.Value);

                if (existingAttempt != null)
                {
                    TempData["ErrorMessage"] = "Bạn đã làm bài thi này rồi!";
                    return RedirectToAction(nameof(Index));
                }
            }

            // TODO: Implement UI làm bài thi
            return View(exam);
        }
    }
}
