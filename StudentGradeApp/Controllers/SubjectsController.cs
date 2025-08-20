using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentGradeApp.Data;
using StudentGradeApp.Models;
namespace StudentGradesAppCore.Controllers
{

    public class SubjectsController : Controller
    {
        private readonly SchoolContext _context;

        public SubjectsController(SchoolContext context) => _context = context;

        public async Task<IActionResult> Index() =>
            View(await _context.Subjects.ToListAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var subject = await _context.Subjects
                .Include(s => s.Grades)
                .ThenInclude(g => g.Student)
                .FirstOrDefaultAsync(m => m.SubjectId == id);
            if (subject == null) return NotFound();
            return View(subject);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubjectName")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return NotFound();
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubjectId,SubjectName")] Subject subject)
        {
            if (id != subject.SubjectId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return NotFound();
            return View(subject);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}