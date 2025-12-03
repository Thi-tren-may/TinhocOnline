using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TinhocOnline.Models;

namespace TinhocOnline.Areas.Student.Controllers
{
    [Area("Student")]
    public class StatisticsController : Controller
    {
        private readonly DataContext _context;

        public StatisticsController(DataContext context)
        {
            _context = context;
        }

        // GET: Student/Statistics - Trang thống kê (cần đăng nhập)
        public async Task<IActionResult> Index()
        {
            var studentId = HttpContext.Session.GetInt32("UserId");
            if (studentId == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            // Tổng số bài thi đã làm
            var totalExams = await _context.StudentExams
                .Where(se => se.StudentId == studentId.Value)
                .CountAsync();

            // Điểm trung bình
            var studentExamsWithScore = await _context.StudentExams
                .Where(se => se.StudentId == studentId.Value && se.Score.HasValue)
                .Select(se => se.Score.Value)
                .ToListAsync();
            
            var averageScore = studentExamsWithScore.Any() ? (double)studentExamsWithScore.Average() : 0;

            // Số bài đạt
            var passedExams = await _context.StudentExams
                .Include(se => se.Exam)
                .Where(se => se.StudentId == studentId.Value && se.Score >= se.Exam.PassingScore)
                .CountAsync();

            // Số bài không đạt
            var failedExams = totalExams - passedExams;

            // Điểm cao nhất
            var highestScore = studentExamsWithScore.Any() ? studentExamsWithScore.Max() : 0;

            // Điểm thấp nhất
            var lowestScore = studentExamsWithScore.Any() ? studentExamsWithScore.Min() : 0;

            // 5 bài thi gần nhất
            var recentExams = await _context.StudentExams
                .Include(se => se.Exam)
                .Where(se => se.StudentId == studentId.Value)
                .OrderByDescending(se => se.StartTime)
                .Take(5)
                .Select(se => new
                {
                    ExamTitle = se.Exam.ExamName,
                    Score = se.Score,
                    StartTime = se.StartTime,
                    EndTime = se.EndTime,
                    Status = se.Score >= se.Exam.PassingScore ? "Đạt" : "Không đạt"
                })
                .ToListAsync();

            ViewBag.TotalExams = totalExams;
            ViewBag.AverageScore = Math.Round(averageScore / 100, 2);
            ViewBag.PassedExams = passedExams;
            ViewBag.FailedExams = failedExams;
            ViewBag.HighestScore = highestScore / 100;
            ViewBag.LowestScore = lowestScore / 100;
            ViewBag.RecentExams = recentExams;

            return View();
        }
    }
}
