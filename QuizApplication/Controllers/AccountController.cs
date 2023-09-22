using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizApplication.HelperClasses;
using QuizApplication.Models;
using QuizApplication.Repository;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;
using QuizApplication.Filters;

namespace QuizApplication
{
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]

    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        
        /// <summary>
        /// Displays the student registration form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult StudentRegister()
        {
            return View();
        }
        /// <summary>
        /// Handles the student registration form submission
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult StudentRegister(Student student)
        {
            if (ModelState.IsValid)
            {
                // Check if the user with the same email already exists
                var existingStudent = _accountRepository.GetStudentByEmail(student.StudentEmail.ToLower());

                if (existingStudent != null)
                {
                    // User with the same email already exists, add an error message
                    ModelState.AddModelError("StudentEmail", "A user with this email already exists.");
                    return View(student);
                }

                // If user doesn't exist, proceed to create a new user
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(student.StudentPassword);

                var studentObj = new Student()
                {
                    StudentName = student.StudentName,
                    StudentEmail = student.StudentEmail.ToLower(),
                    StudentPassword = hashedPassword
                };

                _accountRepository.AddStudent(studentObj);
                return RedirectToAction("StudentLogin");
            }
            return View(student);
        }

        /// <summary>
        /// Displays the student login form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult StudentLogin()
        {
            return View();
        }
        /// <summary>
        /// Handles the student login form submission
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> StudentLogin(Student student)
        {
            if (student!=null)
            {
                Student? studentObj = _accountRepository.GetStudentByEmail(student.StudentEmail.ToLower());

                if (studentObj != null)
                {
                    bool passwordMatches = BCrypt.Net.BCrypt.Verify(student.StudentPassword, studentObj.StudentPassword);

                    if (passwordMatches)
                    {
                        HttpContext.Session.SetInt32("StudentId", studentObj.Id);
                        HttpContext.Session.SetString("user_type", "Student");
                        return RedirectToAction("ExamDashboard","Student"); 
                    }
                }
            }
            ViewBag.ErrorMessage = "[Invalid Password or Email]";
            return View(student);
        }
        /// <summary>
        /// Handles the teacher registration form submission
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult TeacherRegister()
        {
            return View();
        }
        /// <summary>
        /// Handles the teacher registration form submission
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult TeacherRegister(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                // Check if a user with the same email already exists
                var existingTeacher = _accountRepository.GetTeacherByEmail(teacher.TeacherEmail.ToLower());

                if (existingTeacher != null)
                {
                    ModelState.AddModelError("TeacherEmail", "A user with this email already exists.");
                    return View(teacher);
                }

                // Hash the password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(teacher.TeacherPassword);

                var teacherObj = new Teacher()
                {
                    TeacherName = teacher.TeacherName,
                    TeacherEmail = teacher.TeacherEmail.ToLower(),
                    TeacherPassword = hashedPassword
                };

                _accountRepository.AddTeacher(teacherObj);
                return RedirectToAction("TeacherLogin");
            }
            return View(teacher);
        }

        /// <summary>
        /// Displays the teacher login form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult TeacherLogin()
        {
            return View();
        }
        /// <summary>
        /// Handles the teacher login form submission.
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> TeacherLogin(Teacher teacher)
        {
            if (teacher!=null)
            {
                Teacher? teacherObj = _accountRepository.GetTeacherByEmail(teacher.TeacherEmail.ToLower());

                if (teacherObj != null)
                {
                    bool passwordMatches = BCrypt.Net.BCrypt.Verify(teacher.TeacherPassword, teacherObj.TeacherPassword);

                    if (passwordMatches)
                    {
                        HttpContext.Session.SetInt32("TeacherId", teacherObj.Id);
                        HttpContext.Session.SetString("user_type", "Teacher");
                        return RedirectToAction("Report", "Teacher"); 
                    }
                }
            }
            ViewBag.ErrorMessage = "[Invalid Password or Email]";
            return View();
        }
        /// <summary>
        /// Logs the user out by clearing the session
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }
        public ActionResult StudentForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult StudentForgotPassword(string StudentEmail)
        {
            Student studentObj = _accountRepository.GetStudentByEmail(StudentEmail.ToLower());
            OTPManipulation oTPManipulationObj = new OTPManipulation();
            if (studentObj == null)
            {
                ViewBag.ErrorMessage = "Email doesn't exist";
                return View();
            }
            string otp = oTPManipulationObj.GenerateOtp();
            HttpContext.Session.SetString("StudentEmail", StudentEmail);
            HttpContext.Session.SetString("ResetOtp", otp);
            HttpContext.Session.SetString("OtpExpiry", DateTime.Now.AddMinutes(5).ToString());

            oTPManipulationObj.SendOTPEmail(studentObj.StudentEmail, otp);
            return View("StudentOTPVerification");
        }
        public ActionResult StudentOTPVerification()
        {
            return View();
        }
        [HttpPost]
        public ActionResult StudentOTPVerification(string enteredOtp)
        {
            string savedOtp = HttpContext.Session.GetString("ResetOtp");
            DateTime otpExpiry = Convert.ToDateTime(HttpContext.Session.GetString("OtpExpiry"));
            if(enteredOtp==savedOtp && DateTime.Now <= otpExpiry)
            {
                return RedirectToAction("StudentResetPassword");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid OTP or OTP has expired.";
                return View();
            }
        }
        public ActionResult StudentResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult StudentResetPassword(string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                // Passwords do not match, return an error view
                ViewBag.ErrorMessage = "Passwords do not match.";
                return View(); // Assuming you have a ResetPassword view for displaying the error.
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            string StudentEmail = HttpContext.Session.GetString("StudentEmail");
            var student = _accountRepository.GetStudentByEmail(StudentEmail);

            if (student != null)
            {
                // Update the student's password
                student.StudentPassword = hashedPassword;

                // Save changes to the database
                _accountRepository.SaveChanges();
                return View("StudentLogin");
            }
            else
            {
                ViewBag.ErrorMessage = "Email doesn't exist";
                return View("StudentForgotPassword");
            }
        }
        public ActionResult TeacherForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TeacherForgotPassword(string TeacherEmail)
        {
            Teacher teacherObj = _accountRepository.GetTeacherByEmail(TeacherEmail.ToLower());
            OTPManipulation oTPManipulationObj = new OTPManipulation();

            if (teacherObj == null)
            {
                ViewBag.ErrorMessage = "Email doesn't exist";
                return View();
            }

            string otp = oTPManipulationObj.GenerateOtp();
            HttpContext.Session.SetString("TeacherEmail", TeacherEmail);
            HttpContext.Session.SetString("ResetOtp", otp);
            HttpContext.Session.SetString("OtpExpiry", DateTime.Now.AddMinutes(5).ToString());

            oTPManipulationObj.SendOTPEmail(teacherObj.TeacherEmail, otp);
            return View("TeacherOTPVerification");
        }

        public ActionResult TeacherOTPVerification()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TeacherOTPVerification(string enteredOtp)
        {
            string savedOtp = HttpContext.Session.GetString("ResetOtp");
            DateTime otpExpiry = Convert.ToDateTime(HttpContext.Session.GetString("OtpExpiry"));

            if (enteredOtp == savedOtp && DateTime.Now <= otpExpiry)
            {
                return RedirectToAction("TeacherResetPassword");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid OTP or OTP has expired.";
                return View();
            }
        }

        public ActionResult TeacherResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TeacherResetPassword(string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                // Passwords do not match, return an error view
                ViewBag.ErrorMessage = "Passwords do not match.";
                return View(); 
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            string TeacherEmail = HttpContext.Session.GetString("TeacherEmail");
            var teacher = _accountRepository.GetTeacherByEmail(TeacherEmail);

            if (teacher != null)
            {
                // Update the teacher's password
                teacher.TeacherPassword = hashedPassword;

                // Save changes to the database
                _accountRepository.SaveChanges();
                return View("TeacherLogin");
            }
            else
            {
                ViewBag.ErrorMessage = "Email doesn't exist";
                return View("TeacherForgotPassword");
            }
        }

    }
}


