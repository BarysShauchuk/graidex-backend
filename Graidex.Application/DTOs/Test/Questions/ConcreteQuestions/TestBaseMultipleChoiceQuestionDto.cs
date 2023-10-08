using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graidex.Application.DTOs.Test.Questions.ChoiceOptions;

namespace Graidex.Application.DTOs.Test.Questions.ConcreteQuestions
{
    public class TestBaseMultipleChoiceQuestionDto : TestBaseQuestionDto
    {
        public required List<MultipleChoiceOptionDto> Options { get; set; }

        public int PointsPerCorrectAnswer { get; set; }
    }
}
