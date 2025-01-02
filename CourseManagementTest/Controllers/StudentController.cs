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
            }

            if (student == null && id.HasValue)
            {
                return NotFound();
            }

            return View(student);
        }
    }
}
