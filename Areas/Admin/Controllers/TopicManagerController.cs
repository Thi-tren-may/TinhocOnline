using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TinhocOnline.Models;

namespace TinhocOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TopicManagerController : Controller
    {
        private readonly DataContext _context;

        public TopicManagerController(DataContext context)
        {
            _context = context;
        }

        // GET: Admin/TopicManager
        public async Task<IActionResult> Index()
        {
            return View(await _context.Topics.ToListAsync());
        }

        // GET: Admin/TopicManager/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics
                .FirstOrDefaultAsync(m => m.TopicId == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // GET: Admin/TopicManager/Create
        public IActionResult Create()
        {
            // Lấy mã chủ đề cuối cùng để tạo mã mới
            var lastTopic = _context.Topics
                .OrderByDescending(t => t.TopicCode)
                .FirstOrDefault();
            
            string nextTopicCode = "A";
            if (lastTopic != null && !string.IsNullOrEmpty(lastTopic.TopicCode))
            {
                char lastChar = lastTopic.TopicCode[0];
                if (lastChar < 'Z')
                {
                    nextTopicCode = ((char)(lastChar + 1)).ToString();
                }
                else
                {
                    nextTopicCode = "AA"; // hoặc có thể return error
                }
            }
            
            ViewBag.NextTopicCode = nextTopicCode;
            
            var topic = new Topic { TopicCode = nextTopicCode };
            return View(topic);
        }

        // POST: Admin/TopicManager/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TopicCode,TopicName,Description,Status")] Topic topic)
        {
            // Đảm bảo TopicCode được tạo tự động nếu trống
            if (string.IsNullOrEmpty(topic.TopicCode))
            {
                var lastTopic = _context.Topics
                    .OrderByDescending(t => t.TopicCode)
                    .FirstOrDefault();

                string nextTopicCode = "A";
                if (lastTopic != null && !string.IsNullOrEmpty(lastTopic.TopicCode))
                {
                    char lastChar = lastTopic.TopicCode[0];
                    if (lastChar < 'Z')
                    {
                        nextTopicCode = ((char)(lastChar + 1)).ToString();
                    }
                    else
                    {
                        nextTopicCode = "AA";
                    }
                }

                topic.TopicCode = nextTopicCode;
            }
            
            // chuyển về chữ thường
            topic.Status = topic.Status.ToLower();
            
            if (ModelState.IsValid)
            {
                _context.Add(topic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.NextTopicCode = topic.TopicCode;
            return View(topic);
        }

        // GET: Admin/TopicManager/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            return View(topic);
        }

        // POST: Admin/TopicManager/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TopicId,TopicCode,TopicName,Description,Status")] Topic topic)
        {
            if (id != topic.TopicId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(topic.TopicId))
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
            return View(topic);
        }

        // GET: Admin/TopicManager/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics
                .FirstOrDefaultAsync(m => m.TopicId == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // POST: Admin/TopicManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic != null)
            {
                _context.Topics.Remove(topic);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TopicExists(int id)
        {
            return _context.Topics.Any(e => e.TopicId == id);
        }
    }
}
