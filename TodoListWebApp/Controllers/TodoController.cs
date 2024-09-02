using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodoListWebApp.Data;
using TodoListWebApp.Models;

namespace TodoListWebApp.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly TodoListDbContext _context;

        public TodoController(TodoListDbContext context)
        {
            _context = context;
        }

        // GET: Todo
        public async Task<IActionResult> Index(string importance, bool? completed, string searchString, string category, DateTime? dueDate)
        {
            if (_context.TodoModel is null)
            {
                return Problem("Entity set 'TodoListDbContext.TodoModel'  is null.");
            }

            var items = from t in _context.TodoModel
                        select t;

            if (!string.IsNullOrEmpty(importance))
            {
                items = items.Where(s => s.Importance == importance);
            }

            if (completed.HasValue)
            {
                items = items.Where(s => s.Completed == completed.Value);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Description.ToLower().Contains(searchString.ToLower()));
            }

            if (!string.IsNullOrEmpty(category))
            {
                items = items.Where(s => s.Category == category);
            }

            if (dueDate.HasValue)
            {
                items = items.Where(s => s.DueDate.Date == dueDate);
            }
                        
            //sort by priority
            items = items.OrderBy(s => s.Completed)
                         .ThenByDescending(s => s.Importance == "High")
                         .ThenByDescending(s => s.Importance == "Medium")
                         .ThenByDescending(s => s.Importance == "Low")
                         .ThenBy(s => s.DueDate);

            return View(await items.ToListAsync());
        }

        // GET: Todo/Details/id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TodoModel == null)
            {
                return NotFound();
            }

            var todoModel = await _context.TodoModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoModel == null)
            {
                return NotFound();
            }

            return View(todoModel);
        }

        // GET: Todo/Create
        public IActionResult Create()
        {
            ViewData["PriorityList"] = new SelectList(new[] { "High", "Medium", "Low" });
            ViewData["CategoryList"] = new SelectList(new[] { "Home", "School", "Work", "Misc" });
            return View();
        }

        // POST: Todo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Importance,Id,Description,Completed,Category,DueDate")] TodoModel todoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todoModel);
        }

        // GET: Todo/Edit/id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TodoModel == null)
            {
                return NotFound();
            }

            var todoModel = await _context.TodoModel.FindAsync(id);
            if (todoModel == null)
            {
                return NotFound();
            }
            //populate priority list for drop-down
            ViewData["PriorityList"] = new SelectList(new[] { "High", "Medium", "Low" }, todoModel.Importance);

            //populate category list for drop-down
            ViewData["CategoryList"] = new SelectList(new[] { "Home", "School", "Work", "Misc" }, todoModel.Category);

            return View(todoModel);
        }

        // POST: Todo/Edit/id
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Importance,Id,Description,Completed,Category,DueDate")] TodoModel todoModel)
        {
            if (id != todoModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoModelExists(todoModel.Id))
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
            //populate priority list for drop-down
            ViewData["PriorityList"] = new SelectList(new[] { "High", "Medium", "Low" }, todoModel.Importance);
            //populate category list for drop-down
            ViewData["CategoryList"] = new SelectList(new[] { "Home", "School", "Work", "Misc" }, todoModel.Category);

            return View(todoModel);
        }

        // GET: Todo/Delete/id
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TodoModel == null)
            {
                return NotFound();
            }

            var todoModel = await _context.TodoModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoModel == null)
            {
                return NotFound();
            }

            return View(todoModel);
        }

        // POST: Todo/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TodoModel == null)
            {
                return Problem("Entity set 'TodoListDbContext.TodoModel'  is null.");
            }
            var todoModel = await _context.TodoModel.FindAsync(id);
            if (todoModel != null)
            {
                _context.TodoModel.Remove(todoModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoModelExists(int id)
        {
            return (_context.TodoModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}