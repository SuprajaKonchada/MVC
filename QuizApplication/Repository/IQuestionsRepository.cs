using QuizApplication.Models;

namespace QuizApplication.Repository
{
    public interface IQuestionsRepository
    {
        public List<Question> GetQuestionsByCategoryId(int categoryId);
        public List<Category> GetCategoryByTeacherId(int teacherId);
        public void AddQuestions(Question question);
        public void UpdateTotalQuestionsForCategory(int categoryId);
        public void UpdateTotalMarksFromCategory(int categoryId);
        public Question GetQuestionById(int questionId);
        public Question FindQuestion(int questionId);
        public void UpdateQuestion(Question question);
        public Category GetCategoryByCategoryId(int categoryId);
        public void DeleteQuestion(Question question);
        public void SaveChanges();

    }
}
