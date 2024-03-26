using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;

namespace Graidex.Application.Services.TestChecking.AnswerCheckers.AnswerCheckersAI
{
    public abstract class AnswerCheckerAI<Q, A> : IAnswerCheckerAI
        where Q : Question where A : Answer
    {
        public Type QuestionType { get; } = typeof(Q);
        public Type AnswerType { get; } = typeof(A);

        protected abstract Task EvaluateAsync(Q question, A answer, CancellationToken cancellationToken);

        public async Task EvaluateAsync(Question question, Answer answer, CancellationToken cancellationToken)
        {
            if (question is not Q q)
            {
                throw new ArgumentException($"Wrong question type, {QuestionType.Name} expected", nameof(question));
            }

            if (answer is not A a)
            {
                throw new ArgumentException($"Wrong answer type, {AnswerType.Name} expected", nameof(answer));
            }

            await EvaluateAsync(q, a, cancellationToken);
        }
    }
}
