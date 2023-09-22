using QuizApplication.Models;
using System.Collections.Generic;

namespace QuizApplication.Repository
{
    public interface ICategoryRepository
    {
        List<Category> GetCategoriesByTeacherId(int teacherId);
        public void DeleteCategory(int categoryId);
        public bool CategoryExistsForTeacher(int teacherId, string categoryName);
        public void AddCategory(Category category);

    }
}
