using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TinhocOnline.Models;
using TinhocOnline.Services;

namespace TinhocOnline.Areas.Student.Controllers
{
    [Area("Student")]
    public class AIFeedbackController : Controller
    {
        private readonly DataContext _context;
        private readonly GeminiService _geminiService;

        public AIFeedbackController(DataContext context, GeminiService geminiService)
        {
            _context = context;
            _geminiService = geminiService;
        }

        // GET: Student/AIFeedback/GetFeedback/5
        public async Task<IActionResult> GetFeedback(int? studentExamId)
        {
            var studentId = HttpContext.Session.GetInt32("UserId");
            if (studentId == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            if (studentExamId == null)
            {
                return NotFound();
            }

            try
            {
                // Load StudentExam với tất cả thông tin cần thiết
                var studentExam = await _context.StudentExams
                    .Include(se => se.Exam)
                    .Include(se => se.StudentExamQuestions)
                        .ThenInclude(seq => seq.Question)
                            .ThenInclude(q => q.Topic)
                    .Include(se => se.StudentExamQuestions)
                        .ThenInclude(seq => seq.Question)
                            .ThenInclude(q => q.Answers)
                    .Include(se => se.StudentAnswers)
                        .ThenInclude(sa => sa.Answer)
                    .FirstOrDefaultAsync(se => se.StudentExamId == studentExamId && se.StudentId == studentId);

                if (studentExam == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy bài thi hoặc bạn không có quyền truy cập.";
                    return RedirectToAction("Index", "Exam");
                }

                if (studentExam.Status != "completed")
                {
                    TempData["ErrorMessage"] = "Bài thi chưa hoàn thành. Vui lòng hoàn thành bài thi trước khi xem đánh giá.";
                    return RedirectToAction("ViewResults", "Exam", new { id = studentExamId });
                }

                // Lấy kết quả phân tích từ DB (đã lưu khi submit)
                var analysis = await _geminiService.GetExamAnalysisAsync(studentExamId.Value);

                string analysisJson;

                if (analysis == null)
                {
                    // Chưa có phân tích -> Gọi AI ngay bây giờ
                    var topics = studentExam.StudentExamQuestions
                        .Select(seq => seq.Question.Topic)
                        .Where(t => t != null)
                        .Distinct()
                        .Select(t => new Services.DTOs.TopicDto 
                        { 
                            TopicId = t!.TopicId, 
                            TopicName = t.TopicName 
                        })
                        .ToList();

                    var questions = studentExam.StudentExamQuestions
                        .Select(seq => new Services.DTOs.QuestionDto
                        {
                            QuestionId = seq.QuestionId,
                            TopicName = seq.Question.Topic?.TopicName ?? "N/A",
                            QuestionText = seq.Question.QuestionText,
                            CorrectAnswer = seq.Question.Answers.FirstOrDefault(a => a.IsCorrect)?.AnswerText ?? ""
                        }).ToList();

                    var userAnswers = studentExam.StudentAnswers
                        .Select(sa => new Services.DTOs.UserAnswerDto
                        {
                            QuestionId = sa.QuestionId,
                            UserAnswer = sa.Answer?.AnswerText ?? ""
                        }).ToList();

                    var analysisRequest = new Services.DTOs.ExamAnalysisRequest
                    {
                        Topics = topics,
                        Questions = questions,
                        UserAnswers = userAnswers
                    };

                    // Gọi AI
                    analysisJson = await _geminiService.AnalyzeExamResultAsync(analysisRequest);

                    // Lưu vào DB
                    await _geminiService.SaveAnalysisResultAsync(
                        studentExamId.Value,
                        studentId.Value,
                        studentExam.ExamId,
                        analysisJson
                    );
                }
                else
                {
                    // Đã có phân tích trong DB
                    analysisJson = analysis.AnalysisResultJson;
                }

                // Parse JSON và hiển thị
                ViewBag.AIResponse = analysisJson;
                ViewBag.StudentExam = studentExam;

                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi khi tải kết quả phân tích: {ex.Message}";
                return RedirectToAction("ViewResults", "Exam", new { id = studentExamId });
            }
        }
    }
}
