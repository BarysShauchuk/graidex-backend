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
    public interface IAnswer<out T> where T : Question
    {
        /// <summary>
        /// Gets the question this answer relates to.
        /// </summary>
        public T Question { get; }

        /// <summary>
        /// Gets or sets the number of points awarded for this answer.
        /// </summary>
        public int Points { get; set; }
    }
}
