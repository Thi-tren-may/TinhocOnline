using Microsoft.AspNetCore.Mvc;

namespace TinhocOnline.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
