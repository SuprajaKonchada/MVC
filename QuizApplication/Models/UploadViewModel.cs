using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Models
{
    public class UploadViewModel
    {
        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }
        public IFormFile File { get; set; }
    }
}
