using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TinhocOnline.Models;
using TinhocOnline.Models.ViewModels;

namespace TinhocOnline.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    public class ExamManagerController : Controller
    {
        private readonly DataContext _context;

        public ExamManagerController(DataContext context)
        {
            _context = context;
        }

        // GET: Teacher/ExamManager
        public async Task<IActionResult> Index()
        {
            var teacherId = HttpContext.Session.GetInt32("UserId");
            if (teacherId == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            var exams = await _context.Exams
                .Include(e => e.ExamType)
                .Include(e => e.Creator)
                .Include(e => e.ExamTopics)
                    .ThenInclude(et => et.Topic)
                .Where(e => e.CreatedBy == teacherId.Value)
                .OrderByDescending(e => e.ExamId)
                .ToListAsync();

            return View(exams);
        }

        // GET: Teacher/ExamManager/Create
        public async Task<IActionResult> Create()
        {
            var teacherId = HttpContext.Session.GetInt32("UserId");
            if (teacherId == null)
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
                CreatedBy = teacherId.Value,
                TotalQuestions = 50,
                EasyPercentage = 40,
                MediumPercentage = 30,
                HardPercentage = 30,
                Duration = 45,
                PassingScore = 5.0M
            };

            return View(model);
        }

        // POST: Teacher/ExamManager/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateExamViewModel model)
        {
            var teacherId = HttpContext.Session.GetInt32("UserId");
            if (teacherId == null)
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
                        CreatedBy = teacherId.Value,
                        Status = model.Status
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
                        
                        TempData["ErrorMessage"] = "Không đủ câu hỏi trong ngân hàng để tạo đề thi. Vui lòng giảm số câu hoặc thêm câu hỏi mới.";
                        
                        ViewBag.ExamTypes = new SelectList(
                            await _context.ExamTypes.Where(et => et.Status == "active").ToListAsync(),
                            "ExamTypeId",
                            "TypeName"
                        );
                        ViewBag.Topics = await _context.Topics.Where(t => t.Status == "active").ToListAsync();
                        
                        return View(model);
                    }

                    TempData["SuccessMessage"] = "Tạo đề thi thành công!";
                    return RedirectToAction(nameof(Details), new { id = exam.ExamId });
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

        // Thuật toán sinh câu hỏi
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

                Console.WriteLine($"[DEBUG] Topic {topicId}: {questionsForTopic} câu ({topicPercentage}%)");

                if (questionsForTopic == 0) continue;

                // Tính số câu theo độ khó
                var easyCount = (int)Math.Round(questionsForTopic * model.EasyPercentage / 100);
                var mediumCount = (int)Math.Round(questionsForTopic * model.MediumPercentage / 100);
                var hardCount = questionsForTopic - easyCount - mediumCount;

                // Query câu hỏi cho topic này
                var query = _context.Questions
                    .Where(q => q.TopicId == topicId && q.Status == "active");

                // Không filter theo GradeLevel để có đủ câu hỏi
                // GradeLevel chỉ dùng để tham khảo, không dùng để filter

                // Lấy câu dễ
                var easyQuestions = await query
                    .Where(q => q.DifficultyLevel == "easy")
                    .OrderBy(q => Guid.NewGuid())
                    .Take(easyCount)
                    .ToListAsync();

                // Lấy câu trung bình
                var mediumQuestions = await query
                    .Where(q => q.DifficultyLevel == "medium")
                    .OrderBy(q => Guid.NewGuid())
                    .Take(mediumCount)
                    .ToListAsync();

                // Lấy câu khó
                var hardQuestions = await query
                    .Where(q => q.DifficultyLevel == "hard")
                    .OrderBy(q => Guid.NewGuid())
                    .Take(hardCount)
                    .ToListAsync();

                // Kiểm tra đủ câu không
                if (easyQuestions.Count < easyCount || 
                    mediumQuestions.Count < mediumCount || 
                    hardQuestions.Count < hardCount)
                {
                    return false; // Không đủ câu hỏi
                }

                // Thêm vào danh sách
                foreach (var q in easyQuestions.Concat(mediumQuestions).Concat(hardQuestions))
                {
                    questions.Add(new ExamQuestion
                    {
                        ExamId = examId,
                        QuestionId = q.QuestionId,
                        QuestionOrder = order++
                    });
                }

                // Lưu ExamTopic
                var examTopic = new ExamTopic
                {
                    ExamId = examId,
                    TopicId = topicId,
                    QuestionCount = questionsForTopic
                };
                _context.ExamTopics.Add(examTopic);
            }

            // Trộn câu hỏi nếu được yêu cầu
            if (model.ShuffleQuestions)
            {
                var shuffled = questions.OrderBy(q => Guid.NewGuid()).ToList();
                for (int i = 0; i < shuffled.Count; i++)
                {
                    shuffled[i].QuestionOrder = i + 1;
                }
                questions = shuffled;
            }

            // Lưu vào database
            _context.ExamQuestions.AddRange(questions);
            await _context.SaveChangesAsync();

            return true;
        }

        // GET: Teacher/ExamManager/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .Include(e => e.ExamType)
                .Include(e => e.Creator)
                .Include(e => e.ExamTopics)
                    .ThenInclude(et => et.Topic)
                .Include(e => e.ExamQuestions)
                    .ThenInclude(eq => eq.Question)
                        .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(e => e.ExamId == id);

            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        // POST: Teacher/ExamManager/Publish/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Publish(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            exam.Status = "published";
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã xuất bản đề thi thành công!";
            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: Teacher/ExamManager/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            // Kiểm tra xem có học sinh đã làm bài chưa
            var hasStudentExams = await _context.StudentExams.AnyAsync(se => se.ExamId == id);
            if (hasStudentExams)
            {
                TempData["ErrorMessage"] = "Không thể xóa đề thi đã có học sinh làm bài!";
                return RedirectToAction(nameof(Index));
            }

            // Xóa ExamQuestions và ExamTopics
            var examQuestions = _context.ExamQuestions.Where(eq => eq.ExamId == id);
            var examTopics = _context.ExamTopics.Where(et => et.ExamId == id);

            _context.ExamQuestions.RemoveRange(examQuestions);
            _context.ExamTopics.RemoveRange(examTopics);
            _context.Exams.Remove(exam);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa đề thi thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
