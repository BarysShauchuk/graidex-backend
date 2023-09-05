using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.Questions
{
    [JsonDerivedType(typeof(TestOpenQuestionDto), typeDiscriminator: nameof(TestOpenQuestionDto))]
    [JsonDerivedType(typeof(TestSingleChoiceQuestionDto), typeDiscriminator: nameof(TestSingleChoiceQuestionDto))]
    [JsonDerivedType(typeof(TestMultipleChoiceQuestionDto), typeDiscriminator: nameof(TestMultipleChoiceQuestionDto))]
    public abstract class TestQuestionDto
    {
        /// <summary>
        /// Gets or sets the text of the question.
        /// </summary>
        public required string Text { get; set; }
        public string DefaultComment { get; set; } = string.Empty;
    }
}
