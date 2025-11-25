using Microsoft.AspNetCore.Mvc;

namespace TinhocOnline.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home/Index - Trang chủ công khai (không cần đăng nhập)
        public IActionResult Index()
        {
            return View();
        }
    }
}
