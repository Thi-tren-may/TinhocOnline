using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TinhocOnline.Models;
using TinhocOnline.Services;
using TinhocOnline.Services.DTOs;

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
                // 1. Load StudentExam với tất cả thông tin cần thiết
                var studentExam = await _context.StudentExams
                    .Include(se => se.Exam)
                        .ThenInclude(e => e.ExamTopics)
                            .ThenInclude(et => et.Topic)
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

                // 2. Chuẩn bị data cho ExamAnalysisRequest
                // Lấy danh sách topics
                var topics = studentExam.Exam.ExamTopics
                    .Select(et => new TopicDto
                    {
                        TopicId = et.TopicId,
                        TopicName = et.Topic.TopicName
                    })
                    .ToList();

                // Lấy danh sách questions với đáp án đúng
                var questions = studentExam.StudentExamQuestions
                    .Select(seq => new QuestionDto
                    {
                        QuestionId = seq.QuestionId,
                        TopicName = seq.Question.Topic?.TopicName ?? "N/A",
                        QuestionText = seq.Question.QuestionText,
                        CorrectAnswer = seq.Question.Answers.FirstOrDefault(a => a.IsCorrect)?.AnswerText ?? "N/A"
                    })
                    .ToList();

                // Lấy danh sách answers của học sinh
                var userAnswers = studentExam.StudentAnswers
                    .Select(sa => new UserAnswerDto
                    {
                        QuestionId = sa.QuestionId,
                        UserAnswer = sa.Answer?.AnswerText ?? "Không trả lời"
                    })
                    .ToList();

                // Tạo request object
                var analysisRequest = new ExamAnalysisRequest
                {
                    Topics = topics,
                    Questions = questions,
                    UserAnswers = userAnswers,
                    Model = null // Sử dụng model mặc định từ appsettings.json
                };

                // 3. Gọi Gemini API
                var aiResponse = await _geminiService.AnalyzeExamResultAsync(analysisRequest);

                // 4. Parse JSON response
                // TODO: Implement JSON parsing to ExamAnalysisResponse
                // For now, pass raw response to view
                ViewBag.AIResponse = aiResponse;
                ViewBag.StudentExam = studentExam;

                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi khi phân tích bài thi: {ex.Message}";
                return RedirectToAction("ViewResults", "Exam", new { id = studentExamId });
            }
        }
    }
}
