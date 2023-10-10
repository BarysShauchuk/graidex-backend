using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using Graidex.Domain.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graidex.Domain.Models.Tests
{
    /// <summary>
    /// Represents a result of the test.
    /// </summary>
    public class TestResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for the test result.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether the test was already checked automatically.
        /// </summary>
        public bool IsAutoChecked { get; set; }

        /// <summary>
        /// Gets or sets the time of the start of the test.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the time of the end of the test.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether the test was submitted.
        /// </summary>
        public bool IsSubmitted { get; set; }

        /// <summary>
        /// Gets or sets id of the test the result belongs to.
        /// </summary>
        public required int TestId { get; set; }

        /// <summary>
        /// Gets or sets id of the student who took the test.
        /// </summary>
        public required int StudentId { get; set; }

        /// <summary>
        /// Gets or sets the collection of answers to the questions in the test.
        /// </summary>
        public List<IAnswer<Question>> Answers { get; set; } = new();

        /// <summary>
        /// Gets or sets the total amount of points earned.
        /// </summary>
        [NotMapped]
        public int TotalPoints => Answers.Sum(answer => answer.Points);

        /// <summary>
        /// Gets or sets the grade earned.
        /// </summary>
        [NotMapped]
        public int Grade => Answers.Sum(answer => answer.Question.MaxPoints);
    }
}