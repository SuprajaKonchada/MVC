using QuizApplication.Models;

namespace QuizApplication.Repository
{
    public interface IStudentRepository
    {
        public List<Category> GetCategories();
        public Category GetCategoryByRoomNumber(string room);
        public Report GetReportFromStudentIdCategoryId(int studentId, int categoryId);
        public List<Question> GetQuestionByCategoryId(int Id);
        public int TotalQuestionsForCategory(int categoryId);
        public Question CurrentQuestionByQuestionId(int currentQuestionId);
        public void AddStudentQuestionMarks(StudentQuestionMarks studentQuestionMarks);
        public void AddScoreToReport(Report report);
    }
}
