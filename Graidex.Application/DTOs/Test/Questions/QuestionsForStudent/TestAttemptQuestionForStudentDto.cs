using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Graidex.Application.DTOs.Test.Questions.QuestionsForStudent.ConcreteQuestionsForStudent;

namespace Graidex.Application.DTOs.Test.Questions.QuestionsForStudent
{
    [JsonDerivedType(typeof(TestAttemptOpenQuestionForStudentDto), typeDiscriminator: nameof(TestAttemptOpenQuestionForStudentDto))]
    [JsonDerivedType(typeof(TestAttemptSingleChoiceQuestionForStudentDto), typeDiscriminator: nameof(TestAttemptSingleChoiceQuestionForStudentDto))]
    [JsonDerivedType(typeof(TestAttemptMultipleChoiceQuestionForStudentDto), typeDiscriminator: nameof(TestAttemptMultipleChoiceQuestionForStudentDto))]
    public abstract class TestAttemptQuestionForStudentDto
    {
        /// <summary>
        /// Gets or sets the text of the question.
        /// </summary>
        public required string Text { get; set; }
        public string DefaultComment { get; set; } = string.Empty;
    }
}
