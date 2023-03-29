using Graidex.Domain.Models.Questions;
using System.Text.Json.Serialization;

namespace Graidex.Domain.Models.Answers
{
    /// <summary>
    /// Represents an answer to a question in the test.
    /// </summary>
    [JsonDerivedType(typeof(SingleChoiceAnswer), typeDiscriminator: nameof(SingleChoiceAnswer))]
    [JsonDerivedType(typeof(MultipleChoiceAnswer), typeDiscriminator: nameof(MultipleChoiceAnswer))]
    [JsonDerivedType(typeof(OpenAnswer), typeDiscriminator: nameof(OpenAnswer))]
    public abstract class Answer
    {
        /// <summary>
        /// Gets or sets maximum amount of points that can be awarded for this answer.
        /// </summary>
        public virtual int MaxPoints { get; }

        /// <summary>
        /// Gets or sets amount of points awarded for this answer.
        /// </summary>
        public int Points { get; set; }
    }
}
