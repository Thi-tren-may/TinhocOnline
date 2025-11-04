using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TinhocOnline.Models;
using TinhocOnline.Models.ViewModels;

namespace TinhocOnline.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    public class QuestionManagerController : Controller
    {
        private readonly DataContext _context;

        public QuestionManagerController(DataContext context)
        {
            _context = context;
        }

        // GET: Teacher/QuestionManager
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 20;
            
            // Lấy tổng số câu hỏi
            var totalItems = await _context.Questions.CountAsync();
            
            // Lấy dữ liệu với phân trang ở server
            var questions = await _context.Questions
                .Include(q => q.Answers)
                .Include(q => q.Creator)
                .Include(q => q.Topic)
                .OrderBy(q => q.QuestionId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            // Truyền thông tin phân trang qua ViewBag
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.TotalItems = totalItems;
            ViewBag.PageSize = pageSize;
            
            return View(questions);
        }

        // GET: Teacher/QuestionManager/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Creator)
                .Include(q => q.Topic)
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Teacher/QuestionManager/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.Users.Where(u => u.Role == "teacher"), "UserId", "FullName");
            ViewData["TopicId"] = new SelectList(_context.Topics.Where(t => t.Status == "active"), "TopicId", "TopicName");
            return View();
        }

        // POST: Teacher/QuestionManager/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionWithAnswersViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Tạo Question
                var question = new Question
                {
                    TopicId = model.TopicId,
                    QuestionText = model.QuestionText,
                    DifficultyLevel = model.DifficultyLevel,
                    CreatedBy = model.CreatedBy,
                    Status = model.Status
                };

                _context.Questions.Add(question);
                await _context.SaveChangesAsync();

                // Tạo 4 Answers
                var answers = new List<Answer>
                {
                    new Answer
                    {
                        QuestionId = question.QuestionId,
                        AnswerText = model.AnswerA,
                        IsCorrect = model.CorrectAnswer == "A",
                        AnswerOrder = "A"
                    },
                    new Answer
                    {
                        QuestionId = question.QuestionId,
                        AnswerText = model.AnswerB,
                        IsCorrect = model.CorrectAnswer == "B",
                        AnswerOrder = "B"
                    },
                    new Answer
                    {
                        QuestionId = question.QuestionId,
                        AnswerText = model.AnswerC,
                        IsCorrect = model.CorrectAnswer == "C",
                        AnswerOrder = "C"
                    },
                    new Answer
                    {
                        QuestionId = question.QuestionId,
                        AnswerText = model.AnswerD,
                        IsCorrect = model.CorrectAnswer == "D",
                        AnswerOrder = "D"
                    }
                };

                _context.Answers.AddRange(answers);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["CreatedBy"] = new SelectList(_context.Users.Where(u => u.Role == "teacher"), "UserId", "FullName", model.CreatedBy);
            ViewData["TopicId"] = new SelectList(_context.Topics.Where(t => t.Status == "active"), "TopicId", "TopicName", model.TopicId);
            return View(model);
        }

        // GET: Teacher/QuestionManager/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.QuestionId == id);
                
            if (question == null)
            {
                return NotFound();
            }
            
            ViewData["CreatedBy"] = new SelectList(_context.Users, "UserId", "Email", question.CreatedBy);
            ViewData["TopicId"] = new SelectList(_context.Topics, "TopicId", "TopicName", question.TopicId);
            return View(question);
        }

        // POST: Teacher/QuestionManager/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionId,TopicId,QuestionText,DifficultyLevel,CreatedBy,Status")] Question question)
        {
            if (id != question.QuestionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.QuestionId))
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
            ViewData["CreatedBy"] = new SelectList(_context.Users, "UserId", "Email", question.CreatedBy);
            ViewData["TopicId"] = new SelectList(_context.Topics, "TopicId", "TopicName", question.TopicId);
            return View(question);
        }

        // POST: Teacher/QuestionManager/EditAnswer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAnswer(int questionId, Dictionary<string, AnswerDto> answers)
        {
            if (!QuestionExists(questionId))
            {
                return NotFound();
            }

            // Kiểm tra có ít nhất 1 đáp án đúng
            if (!answers.Any(a => a.Value.IsCorrect))
            {
                ModelState.AddModelError("", "Phải có ít nhất 1 đáp án đúng");
                return RedirectToAction(nameof(Edit), new { id = questionId });
            }

            try
            {
                foreach (var answerDto in answers.Values)
                {
                    var answer = await _context.Answers.FindAsync(answerDto.AnswerId);
                    if (answer != null && answer.QuestionId == questionId)
                    {
                        answer.AnswerText = answerDto.AnswerText;
                        answer.IsCorrect = answerDto.IsCorrect;
                        _context.Update(answer);
                    }
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật đáp án thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
            }

            return RedirectToAction(nameof(Edit), new { id = questionId });
        }

        // GET: Teacher/QuestionManager/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Creator)
                .Include(q => q.Topic)
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Teacher/QuestionManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                // Chuyển status về inactive thay vì xóa
                question.Status = "inactive";
                _context.Update(question);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.QuestionId == id);
        }
    }
}
