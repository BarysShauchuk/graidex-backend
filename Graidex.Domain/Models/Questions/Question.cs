using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Graidex.Domain.Models.Questions
{
    /// <summary>
    /// Represents a question in the test.
    /// </summary>
    [JsonDerivedType(typeof(SingleChoiceQuestion), typeDiscriminator: nameof(SingleChoiceQuestion))]
    [JsonDerivedType(typeof(MultipleChoiceQuestion), typeDiscriminator: nameof(MultipleChoiceQuestion))]
    [JsonDerivedType(typeof(OpenQuestion), typeDiscriminator: nameof(OpenQuestion))]
    public abstract class Question
    {
        /// <summary>
        /// Gets or sets the text of the question.
        /// </summary>
        public required string Text { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of points that can be awarded for this question.
        /// </summary>
        public virtual int MaxPoints { get; set; }
    }
}