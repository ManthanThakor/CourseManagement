using CourseManagementTest.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseManagementTest.Models;

namespace CourseManagementTest.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.ToListAsync();
            return View(students);
        }

        public async Task<IActionResult> CreateOrEdit(int? id)
        {
            Student student;
            if (id == null)
            {
                student = new Student();
            }
            else
            {
                student = await _context.Students.FindAsync(id);
                if (student == null)
                {
                    return NotFound();
                }
            }

            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(int? id, [Bind("StudentId,Name,Email,Address,DOB")] Student student)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    _context.Students.Add(student);
                }
                else
                {
                    var existingStudent = await _context.Students.FindAsync(id);

                    if (existingStudent != null)
                    {
                        existingStudent.Name = student.Name;
                        existingStudent.Email = student.Email;
                        existingStudent.Address = student.Address;
                        existingStudent.DOB = student.DOB;

                        _context.Students.Update(existingStudent);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }
        public async Task<IActionResult> Details(int id)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

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
    }
}
