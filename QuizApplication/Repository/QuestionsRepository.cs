using Microsoft.EntityFrameworkCore;
using QuizApplication.Data;
using QuizApplication.Models;

namespace QuizApplication.Repository
{
    public class QuestionsRepository : IQuestionsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public QuestionsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Question> GetQuestionsByCategoryId(int categoryId)
        {
            return _dbContext.Questions
                .Where(x => x.CategoryId == categoryId).ToList();
        }
        public List<Category> GetCategoryByTeacherId(int teacherId)
        {
            return _dbContext.Categories
                    .Where(x => x.TeacherId == teacherId).ToList(); ;
        }
        public void AddQuestions(Question question)
        {
            _dbContext.Questions.Add(question);
            _dbContext.SaveChanges();
        }
        public void UpdateTotalQuestionsForCategory(int categoryId)
        {
            var category = _dbContext.Categories
                .FirstOrDefault(c => c.Id == categoryId);
            if (category != null)
            {
                category.TotalQuestions = _dbContext.Questions
                    .Count(q => q.CategoryId == categoryId);
                _dbContext.SaveChanges();
            }
        }
        public void UpdateTotalMarksFromCategory(int categoryId)
        {
            var category = _dbContext.Categories
                .FirstOrDefault(c => c.Id == categoryId);

            if (category != null)
            {
                category.TotalMarks = _dbContext.Questions
                    .Where(q => q.CategoryId == categoryId)
                    .Sum(q => q.Mark); // Assuming there's a 'Marks' property in your 'Question' model

                _dbContext.SaveChanges();
            }
        }

        public Question GetQuestionById(int questionId)
        {
            return _dbContext.Questions
                .FirstOrDefault(m => m.Id == questionId);
        }
        public Question FindQuestion(int questionId)
        {
            return _dbContext.Questions
                .Find(questionId);
        }
        public void UpdateQuestion(Question question)
        {
            _dbContext.Update(question);
            _dbContext.SaveChanges();
        }
        public Category GetCategoryByCategoryId(int categoryId)
        {
            return _dbContext.Categories
                .FirstOrDefault(c => c.Id == categoryId);
        }
        public void DeleteQuestion(Question question)
        {
            _dbContext.Questions.Remove(question);
            _dbContext.SaveChanges();
        }
        public bool QuestionsExists(int Id)
        {
            return (_dbContext.Questions?.Any(e => e.Id == Id)).GetValueOrDefault();
        }
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
