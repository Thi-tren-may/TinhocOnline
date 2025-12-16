using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TinhocOnline.Services;
using TinhocOnline.Models;
using TinhocOnline.Areas.Student.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace TinhocOnline.Areas.Student.Controllers
{
    [Area("Student")]
    public class LearningProgressController : Controller
    {
        private readonly GeminiService _geminiService;
        private readonly DataContext _context;

        public LearningProgressController(GeminiService geminiService, DataContext context)
        {
            _geminiService = geminiService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            var viewModel = new ReviewIndexViewModel();

            // Lấy tất cả các phân tích của học sinh
            var analyses = await _context.StudentExamAnalyses
                .Where(a => a.UserId == userId)
                .Include(a => a.Exam)
                .OrderByDescending(a => a.AnalyzedAt)
                .ToListAsync();

            if (analyses.Any())
            {
                // Tính toán tiến độ tổng quan
                viewModel.Progress.TotalExamsTaken = analyses.Count;
                viewModel.Progress.TotalQuestionsAnswered = analyses.Sum(a => a.TotalQuestions);
                viewModel.Progress.TotalCorrectAnswers = analyses.Sum(a => a.CorrectAnswers);
                viewModel.Progress.AverageAccuracy = analyses.Average(a => a.AccuracyPercentage);
                viewModel.Progress.LastExamDate = analyses.FirstOrDefault()?.AnalyzedAt;

                // Lấy các phân tích gần đây
                viewModel.RecentAnalyses = analyses.Take(10).Select(a => new RecentAnalysis
                {
                    StudentExamId = a.StudentExamId,
                    ExamName = a.Exam?.ExamName ?? "N/A",
                    AnalyzedAt = a.AnalyzedAt,
                    TotalQuestions = a.TotalQuestions,
                    CorrectAnswers = a.CorrectAnswers,
                    AccuracyPercentage = a.AccuracyPercentage
                }).ToList();

                // Lấy chủ đề cần ôn (từ StudentTopicPerformance)
                var topicPerformances = await _geminiService.GetUserTopicPerformanceAsync(userId.Value);
                
                viewModel.RecommendedTopics = topicPerformances
                    .OrderBy(tp => tp.AccuracyPercentage)
                    .Select(tp => new TopicRecommendation
                    {
                        TopicName = tp.TopicName,
                        AccuracyPercentage = tp.AccuracyPercentage,
                        TotalAttempts = tp.TotalAttempts,
                        TotalQuestions = tp.TotalQuestions,
                        Priority = tp.AccuracyPercentage < 50 ? "High" : tp.AccuracyPercentage < 70 ? "Medium" : "Low"
                    }).ToList();

                // Gọi AI để đánh giá tổng quan
                string aiAssessmentJson = null;
                try
                {
                    aiAssessmentJson = await _geminiService.AnalyzeOverallProgressAsync(userId.Value);
                    if (!string.IsNullOrEmpty(aiAssessmentJson))
                    {
                        // Parse JSON từ AI với options case-insensitive
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        var assessmentData = JsonSerializer.Deserialize<OverallAssessment>(aiAssessmentJson, options);
                        viewModel.Assessment = assessmentData;
                    }
                }
                catch (Exception ex)
                {
                    // Log error nhưng không fail trang
                    Console.WriteLine($"Error getting AI assessment: {ex.Message}");
                    ViewBag.AIError = ex.Message;
                    ViewBag.AIJson = aiAssessmentJson ?? "null";
                }
            }
            else
            {
                // Chưa có bài thi nào -> recommend hết các chủ đề từ DB
                var allTopicsFromDb = await _context.Topics
                    .OrderBy(t => t.TopicName)
                    .Select(t => t.TopicName)
                    .ToListAsync();

                viewModel.RecommendedTopics = allTopicsFromDb.Select(topic => new TopicRecommendation
                {
                    TopicName = topic,
                    AccuracyPercentage = 0,
                    TotalAttempts = 0,
                    TotalQuestions = 0,
                    Priority = "High"
                }).ToList();
            }

            return View(viewModel);
        }
    }
}
