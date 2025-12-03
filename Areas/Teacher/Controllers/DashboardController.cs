using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TinhocOnline.Models;

namespace TinhocOnline.Areas.Teacher.Controllers
{
    public class DashboardController : BaseTeacherController
    {
        private readonly DataContext _context;

        public DashboardController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var teacherId = HttpContext.Session.GetInt32("UserId");

            // Tổng số câu hỏi đã tạo
            var totalQuestions = await _context.Questions
                .Where(q => q.CreatedBy == teacherId.Value)
                .CountAsync();

            // Tổng số đề thi đã tạo
            var totalExams = await _context.Exams
                .Where(e => e.CreatedBy == teacherId.Value)
                .CountAsync();

            // Tổng số học sinh đã làm bài thi của giáo viên này
            var totalStudents = await _context.StudentExams
                .Include(se => se.Exam)
                .Where(se => se.Exam.CreatedBy == teacherId.Value)
                .Select(se => se.StudentId)
                .Distinct()
                .CountAsync();

            // Số bài thi được làm
            var totalAttempts = await _context.StudentExams
                .Include(se => se.Exam)
                .Where(se => se.Exam.CreatedBy == teacherId.Value)
                .CountAsync();

            // Câu hỏi theo chủ đề
            var questionsByTopic = await _context.Questions
                .Include(q => q.Topic)
                .Where(q => q.CreatedBy == teacherId.Value)
                .GroupBy(q => q.Topic.TopicName)
                .Select(g => new
                {
                    TopicName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            // Đề thi gần nhất
            var recentExams = await _context.Exams
                .Include(e => e.ExamType)
                .Where(e => e.CreatedBy == teacherId.Value)
                .OrderByDescending(e => e.CreatedAt)
                .Take(5)
                .Select(e => new
                {
                    Title = e.ExamName,
                    ExamType = e.ExamType.TypeName,
                    Duration = e.Duration,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();

            ViewBag.TotalQuestions = totalQuestions;
            ViewBag.TotalExams = totalExams;
            ViewBag.TotalStudents = totalStudents;
            ViewBag.TotalAttempts = totalAttempts;
            ViewBag.QuestionsByTopic = questionsByTopic;
            ViewBag.RecentExams = recentExams;

            return View();
        }
    }
}
