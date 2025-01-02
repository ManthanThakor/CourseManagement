using Microsoft.AspNetCore.Mvc;

namespace CourseManagementTest.Controllers
{
    public class Enrollment : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
