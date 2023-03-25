using System.ComponentModel.DataAnnotations;

namespace Graidex.Domain.Models.Questions
{   
    /// <summary>
    /// Represents a question in the test.
    /// </summary>
    public abstract class Question
    {   
        /// <summary>
        /// Gets or sets the unique identifier for the question.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the test the question belongs to.
        /// </summary>
        public required virtual Test Test { get; set; }

        /// <summary>
        /// Gets or sets the text of the question.
        /// </summary>
        public required string Text { get; set; }
    }
}