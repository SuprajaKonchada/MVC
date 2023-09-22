using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using QuizApplication.Filters;
using QuizApplication.Models;
using QuizApplication.Repository;

namespace QuizApplication.Controllers
{
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    [Authorization("Teacher")]
    public class QuestionsController : Controller
    {
        private readonly IQuestionsRepository _questionRepository;

        public QuestionsController(IQuestionsRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }
        /// <summary>
        /// Displays all questions for a specific category
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IActionResult ViewAllQuestions(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Dashboard");
            }
            return View(_questionRepository.GetQuestionsByCategoryId((int)Id));
        }
        /// <summary>
        /// Displays the form to add a new question
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddQuestions()
        {
            int teacherID = Convert.ToInt32(HttpContext.Session.GetInt32("TeacherId"));
            List<Category> categoryList = _questionRepository.GetCategoryByTeacherId(teacherID);
            ViewBag.list = new SelectList(categoryList, "Id", "Name");
            return View();
        }
        /// <summary>
        /// Handles the form submission to add a new question
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddQuestions(Question question)
        {
            int teacherID = Convert.ToInt32(HttpContext.Session.GetInt32("TeacherId"));
            List<Category> categoryList = _questionRepository.GetCategoryByTeacherId(teacherID);
            ViewBag.list = new SelectList(categoryList, "Id", "Name");

            if (question != null && !string.IsNullOrEmpty(question.CorrectOption))
            {
                Question questionObj = new Question();
                questionObj.QuestionText = question.QuestionText;
                questionObj.OptionA = question.OptionA;
                questionObj.OptionB = question.OptionB;
                questionObj.OptionC = question.OptionC;
                questionObj.OptionD = question.OptionD;
                questionObj.CorrectOption = question.CorrectOption;
                questionObj.Mark = question.Mark;
                questionObj.NegativeMark = question.NegativeMark;
                questionObj.Duration = question.Duration;
                questionObj.CategoryId = question.CategoryId;

                _questionRepository.AddQuestions(questionObj);

                // Update the totalQuestions in Category table
                _questionRepository.UpdateTotalQuestionsForCategory(question.CategoryId);
                _questionRepository.UpdateTotalMarksFromCategory(question.CategoryId);

                ViewBag.successmsg = "Added Successfully";
                return RedirectToAction("AddQuestions");
            }

            ModelState.AddModelError("CorrectOption", "Please select the correct option.");
            return View(question);
        }
        /// <summary>
        /// Handles the upload of questions from an Excel file
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadQuestions(UploadViewModel viewModel)
        {

            if (viewModel.File == null || viewModel.File.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a file.");
                return View(viewModel);
            }

            using var package = new ExcelPackage(viewModel.File.OpenReadStream());
            var worksheet = package.Workbook.Worksheets[0];

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                string question = worksheet.Cells[row, 2].Value?.ToString();
                string optionA = worksheet.Cells[row, 3].Value?.ToString();
                string optionB = worksheet.Cells[row, 4].Value?.ToString();
                string optionC = worksheet.Cells[row, 5].Value?.ToString();
                string optionD = worksheet.Cells[row, 6].Value?.ToString();
                string correctanswer = worksheet.Cells[row, 7].Value?.ToString();
                int mark = Convert.ToInt32(worksheet.Cells[row, 8].Value);
                int negativemark = Convert.ToInt32(worksheet.Cells[row, 9].Value);
                int duration = Convert.ToInt32(worksheet.Cells[row, 10].Value);

                // Check if mark or duration is zero and set them to 1 and 30 respectively
                if (mark == 0)
                {
                    mark = 1;
                }

                if (duration == 0)
                {
                    duration = 30;
                }

                var questions = new Question()
                {
                    QuestionText = question,
                    OptionA = optionA,
                    OptionB = optionB,
                    OptionC = optionC,
                    OptionD = optionD,
                    CorrectOption = correctanswer,
                    Mark = mark,
                    NegativeMark = negativemark,
                    Duration = duration,
                    CategoryId = viewModel.CategoryId,
                };
                _questionRepository.AddQuestions(questions);
            }

            // Update the TotalQuestions field in the Category table
            _questionRepository.UpdateTotalQuestionsForCategory(viewModel.CategoryId);
            _questionRepository.UpdateTotalMarksFromCategory(viewModel.CategoryId);

            return RedirectToAction("AddQuestions");

        }
        /// <summary>
        /// Displays details of a specific question
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IActionResult Details(int? Id)
        {
            var questions = _questionRepository.GetQuestionById((int)Id);
            return View(questions);
        }
        /// <summary>
        /// Displays the form to edit a question
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IActionResult Edit(int? Id)
        {
            var questions = _questionRepository.FindQuestion((int)Id);
            return View(questions);
        }
        /// <summary>
        /// Handles the form submission to edit a question
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="questions"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(int Id, [Bind("Id,QuestionText,OptionA,OptionB,OptionC,OptionD,CorrectOption,Mark,NegativeMark,Duration")] Question questions)
        {
            var existingQuestion = _questionRepository.FindQuestion((int)Id);
            // Calculate the change in marks
            int changeInMarks = questions.Mark - existingQuestion.Mark;
            existingQuestion.QuestionText = questions.QuestionText;
            existingQuestion.OptionA = questions.OptionA;
            existingQuestion.OptionB = questions.OptionB;
            existingQuestion.OptionC = questions.OptionC;
            existingQuestion.OptionD = questions.OptionD;
            existingQuestion.CorrectOption = questions.CorrectOption;
            existingQuestion.Mark = questions.Mark;
            existingQuestion.NegativeMark = questions.NegativeMark;
            existingQuestion.Duration = questions.Duration;

            _questionRepository.UpdateQuestion(existingQuestion);

            // Update the totalMarks in the category table
            var category = _questionRepository.GetCategoryByCategoryId(existingQuestion.CategoryId);

            if (category != null)
            {
                category.TotalMarks += changeInMarks;
            }

            _questionRepository.SaveChanges();
            return RedirectToAction("ViewAllQuestions", new { id = existingQuestion.CategoryId });

        }
        /// <summary>
        /// Displays the confirmation page to delete a question
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int? Id)
        {
            var questions = _questionRepository.GetQuestionById((int)Id);
            return View(questions);
        }
        /// <summary>
        /// Handles the form submission to delete a question
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int Id)
        {
            var question = _questionRepository.FindQuestion(Id);

            if (question != null)
            {
                int categoryId = question.CategoryId;

                // Update the totalQuestions in Category table first
                var category = _questionRepository.GetCategoryByCategoryId(categoryId);

                if (category != null)
                {
                    category.TotalQuestions--; // Decrease the count
                    category.TotalMarks -= question.Mark; // Decrease the totalMarks by the marks of the deleted question
                }

                _questionRepository.DeleteQuestion(question);
            }

            int TeacherId = Convert.ToInt32(HttpContext.Session.GetInt32("TeacherId"));
            return RedirectToAction("ViewAllQuestions", new { id = question.CategoryId });
        }

    }
}
