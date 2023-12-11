using Graidex.Application.DTOs.Test.Answers.TestAttempt;
using Graidex.Application.DTOs.Test.Questions;
using Graidex.Application.DTOs.Test.Questions.QuestionsForStudent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.TestResult
{
    public class GetResultAnswerForTeacherDto
    {
        public required TestBaseQuestionDto Question { get; set; }
        public required GetResultAnswerDto Answer { get; set; }
    }
}
