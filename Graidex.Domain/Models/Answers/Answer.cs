using Graidex.Domain.Models.Questions;

namespace Graidex.Domain.Models.Answers
{   
    /// <summary>
    /// Represents an answer to a question in the test.
    /// </summary>
    public abstract class Answer
    {
        /// <summary>
        /// Gets or sets the unique identifier for the answer.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the test result the answer relates to.
        /// </summary>
        public required virtual TestResult TestResult { get; set; }
    }
}
