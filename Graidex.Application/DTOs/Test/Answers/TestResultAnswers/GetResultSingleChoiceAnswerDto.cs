using Graidex.Application.DTOs.Test.Answers.TestAttempt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.Answers.TestResultAnswers
{
    public class GetResultSingleChoiceAnswerDto : GetResultAnswerDto
    {
        public int ChoiceOptionIndex { get; set; } = -1;
    }
}
