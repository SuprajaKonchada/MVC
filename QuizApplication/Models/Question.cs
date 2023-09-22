using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Question text is required.")]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "Option A is required.")]
        public string OptionA { get; set; }

        [Required(ErrorMessage = "Option B is required.")]
        public string OptionB { get; set; }

        [Required(ErrorMessage = "Option C is required.")]
        public string OptionC { get; set; }

        [Required(ErrorMessage = "Option D is required.")]
        public string OptionD { get; set; }

        [Required(ErrorMessage = "Correct option is required.")]
        public string CorrectOption { get; set; }

        [Required(ErrorMessage = "Mark is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Mark must be a positive number.")]
        public int Mark { get; set; }

        [Required(ErrorMessage = "Negative mark is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Negative mark must be a positive number.")]
        public int NegativeMark { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage ="Time required for this question")]
        public int Duration { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }
    }
}
