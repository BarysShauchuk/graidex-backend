using Graidex.Domain.Models;
using Graidex.Domain.Models.Answers;
using Graidex.Domain.Models.Questions;
using Graidex.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Graidex.Infrastructure.Infrastructure;

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

            modelBuilder.Entity<Test>()
                .Property(x => x.Questions)
                .HasConversion(JsonExtensions.CreateJsonConverter<List<Question>>());

            modelBuilder.Entity<TestResult>()
                .Property(x => x.Answers)
                .HasConversion(JsonExtensions.CreateJsonConverter<List<Answer>>());
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
    }
}
