using QuizApplication.Models;
using Microsoft.EntityFrameworkCore;


namespace QuizApplication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<StudentQuestionMarks> StudentQuestionMarks { get; set; }
    }

}
