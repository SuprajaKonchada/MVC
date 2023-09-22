using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Models
{
    public class StudentQuestionMarks
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int QuestionId { get; set; }
        public string SelectedOption { get; set; }
        public int MarksObtained { get; set; }
        public virtual Student Student { get; set; }
        public virtual Question Question { get; set; }
    }
}
