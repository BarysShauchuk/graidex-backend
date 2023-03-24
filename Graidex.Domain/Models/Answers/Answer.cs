using Graidex.Domain.Models.Questions;

namespace Graidex.Domain.Models.Answers
{
    public abstract class Answer
    {
        public int Id { get; set; }

        public required virtual TestResult TestResult { get; set; }
    }
}
