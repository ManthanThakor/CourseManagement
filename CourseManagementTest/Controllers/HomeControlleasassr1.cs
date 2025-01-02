using CourseManagementTest.Data;
using CourseManagementTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementTest.Controllers
{
    public class StudentControlle : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentControlle(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Student/Index
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.ToListAsync();
            return View(students);
        }

        // GET: Student/CreateOrEdit
        public async Task<IActionResult> CreateOrEdit(int? id)
        {
            Student student = id == null ? new Student() : await _context.Students.FindAsync(id);

            if (student == null && id.HasValue)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/CreateOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(int id, [Bind("StudentId,Name,Email,Address,DOB")] Student student)
        {
            if (ModelState.IsValid)
            {
                if (id == 0) // Create
                {
                    _context.Add(student);
                }
                else // Edit
                {
                    try
                    {
                        _context.Update(student);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!StudentExists(student.StudentId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
