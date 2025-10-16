using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TinhocOnline.Models;

namespace TinhocOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class TeachersController : Controller
    {
        private readonly DataContext _context;

        public TeachersController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Lọc chỉ những người có Role = 'Teacher'
            var teachers = _context.tblUsers
                .Where(u => u.Role == "Teacher")
                .ToList();

            return View(teachers);
        }

        // GET: Xác nhận xóa
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            // 🔹 Tìm giáo viên theo UserID (vì bạn truyền từ danh sách Users)
            var teacher = _context.tblTeachers
                .Include(t => t.User)
                .FirstOrDefault(t => t.UserID == id);

            if (teacher == null)
                return NotFound();

            return View(teacher);
        }

                // POST: Thực hiện xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // 🔹 1. Lấy giáo viên theo UserID
            var teacher = _context.tblTeachers.FirstOrDefault(t => t.UserID == id);

            if (teacher != null)
            {
                // 2️⃣ Xóa giáo viên trước
                _context.tblTeachers.Remove(teacher);
                _context.SaveChanges();  // ⚠️ Lưu ngay để tránh lỗi FK
            }

            // 3️⃣ Sau đó xóa user
            var user = _context.tblUsers.FirstOrDefault(u => u.UserID == id);
            if (user != null)
            {
                _context.tblUsers.Remove(user);
                _context.SaveChanges();  // Xóa cha sau
            }

            return RedirectToAction("Index");
        }

    }
}
