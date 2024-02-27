using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Graidex.Application.DTOs.Test.Questions.ConcreteQuestions;

namespace Graidex.Application.DTOs.Test.Questions
{
    [JsonDerivedType(typeof(TestBaseOpenQuestionDto), typeDiscriminator: nameof(TestBaseOpenQuestionDto))]
    [JsonDerivedType(typeof(TestBaseSingleChoiceQuestionDto), typeDiscriminator: nameof(TestBaseSingleChoiceQuestionDto))]
    [JsonDerivedType(typeof(TestBaseMultipleChoiceQuestionDto), typeDiscriminator: nameof(TestBaseMultipleChoiceQuestionDto))]
    public abstract class TestBaseQuestionDto
    {
        /// <summary>
        /// Gets or sets the text of the question.
        /// </summary>
        public required string Text { get; set; }
        public string DefaultFeedback { get; set; } = string.Empty;
    }
}
