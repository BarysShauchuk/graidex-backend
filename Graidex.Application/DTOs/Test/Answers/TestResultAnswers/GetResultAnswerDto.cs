using Graidex.Application.DTOs.Test.Answers.TestResultAnswers;
using Graidex.Application.DTOs.Test.Questions.ConcreteQuestions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.Answers.TestAttempt
{
    [JsonDerivedType(typeof(GetResultOpenAnswerDto), typeDiscriminator: nameof(GetResultOpenAnswerDto))]
    [JsonDerivedType(typeof(GetResultSingleChoiceAnswerDto), typeDiscriminator: nameof(GetResultSingleChoiceAnswerDto))]
    [JsonDerivedType(typeof(GetResultMultipleChoiceAnswerDto), typeDiscriminator: nameof(GetResultMultipleChoiceAnswerDto))]
    public class GetResultAnswerDto
    {
        public int Points { get; set; }
        public string? Feedback { get; set; }
        public int QuestionIndex { get; set; }
    }
}