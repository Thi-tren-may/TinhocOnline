using Microsoft.AspNetCore.Mvc;

namespace TinhocOnline.Areas.Student.Controllers
{
    [Area("Student")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
