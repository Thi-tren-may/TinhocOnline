using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TinhocOnline.Models;

namespace TinhocOnline.Controllers
{
    public class AuthController : Controller
    {
        private readonly DataContext _context;

        public AuthController(DataContext context)
        {
            _context = context;
        }

        // GET: Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string role)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin";
                return View();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password && u.Role == role);

            if (user == null)
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View();
            }

            if (user.Status != "active")
            {
                ViewBag.Error = "Tài khoản của bạn đã bị khóa";
                return View();
            }

            // Lưu vào Session - ĐƠN GIẢN NHẤT
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("FullName", user.FullName);
            HttpContext.Session.SetString("Role", user.Role);

            // Redirect theo role
            if (user.Role == "admin")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else if (user.Role == "teacher")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Teacher" });
            }
            else if (user.Role == "student")
            {
                return RedirectToAction("Index", "Statistics", new { area = "Student" });
            }

            return RedirectToAction("Index", "Home", new { area = "Student" });
        }

        // GET: Auth/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user, string confirmPassword, string role)
        {
            // Kiểm tra role hợp lệ (chỉ cho phép student và teacher đăng ký)
            if (role != "student" && role != "teacher")
            {
                ViewBag.Error = "Vai trò không hợp lệ";
                return View(user);
            }

            user.Role = role;

            // Kiểm tra password khớp
            if (user.Password != confirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp";
                return View(user);
            }

            // Kiểm tra username đã tồn tại
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại";
                return View(user);
            }

            // Kiểm tra email đã tồn tại
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                ViewBag.Error = "Email đã được sử dụng";
                return View(user);
            }

            // Set default status
            user.Status = "active";

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            ViewBag.Success = "Đăng ký thành công! Vui lòng đăng nhập.";
            return View();
        }

        // GET: Auth/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
