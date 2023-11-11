using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graidex.Application.DTOs.Test.Questions.QuestionsForStudent.ChoiceOptionsForStudent;

namespace Graidex.Application.DTOs.Test.Questions.QuestionsForStudent.ConcreteQuestionsForStudent
{
    public class TestAttemptSingleChoiceQuestionForStudentDto : TestAttemptQuestionForStudentDto
    {
        /// <summary>
        /// Gets or sets the collection of choice options for the question.
        /// </summary>
        public required List<ChoiceOptionForStudentDto> Options { get; set; }

        public int MaxPoints { get; set; }
    }
}
