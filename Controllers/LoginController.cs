using Microsoft.AspNetCore.Mvc;

namespace TinhocOnline.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult StudentLogin()
        {
            return View();
        }
        public IActionResult TeacherLogin()
        {
            return View();
        }
        public IActionResult AdminLogin()
        {
            return View();
        }
    }
}
