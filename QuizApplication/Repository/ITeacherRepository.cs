using QuizApplication.Models;

namespace QuizApplication.Repository
{
    public interface ITeacherRepository
    {
        public Category GetCategoryByRoomNumber(string room);
        public List<Report> GetReport(int categoryId);
        public List<StudentQuestionMarks> GetStudentQuestionMarks(int studentId);

    }
}
