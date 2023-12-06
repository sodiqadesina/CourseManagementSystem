using CourseManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CourseManagementSystem.Controllers
{
    public class HomeController : AbsractBaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(IHttpContextAccessor contextAccessor)
             : base(contextAccessor)
        {

        }

        public IActionResult Index()
        {
            SetHomeMessage();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}