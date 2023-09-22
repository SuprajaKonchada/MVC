using QuizApplication.Data;
using QuizApplication.Models;

namespace QuizApplication.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Category> GetCategoriesByTeacherId(int teacherId)
        {
            return _dbContext.Categories
                .Where(c => c.TeacherId == teacherId)
                .OrderByDescending(c => c.Id)
                .ToList();
        }
        public bool CategoryExistsForTeacher(int teacherId, string categoryName)
        {
            return _dbContext.Categories
                .Any(c => c.TeacherId == teacherId && c.Name == categoryName);
        }

        public void AddCategory(Category category)
        {
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
        }

        public void DeleteCategory(int categoryId)
        {
            var categoryToDelete = _dbContext.Categories.Find(categoryId);

            if (categoryToDelete != null)
            {
                _dbContext.Categories.Remove(categoryToDelete);
                _dbContext.SaveChanges();
            }
        }
    }

}
