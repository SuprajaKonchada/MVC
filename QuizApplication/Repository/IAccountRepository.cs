using QuizApplication.Models;

namespace QuizApplication.Repository
{
    public interface IAccountRepository
    {
        public Student GetStudentByEmail(string studentEmail);
        public void AddStudent(Student student);
        public Teacher GetTeacherByEmail(string teacherEmail);
        public void AddTeacher(Teacher teacher);
        public void SaveChanges();
    }
}
