using Microsoft.AspNetCore.Mvc;
using CourseManagementTest.Data;
using Microsoft.EntityFrameworkCore;
using CourseManagementTest.Models;

namespace CourseManagementTest.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.ToListAsync();

            return View(courses);
        }

        public async Task<IActionResult> CreateOrEdit(int? id)
        {
            Course course;
            if (id == null)
            {
                course = new Course();
            }
            else
            {
                course = await _context.Courses.FindAsync(id);
                if (course == null)
                {
                    return NotFound();
                }
            }
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(int? id, [Bind("CourseId,Title,Description")] Course course)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    _context.Courses.Add(course);
                }
                else
                {
                    var existingCourse = await _context.Courses.FindAsync(id);
                    if (existingCourse != null)
                    {
                        existingCourse.Title = course.Title;
                        existingCourse.Description = course.Description;
                        _context.Courses.Update(existingCourse);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
