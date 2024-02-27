using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Factories.Answers
{
    public class AnswerFactory : IAnswerFactory
    {
        public Answer CreateAnswer(Question question, int index)
        {
            Answer answer = question switch
            {
                OpenQuestion => new OpenAnswer(),
                SingleChoiceQuestion => new SingleChoiceAnswer(),
                MultipleChoiceQuestion => new MultipleChoiceAnswer(),
                _ => throw new NotImplementedException()
            };

            answer.Feedback = question.DefaultComment;
            answer.QuestionIndex = index;

            return answer;
        }
    }
}
