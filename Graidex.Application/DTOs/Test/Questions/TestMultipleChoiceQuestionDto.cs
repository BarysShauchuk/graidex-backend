using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.Questions
{
    public class TestMultipleChoiceQuestionDto : TestQuestionDto
    {
        public required List<MultipleChoiceOptionDto> Options { get; set; }

        public int PointsPerCorrectAnswer { get; set; }
    }
}
