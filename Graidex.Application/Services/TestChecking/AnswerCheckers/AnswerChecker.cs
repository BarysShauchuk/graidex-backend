using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.TestChecking.AnswerCheckers
{
    public abstract class AnswerChecker<Q, A> : IAnswerChecker 
        where Q : Question where A : Answer
    {
        public Type QuestionType { get; } = typeof(Q);
        public Type AnswerType { get; } = typeof(A);

        protected abstract void Evaluate(Q question, A answer);

        public void Evaluate(Question question, Answer answer)
        {
            if (question is not Q q)
            {
                throw new ArgumentException($"Wrong question type, {QuestionType.Name} expected", nameof(question));
            }

            if (answer is not A a)
            {
                throw new ArgumentException($"Wrong answer type, {AnswerType.Name} expected", nameof(answer));
            }

            this.Evaluate(q, a);
        }
    }
}
