using Graidex.Application.DTOs.Test.Questions.ConcreteQuestions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.Answers.TestAttempt
{
    [JsonDerivedType(typeof(GetOpenAnswerForStudentDto), typeDiscriminator: nameof(GetOpenAnswerForStudentDto))]
    [JsonDerivedType(typeof(GetSingleChoiceAnswerForStudentDto), typeDiscriminator: nameof(GetSingleChoiceAnswerForStudentDto))]
    [JsonDerivedType(typeof(GetMultipleChoiceAnswerForStudentDto), typeDiscriminator: nameof(GetMultipleChoiceAnswerForStudentDto))]
    public class GetAnswerForStudentDto
    {
    }
}
