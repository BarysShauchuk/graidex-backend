using Graidex.Domain.Models;
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

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
    }
}
