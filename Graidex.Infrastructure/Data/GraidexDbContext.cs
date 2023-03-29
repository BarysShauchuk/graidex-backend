using Graidex.Domain.Models;
using Graidex.Domain.Models.Answers;
using Graidex.Domain.Models.Questions;
using Graidex.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Graidex.Infrastructure.Infrastructure;

namespace Graidex.Infrastructure.Data
{   
    /// <summary>
    /// DbContext class used to perform database operations in the Graidex application.
    /// </summary>
    public class GraidexDbContext : DbContext
    {   
        /// <summary>
        /// Initializes a new instance of the <see cref="GraidexDbContext"/> class.
        /// Ensures that the database for the context exists.
        /// </summary>
        public GraidexDbContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraidexDbContext"/> class with given options.
        /// </summary>
        /// <param name="options">The options for configuring the context.</param>
        public GraidexDbContext(DbContextOptions<GraidexDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Overrides the model configuration performed by conventions.
        /// Remaps the entities to the database.
        /// </summary>
        /// <param name="modelBuilder">The ModelBuilder used to configure entities, relationships and connect them to the database.</param>
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

        /// <summary>
        /// Gets or sets the DbSet of <see cref="Student"/> objects.
        /// </summary>
        public DbSet<Student> Students { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of <see cref="Teacher"/> objects.
        /// </summary>
        public DbSet<Teacher> Teachers { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of <see cref="Subject"/> objects.
        /// </summary>
        public DbSet<Subject> Subjects { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of <see cref="Test"/> objects.
        /// </summary>
        public DbSet<Test> Tests { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of <see cref="TestResult"/> objects.
        /// </summary>
        public DbSet<TestResult> TestResults { get; set; }
    }
}
