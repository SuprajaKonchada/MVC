using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> TeacherId { get; set; }
        public string EncrytpedString { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalMarks { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<Question> Question { get; set; }
    }
}
