using Microsoft.AspNetCore.Mvc;
using QuizApplication.Filters;
using QuizApplication.Models;
using QuizApplication.Repository;

namespace QuizApplication.Controllers
{
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    public class TeacherController : Controller
    {
        private readonly ITeacherRepository _teacherRepository;

        public TeacherController(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }
        /// <summary>
        /// Displays the exam result report for a specific room
        /// </summary>
        /// <returns></returns>
        public IActionResult Report()
        {
            return View();
        }
        /// <summary>
        ///  Handles the form submission to view the exam result report for a specific room
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Report(string? room)
        {
            if (room == null)
            {
                ViewBag.error = "No room found...";
                return View();
            }
            else
            {
                // Find the category based on the provided room string
                Category? category = _teacherRepository.GetCategoryByRoomNumber(room);
                if (category == null)
                {
                    ViewBag.error = "No room found...";
                    return View();
                }
                int categoryId = category.Id;
                // Retrieve and order the exam reports for the specified category
                IEnumerable<Report> report = _teacherRepository.GetReport(categoryId);

                ViewBag.CategoryName = category.Name;
                return View(@"~/Views/Teacher/ExamResult.cshtml", report);
            }
        }
        public ActionResult GetStudentMarks(int StudentId)
        {
            var studentQuestionMarks = _teacherRepository.GetStudentQuestionMarks(StudentId);

            return View("StudentQuestionMarks",studentQuestionMarks);
        }
    }
}

