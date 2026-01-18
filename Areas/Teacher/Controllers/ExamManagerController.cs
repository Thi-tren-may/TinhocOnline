using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TinhocOnline.Models;
using TinhocOnline.Models.ViewModels;

namespace TinhocOnline.Areas.Teacher.Controllers
{
    public class ExamManagerController : BaseTeacherController
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

        // ============================================
        // Tạo đề tùy chỉnh (Custom Mode)
        // ============================================

        // GET: Teacher/ExamManager/CreateQuick
        public async Task<IActionResult> CreateQuick()
        {
            var teacherId = HttpContext.Session.GetInt32("UserId");
            if (teacherId == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            // Load danh sách loại đề thi
            ViewBag.ExamTypes = new SelectList(
                await _context.ExamTypes.Where(et => et.Status == "active").ToListAsync(),
                "ExamTypeId",
                "TypeName"
            );

            // Load danh sách chủ đề
            var topics = await _context.Topics.Where(t => t.Status == "active").ToListAsync();
            ViewBag.Topics = topics;

            // Khởi tạo model với giá trị mặc định
            var model = new CreateExamViewModel
            {
                CreatedBy = teacherId.Value,
                TotalQuestions = 10,
                Duration = 45,
                PassingScore = 5.0M,
                CreateMode = "custom"
            };

            return View(model);
        }

        // POST: Teacher/ExamManager/CreateQuick
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuick(CreateExamViewModel model)
        {
            var teacherId = HttpContext.Session.GetInt32("UserId");
            if (teacherId == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            model.CreateMode = "custom";

            // Validate chủ đề đã chọn
            if (model.SelectedTopicIds == null || !model.SelectedTopicIds.Any())
            {
                ModelState.AddModelError("", "Vui lòng chọn ít nhất 1 chủ đề");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Tạo Exam (chỉ lưu cấu trúc)
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
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        CreatedBy = teacherId.Value,
                        Status = model.Status
                    };

                    _context.Exams.Add(exam);
                    await _context.SaveChangesAsync();

                    // Lưu ma trận chủ đề theo lựa chọn của giáo viên với logic điều chỉnh
                    var topicCount = model.SelectedTopicIds.Count;
                    var baseQuestions = model.TotalQuestions / topicCount; // Số câu cơ bản cho mỗi topic
                    var remainder = model.TotalQuestions % topicCount; // Số câu dư

                    for (int i = 0; i < model.SelectedTopicIds.Count; i++)
                    {
                        var topicId = model.SelectedTopicIds[i];
                        // Các topic đầu tiên sẽ nhận thêm 1 câu từ phần dư
                        var questionsForTopic = baseQuestions + (i < remainder ? 1 : 0);
                        
                        var examTopic = new ExamTopic
                        {
                            ExamId = exam.ExamId,
                            TopicId = topicId,
                            QuestionCount = questionsForTopic
                        };
                        _context.ExamTopics.Add(examTopic);
                    }

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Tạo cấu trúc đề thi thành công!";
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

        // ============================================
        // Tạo đề thi (Old - Deprecated)
        // ============================================

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

            // Validate theo chế độ tạo đề
            if (model.CreateMode == "quick")
            {
                // Quick mode: Validate tổng tỷ lệ độ khó = 100%
                var totalDifficulty = model.EasyPercentage + model.MediumPercentage + model.HardPercentage;
                if (totalDifficulty != 100)
                {
                    ModelState.AddModelError("", $"Tổng tỷ lệ độ khó phải bằng 100% (hiện tại: {totalDifficulty}%)");
                }
            }
            else if (model.CreateMode == "custom")
            {
                // Custom mode: Validate danh sách câu hỏi đã chọn
                if (model.SelectedQuestionIds == null || !model.SelectedQuestionIds.Any())
                {
                    ModelState.AddModelError("", "Vui lòng chọn ít nhất 1 câu hỏi");
                }
                else if (model.SelectedQuestionIds.Count != model.TotalQuestions)
                {
                    ModelState.AddModelError("", $"Số câu hỏi đã chọn ({model.SelectedQuestionIds.Count}) không khớp với tổng số câu ({model.TotalQuestions})");
                }
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

                    bool success;

                    if (model.CreateMode == "quick")
                    {
                        // Chế độ nhanh: Tự động phân bổ câu hỏi
                        var activeTopics = await _context.Topics.Where(t => t.Status == "active").ToListAsync();
                        var topicMatrix = new Dictionary<int, decimal>();
                        decimal percentage = Math.Round(100m / activeTopics.Count, 2);
                        
                        foreach (var topic in activeTopics)
                        {
                            topicMatrix[topic.TopicId] = percentage;
                        }

                        success = await GenerateExamQuestions(exam.ExamId, topicMatrix, model);
                    }
                    else
                    {
                        // Chế độ custom: Sử dụng câu hỏi đã chọn thủ công
                        success = await GenerateExamQuestionsFromSelectedIds(exam.ExamId, model.SelectedQuestionIds, model);
                    }

                    if (!success)
                    {
                        // Xóa exam nếu không thành công
                        _context.Exams.Remove(exam);
                        await _context.SaveChangesAsync();
                        
                        TempData["ErrorMessage"] = "Không thể tạo đề thi. Vui lòng thử lại.";
                        
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

        // GET: Teacher/ExamManager/CreateCustom
        [HttpGet]
        public async Task<IActionResult> CreateCustom()
        {
            var teacherId = HttpContext.Session.GetInt32("UserId");

            // Load ExamTypes
            ViewBag.ExamTypes = new SelectList(
                await _context.ExamTypes.Where(et => et.Status == "active").ToListAsync(),
                "ExamTypeId",
                "TypeName"
            );

            // Load Topics
            var topics = await _context.Topics.Where(t => t.Status == "active").ToListAsync();
            ViewBag.Topics = topics;

            // Khởi tạo model mới
            var model = new CreateExamViewModel
            {
                CreatedBy = teacherId.Value,
                TotalQuestions = 10,
                Duration = 45,
                PassingScore = 5.0M,
                CreateMode = "custom",
                CurrentPage = 1,
                PageSize = 10,
                SelectedQuestionIds = new List<int>()
            };

            // Load câu hỏi với bộ lọc
            await LoadQuestionsForModel(model);

            return View(model);
        }

        // Helper method: Load câu hỏi và phân trang
        private async Task LoadQuestionsForModel(CreateExamViewModel model)
        {
            var teacherId = HttpContext.Session.GetInt32("UserId");

            // Query câu hỏi
            var query = _context.Questions
                .Include(q => q.Topic)
                .Where(q => q.Status == "active" && q.CreatedBy == teacherId.Value);

            // Áp dụng bộ lọc
            if (model.FilterTopicId.HasValue && model.FilterTopicId.Value > 0)
            {
                query = query.Where(q => q.TopicId == model.FilterTopicId.Value);
            }

            if (!string.IsNullOrEmpty(model.FilterDifficulty))
            {
                query = query.Where(q => q.DifficultyLevel == model.FilterDifficulty);
            }

            if (!string.IsNullOrEmpty(model.FilterGradeLevel))
            {
                query = query.Where(q => q.GradeLevel == model.FilterGradeLevel);
            }

            // Đếm tổng số
            var totalItems = await query.CountAsync();
            model.TotalQuestionItems = totalItems;
            model.TotalPages = (int)Math.Ceiling(totalItems / (double)model.PageSize);

            // Đảm bảo CurrentPage hợp lệ
            if (model.CurrentPage < 1) model.CurrentPage = 1;
            if (model.CurrentPage > model.TotalPages && model.TotalPages > 0) 
                model.CurrentPage = model.TotalPages;

            // Phân trang
            var questions = await query
                .OrderBy(q => q.TopicId)
                .ThenBy(q => q.QuestionId)
                .Skip((model.CurrentPage - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(q => new QuestionItemDto
                {
                    QuestionId = q.QuestionId,
                    QuestionText = q.QuestionText,
                    DifficultyLevel = q.DifficultyLevel,
                    GradeLevel = q.GradeLevel,
                    TopicId = q.TopicId,
                    TopicName = q.Topic.TopicName,
                    IsSelected = model.SelectedQuestionIds.Contains(q.QuestionId)
                })
                .ToListAsync();

            model.Questions = questions;
        }

        // Helper method: Merge checkbox selections với danh sách đã chọn
        private void MergeQuestionSelections(CreateExamViewModel model)
        {
            // Lấy danh sách các QuestionId trên trang hiện tại
            var currentPageQuestionIds = model.Questions?.Select(q => q.QuestionId).ToList() ?? new List<int>();

            // Bước 1: Loại bỏ các câu hỏi của trang hiện tại khỏi danh sách cũ
            var selectedFromOtherPages = model.SelectedQuestionIds
                .Where(id => !currentPageQuestionIds.Contains(id))
                .ToList();

            // Bước 2: Thêm các câu hỏi được chọn từ trang hiện tại (checkbox)
            if (model.CurrentPageSelections != null && model.CurrentPageSelections.Any())
            {
                selectedFromOtherPages.AddRange(model.CurrentPageSelections);
            }

            // Bước 3: Cập nhật lại danh sách đã chọn (loại bỏ duplicate)
            model.SelectedQuestionIds = selectedFromOtherPages.Distinct().ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustom(CreateExamViewModel model)
        {
            var teacherId = HttpContext.Session.GetInt32("UserId");
            model.CreateMode = "custom";

            // Gộp lựa chọn từ trang hiện tại (checkbox) với các trang trước (hidden fields)
            MergeQuestionSelections(model);

            // Xử lý action "Lọc" hoặc "Phân trang"
            if (model.ActionButton == "filter" || model.ActionButton == "page")
            {
                ModelState.Clear();
                await LoadQuestionsForModel(model);
                
                ViewBag.ExamTypes = new SelectList(
                    await _context.ExamTypes.Where(et => et.Status == "active").ToListAsync(),
                    "ExamTypeId",
                    "TypeName"
                );
                ViewBag.Topics = await _context.Topics.Where(t => t.Status == "active").ToListAsync();
                
                return View(model);
            }
            // Xử lý action "Tạo đề thi"
            else if (model.ActionButton == "submit")
            {
                if (model.SelectedQuestionIds == null || !model.SelectedQuestionIds.Any())
                {
                    ModelState.AddModelError("", "Vui lòng chọn ít nhất 1 câu hỏi");
                }
                else if (model.SelectedQuestionIds.Count != model.TotalQuestions)
                {
                    ModelState.AddModelError("", $"Số câu hỏi đã chọn ({model.SelectedQuestionIds.Count}) phải bằng tổng số câu ({model.TotalQuestions})");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var exam = new Exam
                        {
                            ExamName = model.ExamName,
                            ExamTypeId = model.ExamTypeId,
                            GradeLevel = model.GradeLevel,
                            Duration = model.Duration,
                            TotalQuestions = model.TotalQuestions,
                            ShuffleQuestions = model.ShuffleQuestions,
                            ShuffleAnswers = model.ShuffleAnswers,
                            PassingScore = model.PassingScore,
                            StartDate = model.StartDate,
                            EndDate = model.EndDate,
                            Status = "draft",
                            CreatedBy = teacherId.Value,
                            CreatedAt = DateTime.Now
                        };

                        _context.Exams.Add(exam);
                        await _context.SaveChangesAsync();

                        // Tạo ExamQuestions và ExamTopics từ danh sách đã chọn
                        var success = await GenerateExamQuestionsFromSelectedIds(exam.ExamId, model.SelectedQuestionIds, model);

                        if (!success)
                        {
                            // Rollback nếu thất bại
                            _context.Exams.Remove(exam);
                            await _context.SaveChangesAsync();
                            ModelState.AddModelError("", "Không thể tạo đề thi với các câu hỏi đã chọn. Vui lòng thử lại.");
                        }
                        else
                        {
                            TempData["SuccessMessage"] = "Tạo đề thi thành công!";
                            return RedirectToAction(nameof(Details), new { id = exam.ExamId });
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", $"Lỗi: {ex.Message}");
                    }
                }

                // Reload view nếu validation thất bại
                await LoadQuestionsForModel(model);
                ViewBag.ExamTypes = new SelectList(
                    await _context.ExamTypes.Where(et => et.Status == "active").ToListAsync(),
                    "ExamTypeId",
                    "TypeName"
                );
                ViewBag.Topics = await _context.Topics.Where(t => t.Status == "active").ToListAsync();

                return View(model);
            }

            // Mặc định: reload trang hiện tại
            await LoadQuestionsForModel(model);
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

        // Tạo đề thi từ danh sách câu hỏi đã chọn thủ công (Custom mode)
        private async Task<bool> GenerateExamQuestionsFromSelectedIds(int examId, List<int> selectedQuestionIds, CreateExamViewModel model)
        {
            try
            {
                // Lấy thông tin các câu hỏi đã chọn
                var questions = await _context.Questions
                    .Include(q => q.Topic)
                    .Where(q => selectedQuestionIds.Contains(q.QuestionId) && q.Status == "active")
                    .ToListAsync();

                if (questions.Count != selectedQuestionIds.Count)
                {
                    return false; // Có câu hỏi không tồn tại hoặc không active
                }

                // Tạo ExamQuestions với thứ tự theo danh sách đã chọn
                var examQuestions = new List<ExamQuestion>();
                for (int i = 0; i < selectedQuestionIds.Count; i++)
                {
                    examQuestions.Add(new ExamQuestion
                    {
                        ExamId = examId,
                        QuestionId = selectedQuestionIds[i],
                        QuestionOrder = i + 1
                    });
                }

                // Trộn câu hỏi nếu được yêu cầu
                if (model.ShuffleQuestions)
                {
                    var shuffled = examQuestions.OrderBy(q => Guid.NewGuid()).ToList();
                    for (int i = 0; i < shuffled.Count; i++)
                    {
                        shuffled[i].QuestionOrder = i + 1;
                    }
                    examQuestions = shuffled;
                }

                // Lưu ExamQuestions
                _context.ExamQuestions.AddRange(examQuestions);

                // Tạo ExamTopics từ các câu hỏi đã chọn (nhóm theo topic)
                var topicGroups = questions.GroupBy(q => q.TopicId);
                foreach (var group in topicGroups)
                {
                    var examTopic = new ExamTopic
                    {
                        ExamId = examId,
                        TopicId = group.Key,
                        QuestionCount = group.Count()
                    };
                    _context.ExamTopics.Add(examTopic);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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

        // GET: Teacher/ExamManager/GetQuestions
        // API để lấy danh sách câu hỏi (dùng cho Custom mode)
        [HttpGet]
        public async Task<IActionResult> GetQuestions(int? topicId, string difficulty, string gradeLevel)
        {
            var teacherId = HttpContext.Session.GetInt32("UserId");
            if (teacherId == null)
            {
                return Json(new { error = "Unauthorized" });
            }

            // Query câu hỏi
            var query = _context.Questions
                .Include(q => q.Topic)
                .Where(q => q.Status == "active");

            // Lọc theo chủ đề nếu có
            if (topicId.HasValue && topicId.Value > 0)
            {
                query = query.Where(q => q.TopicId == topicId.Value);
            }

            // Lọc theo độ khó nếu có
            if (!string.IsNullOrEmpty(difficulty))
            {
                query = query.Where(q => q.DifficultyLevel == difficulty);
            }

            // Lọc theo lớp nếu có
            if (!string.IsNullOrEmpty(gradeLevel))
            {
                query = query.Where(q => q.GradeLevel == gradeLevel);
            }

            // Lấy danh sách và map sang DTO
            var questions = await query
                .OrderBy(q => q.TopicId)
                .ThenBy(q => q.QuestionId)
                .Select(q => new
                {
                    questionId = q.QuestionId,
                    questionText = q.QuestionText,
                    difficultyLevel = q.DifficultyLevel,
                    gradeLevel = q.GradeLevel,
                    topicId = q.TopicId,
                    topicName = q.Topic.TopicName
                })
                .ToListAsync();

            return Json(questions);
        }

        // GET: Teacher/ExamManager/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherId = HttpContext.Session.GetInt32("UserId");
            if (teacherId == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            var exam = await _context.Exams
                .Include(e => e.ExamTopics)
                    .ThenInclude(et => et.Topic)
                .FirstOrDefaultAsync(e => e.ExamId == id && e.CreatedBy == teacherId.Value);

            if (exam == null)
            {
                return NotFound();
            }

            var model = new CreateExamViewModel
            {
                ExamName = exam.ExamName,
                ExamTypeId = exam.ExamTypeId ?? 0,
                GradeLevel = exam.GradeLevel,
                Duration = exam.Duration,
                TotalQuestions = exam.TotalQuestions,
                EasyPercentage = exam.EasyPercentage,
                MediumPercentage = exam.MediumPercentage,
                HardPercentage = exam.HardPercentage,
                ShuffleQuestions = exam.ShuffleQuestions,
                ShuffleAnswers = exam.ShuffleAnswers,
                PassingScore = exam.PassingScore,
                StartDate = exam.StartDate,
                EndDate = exam.EndDate,
                Status = exam.Status,
                CreatedBy = exam.CreatedBy,
                CreateMode = "custom",
                SelectedTopicIds = exam.ExamTopics.Select(et => et.TopicId).ToList()
            };

            // Load ExamTypes and Topics
            ViewBag.ExamTypes = new SelectList(
                await _context.ExamTypes.Where(et => et.Status == "active").ToListAsync(),
                "ExamTypeId",
                "TypeName"
            );

            var topics = await _context.Topics.Where(t => t.Status == "active").ToListAsync();
            ViewBag.Topics = topics;
            ViewBag.ExamId = exam.ExamId;

            return View(model);
        }

        // POST: Teacher/ExamManager/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateExamViewModel model)
        {
            var teacherId = HttpContext.Session.GetInt32("UserId");
            if (teacherId == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            var exam = await _context.Exams
                .Include(e => e.ExamQuestions)
                .Where(e => e.ExamId == id && e.CreatedBy == teacherId.Value)
                .FirstOrDefaultAsync();

            if (exam == null)
            {
                return NotFound();
            }

            model.CreateMode = "custom";

            // Validate chủ đề đã chọn
            if (model.SelectedTopicIds == null || !model.SelectedTopicIds.Any())
            {
                ModelState.AddModelError("", "Vui lòng chọn ít nhất 1 chủ đề");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update exam info
                    exam.ExamName = model.ExamName;
                    exam.ExamTypeId = model.ExamTypeId;
                    exam.GradeLevel = model.GradeLevel;
                    exam.Duration = model.Duration;
                    exam.TotalQuestions = model.TotalQuestions;
                    exam.EasyPercentage = model.EasyPercentage;
                    exam.MediumPercentage = model.MediumPercentage;
                    exam.HardPercentage = model.HardPercentage;
                    exam.ShuffleQuestions = model.ShuffleQuestions;
                    exam.ShuffleAnswers = model.ShuffleAnswers;
                    exam.PassingScore = model.PassingScore;
                    exam.StartDate = model.StartDate;
                    exam.EndDate = model.EndDate;
                    exam.Status = model.Status;

                    // Remove existing exam topics (không có ExamQuestions nữa)
                    var existingTopics = _context.ExamTopics.Where(et => et.ExamId == exam.ExamId);
                    _context.ExamTopics.RemoveRange(existingTopics);

                    // Thêm lại ExamTopics mới với logic đảm bảo tổng đúng
                    var topicCount = model.SelectedTopicIds.Count;
                    var baseQuestions = model.TotalQuestions / topicCount;
                    var remainder = model.TotalQuestions % topicCount;

                    for (int i = 0; i < model.SelectedTopicIds.Count; i++)
                    {
                        var topicId = model.SelectedTopicIds[i];
                        var questionsForTopic = baseQuestions + (i < remainder ? 1 : 0);
                        
                        var examTopic = new ExamTopic
                        {
                            ExamId = exam.ExamId,
                            TopicId = topicId,
                            QuestionCount = questionsForTopic
                        };
                        _context.ExamTopics.Add(examTopic);
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật cấu trúc đề thi thành công!";
                    return RedirectToAction(nameof(Details), new { id = exam.ExamId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Lỗi: {ex.Message}");
                }
            }

            // Reload supporting data if validation failed
            ViewBag.ExamTypes = new SelectList(
                await _context.ExamTypes.Where(et => et.Status == "active").ToListAsync(),
                "ExamTypeId",
                "TypeName"
            );
            ViewBag.Topics = await _context.Topics.Where(t => t.Status == "active").ToListAsync();
            ViewBag.ExamId = id;

            return View(model);
        }
    }
}
