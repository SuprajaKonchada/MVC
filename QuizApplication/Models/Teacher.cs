using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApplication.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string TeacherName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string TeacherEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&+=!*]).*$", ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one number, and one special character.")]
        public string TeacherPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("TeacherPassword", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        [NotMapped] // Not stored in the database
        public string TeacherConfirmPassword { get; set; }
    }
}
