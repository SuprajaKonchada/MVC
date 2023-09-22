using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApplication.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string StudentEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]

        public string StudentPassword { get; set; }

        [NotMapped] // Not stored in the database
        [Required(ErrorMessage = "Re-enter the password")]
        [Compare("StudentPassword", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string StudentConfirmPassword { get; set; }
    }
}
