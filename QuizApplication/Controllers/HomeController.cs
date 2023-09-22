using Microsoft.AspNetCore.Mvc;
using QuizApplication.Models;
using System.Diagnostics;

namespace QuizApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public ActionResult AuthorizationError()
        {
            ViewBag.ErrorMessage = "Unauthorized Error";
            ViewBag.Information = "You are not authorized to access this page.";
            return View("Error");
        }
        public IActionResult ExceptionError(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;
            return View("Error"); 
        }
    }
}