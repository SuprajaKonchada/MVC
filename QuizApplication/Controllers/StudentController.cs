using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizApplication.Filters;
using QuizApplication.Models;
using QuizApplication.Repository;

namespace QuizApplication.Controllers
{
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    [Authorization("Student")]
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        /// <summary>
        /// Displays the Exam Dashboard page for students.
        /// </summary>
        /// <returns></returns>
        public ActionResult ExamDashboard()
        {
            return View();
        }
        /// <summary>
        /// Handles the submission of a room number to start a quiz.
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExamDashboard(string room)
        {
            // Fetch a list of categories from the database
            List<Category> categoryList = _studentRepository.GetCategories();

            // Find a Category with the given room number (EncryptedString)
            var CategoryObj = _studentRepository.GetCategoryByRoomNumber(room);

            if (CategoryObj == null)
            {
                ViewBag.error = "No room found....";
                return View();
            }

            HttpContext.Session.SetInt32("CategoryId", CategoryObj.Id);
            int studentId = (int)HttpContext.Session.GetInt32("StudentId");
            // Check if the student has already taken the exam for this category
            var examResult = _studentRepository.GetReportFromStudentIdCategoryId(studentId, CategoryObj.Id);

            if (examResult != null)
            {
                ViewBag.error = "You have already taken this exam.";
                return View();
            }

            foreach (var item in categoryList)
            {
                if (item.EncrytpedString == room)
                {
                    // Retrieve a list of questions for the selected Category
                    List<Question> questionsList = _studentRepository.GetQuestionByCategoryId(item.Id);
                    Queue<Question> queue = new Queue<Question>();
                    foreach (Question a in questionsList)
                    {
                        queue.Enqueue(a);
                    }

                    // Serialize the queue and store it in TempData
                    string serializedQueue = JsonConvert.SerializeObject(queue);
                    TempData["examid"] = item.TeacherId;
                    TempData["questions"] = serializedQueue;
                    return RedirectToAction("StartQuiz");
                }
                else
                {
                    ViewBag.error = "No Room found...";
                }
            }
            return View();
        }

        /// <summary>
        /// Displays the quiz questions for students to answer.
        /// </summary>
        /// <returns></returns>
        public ActionResult StartQuiz()
        {
            Question? question = null;
            // Fetch the category ID from TempData or wherever it's stored
            int categoryId = HttpContext.Session.GetInt32("CategoryId") ?? 0;

            if (TempData["questions"] != null)
            {
                Queue<Question>? questionslist = null;

                if (TempData["questions"] is string serializedQueue)
                {
                    questionslist = JsonConvert.DeserializeObject<Queue<Question>>(serializedQueue);
                }

                if (questionslist != null && questionslist.Count > 0)
                {

                    question = questionslist.Dequeue();
                    ViewBag.qId = Convert.ToInt32(question.Id);
                    HttpContext.Session.SetInt32("questionId", question.Id);
                    int currentQuestionNumber = _studentRepository.TotalQuestionsForCategory(categoryId) - questionslist.Count; // Calculate the current question number

                    ViewBag.QuestionNumber = currentQuestionNumber;
                    // Pass the question duration to the view using ViewBag
                    ViewBag.QuestionDuration = question.Duration;
                    ViewBag.Totalquestions = _studentRepository.TotalQuestionsForCategory(categoryId);

                    string updatedSerializedQueue = JsonConvert.SerializeObject(questionslist);
                    TempData["questions"] = updatedSerializedQueue;
                    TempData.Keep();
                }
                else
                {
                    return RedirectToAction("EndExam");
                }
            }
            else
            {
                return RedirectToAction("ExamDashboard");
            }

            TempData.Keep();
            return View(question);
        }
        /// <summary>
        /// Handles the submission of answers to quiz questions.
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult StartQuiz(Question question)
        {
            // Initialize variables for scoring
            int scoreExam = 0;
            string? selectedOption = null;

            if (question.OptionA != null)
            {
                selectedOption = "A";
            }
            else if (question.OptionB != null)
            {
                selectedOption = "B";
            }
            else if (question.OptionC != null)
            {
                selectedOption = "C";
            }
            else if (question.OptionD != null)
            {
                selectedOption = "D";
            }
            else
            {
                selectedOption = "NA";
            }
            int? currentQuestionId = HttpContext.Session.GetInt32("questionId");
            int mark = 0;
            int negativeMark = 0;
            string? correctOption = null;

            if (!string.IsNullOrEmpty(selectedOption))
            {
                var currentQuestion = _studentRepository.CurrentQuestionByQuestionId((int)currentQuestionId);
                if (currentQuestion != null)
                {
                    mark = currentQuestion.Mark;
                    negativeMark = currentQuestion.NegativeMark;
                    correctOption = currentQuestion.CorrectOption;
                }
            }

            if (selectedOption.Equals(correctOption))
            {
                scoreExam += mark;
            }
            else if (selectedOption == "NA")
            {
                scoreExam += 0;
            }
            else
            {
                scoreExam -= negativeMark;
            }
            // Create a new instance of StudentQuestionMarks to store information for the current question
            var studentQuestionMarks = new StudentQuestionMarks
            {
                StudentId = (int)HttpContext.Session.GetInt32("StudentId"),
                QuestionId = currentQuestionId ?? 0,
                MarksObtained = scoreExam,
                SelectedOption = selectedOption
            };

            _studentRepository.AddStudentQuestionMarks(studentQuestionMarks);

            int currentTotalScore = HttpContext.Session.GetInt32("TotalScore") ?? 0;
            HttpContext.Session.SetInt32("TotalScore", currentTotalScore + scoreExam);
            int totalScore = HttpContext.Session.GetInt32("TotalScore") ?? 0;
            TempData["TotalScore"] = totalScore;
            TempData.Keep();
            return RedirectToAction("StartQuiz");
        }
        /// <summary>
        /// Displays the end of the exam, calculates the total score, and stores it in the database
        /// </summary>
        /// <returns></returns>

        public ActionResult EndExam()
        {
            int quizTotalScore = TempData["TotalScore"] as int? ?? 0;
            int studentID = (int)HttpContext.Session.GetInt32("StudentId");
            int CategoryID = (int)HttpContext.Session.GetInt32("CategoryId");
            var report = new Report()
            {
                score = quizTotalScore,
                StudentId = studentID,
                CategoryId = CategoryID,
            };
            _studentRepository.AddScoreToReport(report);
            HttpContext.Session.Clear();
            ViewBag.QuizTotalScore = quizTotalScore;
            return View();
        }
    }
}
