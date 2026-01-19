using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TinhocOnline.Models;

namespace TinhocOnline.Areas.Admin.Controllers
{
    public class StudentManagerController : BaseAdminController
    {
        private readonly DataContext _context;

        public StudentManagerController(DataContext context)
        {
            _context = context;
        }

        // GET: Admin/StudentManager
        public async Task<IActionResult> Index()
        {

            var result = await _context.Users
                .Where(u => u.Role == "student")
                .ToListAsync();

            return View(result);
        }

        // GET: Admin/StudentManager/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            // Thống kê học tập
            var studentExams = await _context.StudentExams
                .Where(se => se.StudentId == id)
                .ToListAsync();

            // Tổng số bài thi đã làm
            ViewBag.TotalExams = studentExams.Count;

            // Số bài thi đã hoàn thành (có điểm)
            var completedExams = studentExams.Where(se => se.Score.HasValue).ToList();
            ViewBag.CompletedExams = completedExams.Count;

            // Điểm trung bình
            ViewBag.AverageScore = completedExams.Any() 
                ? Math.Round(completedExams.Average(se => se.Score.Value), 2) 
                : 0;

            return View(user);
        }

        // GET: Admin/StudentManager/Create
        public IActionResult Create()
        {
            var user = new User { Role = "student", Status = "active" };
            return View(user);
        }

        // POST: Admin/StudentManager/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,Password,FullName,Email,Role,Status,DateOfBirth,Gender,PhoneNumber,Address")] User user)
        {
            // Đảm bảo Role luôn là "student"
            user.Role = "student";
            
            // Kiểm tra username đã tồn tại
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại trong hệ thống");
                return View(user);
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Admin/StudentManager/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Admin/StudentManager/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Password,FullName,Email,Role,Status,DateOfBirth,Gender,PhoneNumber,Address")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            // Kiểm tra username đã tồn tại (trừ user hiện tại)
            if (await _context.Users.AnyAsync(u => u.Username == user.Username && u.UserId != user.UserId))
            {
                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại trong hệ thống");
                return View(user);
            }

            // Xóa lỗi validation cho Password nếu nó trống (không bắt buộc khi edit)
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                ModelState.Remove("Password");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy user hiện tại từ database
                    var existingUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    // Nếu password trống, giữ nguyên password cũ
                    if (string.IsNullOrWhiteSpace(user.Password))
                    {
                        user.Password = existingUser.Password;
                    }

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Admin/StudentManager/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/StudentManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
