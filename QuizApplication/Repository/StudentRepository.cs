using QuizApplication.Data;
using QuizApplication.Models;

namespace QuizApplication.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public StudentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Category> GetCategories() => _dbContext.Categories.ToList();
        public Category? GetCategoryByRoomNumber(string room)
        {
            return _dbContext.Categories
                .FirstOrDefault(u => u.EncrytpedString == room);
        }
        public Report? GetReportFromStudentIdCategoryId(int studentId, int categoryId)
        {
            return _dbContext.Reports
                .FirstOrDefault(e => e.StudentId == studentId && e.CategoryId == categoryId);
        }

        public List<Question>  GetQuestionByCategoryId(int Id)
        {
            return _dbContext.Questions
                        .Where(x => x.CategoryId == Id)
                        .ToList();
        }
        public int TotalQuestionsForCategory(int categoryId)
        {
            return _dbContext.Categories
                        .Where(c => c.Id == categoryId)
                        .Select(c => c.TotalQuestions)
                        .FirstOrDefault();
        }
        public Question? CurrentQuestionByQuestionId(int currentQuestionId)
        {
            return _dbContext.Questions
                    .FirstOrDefault(x => x.Id == currentQuestionId);
        }
        public void AddStudentQuestionMarks(StudentQuestionMarks studentQuestionMarks)
        {
            // Add the current question's information to the StudentQuestionMarks table
            _dbContext.StudentQuestionMarks.Add(studentQuestionMarks);
            _dbContext.SaveChanges();
        }
        public void AddScoreToReport(Report report)
        {
            _dbContext.Reports.Add(report);
            _dbContext.SaveChanges();
        }
    }
}
