using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graidex.Application.DTOs.Test.Questions.QuestionsForStudent.ChoiceOptionsForStudent;

namespace Graidex.Application.DTOs.Test.Questions.QuestionsForStudent.ConcreteQuestionsForStudent
{
    public class TestAttemptMultipleChoiceQuestionForStudentDto : TestAttemptQuestionForStudentDto
    {
        public required List<MultipleChoiceOptionForStudentDto> Options { get; set; }

        public int PointsPerCorrectAnswer { get; set; }
    }
}
