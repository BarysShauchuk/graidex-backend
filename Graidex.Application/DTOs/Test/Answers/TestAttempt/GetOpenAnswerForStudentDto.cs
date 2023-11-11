using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.Answers.TestAttempt
{
    public class GetOpenAnswerForStudentDto : GetAnswerForStudentDto
    {
        public required string Text { get; set; } = string.Empty;
    }
}
