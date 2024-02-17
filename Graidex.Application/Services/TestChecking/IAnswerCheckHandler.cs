using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;

namespace Graidex.Application.Services.TestChecking
{
    public interface IAnswerCheckHandler
    {
        public Task EvaluateAsync(Question question, Answer answer);
    }
}