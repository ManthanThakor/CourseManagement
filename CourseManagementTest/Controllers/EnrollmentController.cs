using CourseManagementTest.Data;
using CourseManagementTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementTest.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var enrollments = await _context.Enrollments
                                            .Include(e => e.Student)
                                            .Include(e => e.Course)
                                            .ToListAsync();
            return View(enrollments);
        }

        public async Task<IActionResult> CreateOrEdit(int? id)
        {
            Enrollment enrollment;
            if (id == null)
            {
                enrollment = new Enrollment();
            }
            else
            {
                enrollment = await _context.Enrollments
                                           .Include(e => e.Student)
                                           .Include(e => e.Course)
                                           .FirstOrDefaultAsync(e => e.EnrollmentId == id);

                if (enrollment == null)
                {
                    return NotFound();
                }
            }

            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "Name", enrollment?.StudentId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "Title", enrollment?.CourseId);

            return View(enrollment);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(int? id, [Bind("EnrollmentId,StudentId,CourseId,EnrollmentDate")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    _context.Add(enrollment);
                }
                else
                {
                    _context.Update(enrollment);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "Name", enrollment.StudentId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "Title", enrollment.CourseId);

            return View(enrollment);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
