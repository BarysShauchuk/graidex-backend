using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;

namespace Graidex.Application.Services.TestChecking.AnswerCheckers.AnswerCheckersAI
{
    public interface IAnswerCheckerAI
    {
        public Task EvaluateAsync(Question question, Answer answer, CancellationToken cancellationToken);

        public Type QuestionType { get; }
        public Type AnswerType { get; }
    }
}
