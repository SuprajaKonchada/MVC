using Microsoft.EntityFrameworkCore;
using QuizApplication.Data;
using QuizApplication.Models;

namespace QuizApplication.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public AccountRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Student GetStudentByEmail(string studentEmail)
        {
            return _dbContext.Students.FirstOrDefault(s => s.StudentEmail == studentEmail);
        }
        public void AddStudent(Student student)
        {
            _dbContext.Students.Add(student);
            _dbContext.SaveChanges();
        }
        public Teacher GetTeacherByEmail(string teacherEmail)
        {
            return _dbContext.Teachers.FirstOrDefault(t => t.TeacherEmail == teacherEmail) ;
        }
        public void AddTeacher(Teacher teacher)
        {
            _dbContext.Teachers.Add(teacher);
            _dbContext.SaveChanges();
        }
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
