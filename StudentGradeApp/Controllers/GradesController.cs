using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentGradeApp.Data;
using StudentGradeApp.Models;

namespace StudentGradesAppCore.Controllers
{
    public class GradesController : Controller
    {
        private readonly SchoolContext _context;

        public GradesController(SchoolContext context) => _context = context;

        public async Task<IActionResult> Index(string search, string filter = "ALL")
        {
            var grades = _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                grades = grades.Where(g => g.Student.StudentName.Contains(search));

            if (filter == "PASS")
                grades = grades.Where(g => g.GradeValue >= 75);
            else if (filter == "FAIL")
                grades = grades.Where(g => g.GradeValue < 75);

            ViewBag.Filter = filter;
            ViewBag.Search = search;

            return View(await grades.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var grade = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .FirstOrDefaultAsync(m => m.GradeId == id);
            if (grade == null) return NotFound();
            return View(grade);
        }

        public IActionResult Create()
        {
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentName");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,SubjectId,GradeValue")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentName", grade.StudentId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", grade.SubjectId);
            return View(grade);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null) return NotFound();
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentName", grade.StudentId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", grade.SubjectId);
            return View(grade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GradeId,StudentId,SubjectId,GradeValue")] Grade grade)
        {
            if (id != grade.GradeId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(grade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentName", grade.StudentId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", grade.SubjectId);
            return View(grade);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var grade = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .FirstOrDefaultAsync(m => m.GradeId == id);
            if (grade == null) return NotFound();
            return View(grade);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade != null)
            {
                _context.Grades.Remove(grade);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}