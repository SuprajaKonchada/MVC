using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int score { get; set; }
        public int CategoryId { get; set; }
        public virtual Student Student { get; set; }
        public virtual Category Category { get; set; }
    }
}
