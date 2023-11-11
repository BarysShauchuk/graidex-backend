using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.Answers.TestAttempt
{
    public class GetMultipleChoiceAnswerForStudentDto : GetAnswerForStudentDto
    {
        public required List<int> ChoiceOptionIndexes { get; set; } = new();
    }
}
