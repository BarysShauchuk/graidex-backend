using AutoMapper;
using Graidex.Application.DTOs.Test.Answers.TestAttempt;
using Graidex.Application.DTOs.Test.Answers.TestResultAnswers;
using Graidex.Application.DTOs.Test.Questions;
using Graidex.Application.DTOs.Test.Questions.ChoiceOptions;
using Graidex.Application.DTOs.Test.Questions.ConcreteQuestions;
using Graidex.Application.DTOs.Test.Questions.QuestionsForStudent;
using Graidex.Application.DTOs.Test.Questions.QuestionsForStudent.ChoiceOptionsForStudent;
using Graidex.Application.DTOs.Test.Questions.QuestionsForStudent.ConcreteQuestionsForStudent;
using Graidex.Application.DTOs.Test.TestDraft;
using Graidex.Application.DTOs.Test.TestResult;
using Graidex.Application.Factories.Tests;
using Graidex.Domain.Models;
using Graidex.Domain.Models.Tests;
using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.ChoiceOptions;
using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Graidex.Application.AutoMapperProfiles
{
    /// <summary>
    /// Represents a class that contains mapping profiles for subjects.
    /// </summary>
    public class TestProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectsProfile"/> class.
        /// </summary>
        public TestProfile()
        {
            CreateMap<CreateTestDraftDto, TestDraft>();
            CreateMap<TestDraft, GetTestDraftDto>();
            CreateMap<UpdateTestDraftDto, TestDraft>();
            CreateMap<CreateTestDto, Test>();
            CreateMap<CreateTestDto, TestDraftToTestParameters>();
            CreateMap<Test, GetTestDto>()
                .ForMember(dest => dest.AllowedStudents, act => act.MapFrom(src => src.AllowedStudents.Select(x => x.Email)));
            CreateMap<Test, GetVisibleTestDto>();
            CreateMap<UpdateTestDto, Test>();
            CreateMap<UpdateTestTimeDto, Test>();
            CreateMap<Test, CreateTestDraftDto>();



            CreateMap<Question, TestBaseQuestionDto>()
                .Include<OpenQuestion, TestBaseOpenQuestionDto>()
                .Include<SingleChoiceQuestion, TestBaseSingleChoiceQuestionDto>()
                .Include<MultipleChoiceQuestion, TestBaseMultipleChoiceQuestionDto>();

            CreateMap<OpenQuestion, TestBaseOpenQuestionDto>();
            CreateMap<SingleChoiceQuestion, TestBaseSingleChoiceQuestionDto>();
            CreateMap<MultipleChoiceQuestion, TestBaseMultipleChoiceQuestionDto>();

            CreateMap<ChoiceOption, ChoiceOptionDto>();
            CreateMap<MultipleChoiceOption, MultipleChoiceOptionDto>();


            CreateMap<TestBaseQuestionDto, Question>()
                .Include<TestBaseOpenQuestionDto, OpenQuestion>()
                .Include<TestBaseSingleChoiceQuestionDto, SingleChoiceQuestion>()
                .Include<TestBaseMultipleChoiceQuestionDto, MultipleChoiceQuestion>();

            CreateMap<TestBaseOpenQuestionDto, OpenQuestion>();
            CreateMap<TestBaseSingleChoiceQuestionDto, SingleChoiceQuestion>();
            CreateMap<TestBaseMultipleChoiceQuestionDto, MultipleChoiceQuestion>();

            CreateMap<ChoiceOptionDto, ChoiceOption>();
            CreateMap<MultipleChoiceOptionDto, MultipleChoiceOption>();



            CreateMap<Question, TestAttemptQuestionForStudentDto>()
                .Include<OpenQuestion, TestAttemptOpenQuestionForStudentDto>()
                .Include<SingleChoiceQuestion, TestAttemptSingleChoiceQuestionForStudentDto>()
                .Include<MultipleChoiceQuestion, TestAttemptMultipleChoiceQuestionForStudentDto>();

            CreateMap<OpenQuestion, TestAttemptOpenQuestionForStudentDto>();
            CreateMap<SingleChoiceQuestion, TestAttemptSingleChoiceQuestionForStudentDto>();
            CreateMap<MultipleChoiceQuestion, TestAttemptMultipleChoiceQuestionForStudentDto>();

            CreateMap<ChoiceOption, ChoiceOptionForStudentDto>();
            CreateMap<MultipleChoiceOption, MultipleChoiceOptionForStudentDto>();



            CreateMap<Answer, GetAnswerForStudentDto>()
                .Include<OpenAnswer, GetOpenAnswerForStudentDto>()
                .Include<SingleChoiceAnswer, GetSingleChoiceAnswerForStudentDto>()
                .Include<MultipleChoiceAnswer, GetMultipleChoiceAnswerForStudentDto>();

            CreateMap<OpenAnswer, GetOpenAnswerForStudentDto>();
            CreateMap<SingleChoiceAnswer, GetSingleChoiceAnswerForStudentDto>();
            CreateMap<MultipleChoiceAnswer, GetMultipleChoiceAnswerForStudentDto>();



            CreateMap<GetAnswerForStudentDto, Answer>()
                .Include<GetOpenAnswerForStudentDto, OpenAnswer>()
                .Include<GetSingleChoiceAnswerForStudentDto, SingleChoiceAnswer>()
                .Include<GetMultipleChoiceAnswerForStudentDto, MultipleChoiceAnswer>();

            CreateMap<GetOpenAnswerForStudentDto, OpenAnswer>();
            CreateMap<GetSingleChoiceAnswerForStudentDto, SingleChoiceAnswer>();
            CreateMap<GetMultipleChoiceAnswerForStudentDto, MultipleChoiceAnswer>();

            CreateMap<TestResult, GetTestResultForTeacherDto>();

            CreateMap<LeaveFeedbackForAnswerDto, Answer>();
            CreateMap<LeaveFeedbackForAnswerDto, OpenAnswer>();
            CreateMap<LeaveFeedbackForAnswerDto, SingleChoiceAnswer>();
            CreateMap<LeaveFeedbackForAnswerDto, MultipleChoiceAnswer>();

            CreateMap<Answer, GetResultAnswerDto>()
                .Include<OpenAnswer, GetResultOpenAnswerDto>()
                .Include<SingleChoiceAnswer, GetResultSingleChoiceAnswerDto>()
                .Include<MultipleChoiceAnswer, GetResultMultipleChoiceAnswerDto>();
            CreateMap<OpenAnswer, GetResultOpenAnswerDto>();
            CreateMap<SingleChoiceAnswer, GetResultSingleChoiceAnswerDto>();
            CreateMap<MultipleChoiceAnswer, GetResultMultipleChoiceAnswerDto>();
        }
    }
}
