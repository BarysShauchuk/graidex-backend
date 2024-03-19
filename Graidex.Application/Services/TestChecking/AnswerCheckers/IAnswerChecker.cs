using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.TestChecking.AnswerCheckers
{
    public interface IAnswerChecker
    {
        public void Evaluate(Question question, Answer answer);

        public Type QuestionType { get; }
        public Type AnswerType { get; }
    }
}
