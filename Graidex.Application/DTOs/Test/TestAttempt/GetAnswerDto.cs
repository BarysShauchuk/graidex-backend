using Graidex.Application.DTOs.Test.Answers.TestAttempt;
using Graidex.Application.DTOs.Test.Questions.QuestionsForStudent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.TestAttempt
{
    public class GetAnswerDto
    {
        public required TestAttemptQuestionForStudentDto Question { get; set; }
        public required GetAnswerForStudentDto Answer { get; set; }
    }
}
