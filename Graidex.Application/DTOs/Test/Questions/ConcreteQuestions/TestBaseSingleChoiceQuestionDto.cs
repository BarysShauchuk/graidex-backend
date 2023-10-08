using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graidex.Application.DTOs.Test.Questions.ChoiceOptions;

namespace Graidex.Application.DTOs.Test.Questions.ConcreteQuestions
{
    public class TestBaseSingleChoiceQuestionDto : TestBaseQuestionDto
    {
        /// <summary>
        /// Gets or sets the collection of choice options for the question.
        /// </summary>
        public required List<ChoiceOptionDto> Options { get; set; }
        public int CorrectOptionIndex { get; set; }

        public int MaxPoints { get; set; }
    }
}
