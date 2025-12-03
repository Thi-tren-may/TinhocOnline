using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TinhocOnline.Models;

namespace TinhocOnline.Areas.Admin.Controllers
{
    public class DashboardController : BaseAdminController
    {
        private readonly DataContext _context;

        public DashboardController(DataContext context)
        {
            _context = context;
        }

        // GET: Admin/Dashboard
        public async Task<IActionResult> Index()
        {
            var adminId = HttpContext.Session.GetInt32("UserId");

            // Tổng số người dùng
            var totalUsers = await _context.Users.CountAsync();

            // Số học sinh
            var totalStudents = await _context.Users
                .Where(u => u.Role == "student")
                .CountAsync();

            // Số giáo viên
            var totalTeachers = await _context.Users
                .Where(u => u.Role == "teacher")
                .CountAsync();

            // Số admin
            var totalAdmins = await _context.Users
                .Where(u => u.Role == "admin")
                .CountAsync();

            // Tổng số câu hỏi
            var totalQuestions = await _context.Questions.CountAsync();

            // Tổng số đề thi
            var totalExams = await _context.Exams.CountAsync();

            // Tổng số chủ đề
            var totalTopics = await _context.Topics.CountAsync();

            // Tổng số bài làm
            var totalAttempts = await _context.StudentExams.CountAsync();

            // Điểm trung bình toàn hệ thống
            var scores = await _context.StudentExams
                .Where(se => se.Score.HasValue)
                .Select(se => se.Score.Value)
                .ToListAsync();
            var averageScore = scores.Any() ? (double)scores.Average() / 100 : 0;

            // Người dùng mới (7 ngày gần nhất)
            var sevenDaysAgo = DateTime.Now.AddDays(-7);
            var newUsers = await _context.Users
                .Where(u => u.CreatedAt >= sevenDaysAgo)
                .CountAsync();

            // Phân bố người dùng theo role
            var usersByRole = await _context.Users
                .GroupBy(u => u.Role)
                .Select(g => new
                {
                    Role = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            // Đề thi phổ biến nhất (được làm nhiều nhất)
            var popularExamsData = await _context.StudentExams
                .Include(se => se.Exam)
                .GroupBy(se => new { se.ExamId, se.Exam.ExamName })
                .Select(g => new
                {
                    ExamTitle = g.Key.ExamName,
                    AttemptCount = g.Count(),
                    Scores = g.Select(se => se.Score).ToList()
                })
                .OrderByDescending(x => x.AttemptCount)
                .Take(5)
                .ToListAsync();

            var popularExams = popularExamsData.Select(p => new
            {
                p.ExamTitle,
                p.AttemptCount,
                AverageScore = p.Scores.Where(s => s.HasValue).Any() 
                    ? p.Scores.Where(s => s.HasValue).Average(s => s.Value) / 100 
                    : 0
            }).ToList();

            // Hoạt động gần đây
            var recentActivitiesData = await _context.StudentExams
                .Include(se => se.Student)
                .Include(se => se.Exam)
                .OrderByDescending(se => se.StartTime)
                .Take(10)
                .ToListAsync();

            var recentActivities = recentActivitiesData.Select(se => new
            {
                StudentName = se.Student.FullName,
                ExamTitle = se.Exam.ExamName,
                Score = se.Score.HasValue ? se.Score.Value / 100 : (decimal?)null,
                StartTime = se.StartTime
            }).ToList();

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalStudents = totalStudents;
            ViewBag.TotalTeachers = totalTeachers;
            ViewBag.TotalAdmins = totalAdmins;
            ViewBag.TotalQuestions = totalQuestions;
            ViewBag.TotalExams = totalExams;
            ViewBag.TotalTopics = totalTopics;
            ViewBag.TotalAttempts = totalAttempts;
            ViewBag.AverageScore = Math.Round(averageScore, 2);
            ViewBag.NewUsers = newUsers;
            ViewBag.UsersByRole = usersByRole;
            ViewBag.PopularExams = popularExams;
            ViewBag.RecentActivities = recentActivities;

            return View();
        }
    }
}
