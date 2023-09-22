using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QuizApplication.Filters;
using QuizApplication.Models;
using QuizApplication.Repository;
using System.Text;

namespace QuizApplication.Controllers
{
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    [Authorization("Teacher")] // Apply a custom authorization filter for teacher role
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        /// <summary>
        /// Displays the form to add a new category
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddCategory()
        {
            // Retrieve and display the list of categories for the teacher
            int teacherId = HttpContext.Session.GetInt32("TeacherId") ?? 0;
            List<Category> categoryList = _categoryRepository.GetCategoriesByTeacherId(teacherId);
            ViewData["list"] = categoryList;
            return View();
        }
        /// <summary>
        /// Handles the form submission to add a new category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            int? teacherID = HttpContext.Session.GetInt32("TeacherId");
            List<Category> categoryList = _categoryRepository.GetCategoriesByTeacherId((int)teacherID);

            ViewData["list"] = categoryList;
            Category categoryObj = new Category();
            // Check if the category with the same name doesn't already exist for the teacher
            if (!_categoryRepository.CategoryExistsForTeacher((int)teacherID, category.Name))
            {
                categoryObj.Name = category.Name;
                categoryObj.TeacherId = teacherID;

                // Generate an encrypted string for the category
                var currentDateTime = DateTime.Now;
                var currentTime = new TimeSpan(currentDateTime.Hour, currentDateTime.Minute, currentDateTime.Second);
                string combinedString = category.Name + currentTime.ToString();
                byte[] encodedBytes = Encoding.UTF8.GetBytes(combinedString);
                categoryObj.EncrytpedString = Convert.ToBase64String(encodedBytes);

                _categoryRepository.AddCategory(categoryObj); // Call the repository method to add the category

                categoryList.Insert(0, categoryObj);
                ViewData["list"] = categoryList;

                return View();
            }
            else
            {
                ViewBag.ErrorMsg = "Category already exists. Try with a different name.";
                return View();
            }
        }
        /// <summary>
        /// Deletes a category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteCategory(int id)
        {
            _categoryRepository.DeleteCategory(id);
            return RedirectToAction("AddCategory");
        }
    }
}

