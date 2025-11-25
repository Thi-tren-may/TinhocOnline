using Microsoft.AspNetCore.Mvc;

namespace TinhocOnline.Areas.Student.Controllers
{
    [Area("Student")]
    public class StatisticsController : Controller
    {
        // GET: Student/Statistics - Trang thống kê (cần đăng nhập)
        public IActionResult Index()
        {
            var studentId = HttpContext.Session.GetInt32("UserId");
            if (studentId == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }
            
            return View();
        }
    }
}
