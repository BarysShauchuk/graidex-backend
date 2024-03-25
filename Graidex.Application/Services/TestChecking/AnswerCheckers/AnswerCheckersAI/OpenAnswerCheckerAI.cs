using Graidex.Application.Services.AI;
using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;

namespace Graidex.Application.Services.TestChecking.AnswerCheckers.AnswerCheckersAI
{
    public class OpenAnswerCheckerAI : AnswerCheckerAI<OpenQuestion, OpenAnswer>
    {   
        private readonly IAIService aiService;
        public OpenAnswerCheckerAI(IAIService aiService)
        {
            this.aiService = aiService;
        }

        protected async override Task EvaluateAsync(OpenQuestion question, OpenAnswer answer, CancellationToken cancellationToken)
        {
            answer.Points = 0;

            var result = await this.aiService.EvaluateOpenAnswerAsync(question, answer, cancellationToken);

            if (result.IsT0)
            {
                answer.Points = (int)Math.Round(result.AsT0.Points * question.MaxPoints / 10d);
                answer.Feedback = result.AsT0.Feedback;
                answer.RequireTeacherReview = false;
            }

            if (result.IsT1)
            {
                answer.Feedback = result.AsT1.Comment;
                answer.RequireTeacherReview = true;
                answer.Points = 0;
            }
        }
    }
}
