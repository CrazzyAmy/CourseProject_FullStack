using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CourseWebJSApp.Models;

namespace CourseWebJSApp.Controllers
{
    // /my
    [Route("[controller]")]
    public class MyController : Controller
    {
        private readonly ILogger<MyController> _logger;

        public MyController(ILogger<MyController> logger)
        {
            _logger = logger;
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            return View();
        }
        // /my/profileview
        [HttpGet("profileview")]
        public IActionResult Profile()
        {
            return View();
        }
        // /my/profileview/...
        [HttpGet("profileview/{name}")]
        public IActionResult Profile2()
        {
            return View();
        }
    }
}
