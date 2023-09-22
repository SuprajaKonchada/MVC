using Microsoft.EntityFrameworkCore;
using QuizApplication.Data;
using QuizApplication.Models;

namespace QuizApplication.Repository
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public TeacherRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Category GetCategoryByRoomNumber(string room)
        {
            return _dbContext.Categories
                    .FirstOrDefault(u => u.EncrytpedString == room);
        }
        public List<Report> GetReport(int categoryId)
        {
           return  _dbContext.Reports
                    .Include(r => r.Student)
                    .Where(u => u.CategoryId == categoryId)
                    .OrderByDescending(r => r.score)
                    .ToList();
        }
        public List<StudentQuestionMarks> GetStudentQuestionMarks(int studentId)
        {
            return _dbContext.StudentQuestionMarks
                .Include(sq => sq.Question)
                .Where(sq => sq.StudentId == studentId)
                .ToList();         
        }
    }
}
