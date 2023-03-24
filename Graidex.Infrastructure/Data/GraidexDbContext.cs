using Graidex.Domain.Models;
using Graidex.Domain.Models.Answers;
using Graidex.Domain.Models.Questions;
using Graidex.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Graidex.Infrastructure.Data
{
    public class GraidexDbContext : DbContext
    {
        public GraidexDbContext()
        {
        }

        public GraidexDbContext(DbContextOptions<GraidexDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasIndex(student => student.Email).IsUnique();
            modelBuilder.Entity<Teacher>().HasIndex(teacher => teacher.Email).IsUnique();

            modelBuilder.Entity<SingleChoiceQuestion>().ToTable(nameof(SingleChoiceQuestion));
            modelBuilder.Entity<MultipleChoiceQuestion>().ToTable(nameof(MultipleChoiceQuestion));
            modelBuilder.Entity<OpenQuestion>().ToTable(nameof(OpenQuestion));

            modelBuilder.Entity<SingleChoiceAnswer>().ToTable(nameof(SingleChoiceAnswer));
            modelBuilder.Entity<MultipleChoiceAnswer>().ToTable(nameof(MultipleChoiceAnswer));
            modelBuilder.Entity<OpenAnswer>().ToTable(nameof(OpenAnswer));
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
    }
}
