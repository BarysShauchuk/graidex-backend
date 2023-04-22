using Graidex.Domain.Models.Questions;
using Graidex.Domain.Models.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graidex.Domain.Models
{   
    /// <summary>
    /// Represents a test in the application.
    /// </summary>
    public class Test
    {   
        /// <summary>
        /// Gets or sets the unique identifier for the test.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the test.
        /// </summary>
        [MaxLength(50)]
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the time of the last update of the test.
        /// </summary>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the test is hidden or not.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets the time of the start of the test.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the time of the end of the test.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the time limit of the test.
        /// </summary>
        public TimeSpan TimeLimit { get; set; }

        /// <summary>
        /// Gets or sets id of the subject the test is created for.
        /// </summary>
        public required int SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the collection of students that are allowed to take the test.
        /// </summary>
        public virtual ICollection<Student> AllowedStudents { get; set; } = new List<Student>();

        /// <summary>
        /// Gets or sets the collection of questions for the test.
        /// </summary>
        public List<Question> Questions { get; set; } = new List<Question>();

        /// <summary>
        /// Gets or sets the maximum amount of points that can be earned for the test.
        /// </summary>
        [NotMapped]
        public int MaxPoints => Questions.Sum(question => question.MaxPoints);

        /// <summary>
        /// Gets or sets the minimum grade to pass this test.
        /// </summary>
        public int GradeToPass { get; set; }
    }
}
