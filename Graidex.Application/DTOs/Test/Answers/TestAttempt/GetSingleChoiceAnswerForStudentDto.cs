using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.Answers.TestAttempt
{
    public class GetSingleChoiceAnswerForStudentDto : GetAnswerForStudentDto
    {
        public required int ChoiceOptionIndex { get; set; } = -1;
    }
}
