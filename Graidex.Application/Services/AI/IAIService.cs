using Graidex.Application.DTOs.AI;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using OneOf;

namespace Graidex.Application.Services.AI
{
    public interface IAIService
    {
        public Task<OneOf<EvaluateOpenAnswerDto, ConditionFailed>> EvaluateOpenAnswerAsync(OpenQuestion question, OpenAnswer answer, CancellationToken cancellationToken);
    }
}
