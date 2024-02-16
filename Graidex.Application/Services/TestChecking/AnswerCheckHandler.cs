using Graidex.Application.Services.TestChecking.AnswerCheckers;
using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.TestChecking
{
    public class AnswerCheckHandler : IAnswerCheckHandler
    {
        private readonly Dictionary<(Type, Type), IAnswerChecker> answerCheckers = new();

        public AnswerCheckHandler(IEnumerable<IAnswerChecker> answerCheckers) 
        {            
            this.answerCheckers = answerCheckers
                .ToDictionary(x => (x.QuestionType, x.AnswerType));
        }

        public async Task EvaluateAsync(Question question, Answer answer)
        {
            if (!this.answerCheckers.TryGetValue(
                (question.GetType(), answer.GetType()), out var answerChecker))
            {
                throw new KeyNotFoundException("Question-Answer types pair not found");
            }

            await answerChecker.EvaluateAsync(question, answer);
        }
    }
}
