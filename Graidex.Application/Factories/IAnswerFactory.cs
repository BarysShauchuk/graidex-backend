using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Factories
{
    public interface IAnswerFactory
    {
        public Answer CreateAnswer(Question question, int index);
    }
}
