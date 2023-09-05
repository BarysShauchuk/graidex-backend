using AutoMapper;
using Graidex.Application.DTOs.Test.Questions;
using Graidex.Application.DTOs.Test.TestDraft;
using Graidex.Domain.Models;
using Graidex.Domain.Models.Tests;
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
            CreateMap<TestDraft, DraftToTestDto>();
            CreateMap<DraftToTestDto, Test>();
            CreateMap<CreateTestDto, Test>();
            CreateMap<Test, GetTestDto>();
            CreateMap<UpdateTestDto, Test>();
            CreateMap<Test, CreateTestDraftDto>();
            CreateMap<TestDraft, DuplicateDraftDto>();
            CreateMap<DuplicateDraftDto, TestDraft>();
            CreateMap<Question, TestQuestionDto>()
                .Include<OpenQuestion, TestOpenQuestionDto>()
                .Include<SingleChoiceQuestion, TestSingleChoiceQuestionDto>()
                .Include<MultipleChoiceQuestion, TestMultipleChoiceQuestionDto>();

            CreateMap<OpenQuestion, TestOpenQuestionDto>();
            CreateMap<SingleChoiceQuestion, TestSingleChoiceQuestionDto>();
            CreateMap<MultipleChoiceQuestion, TestMultipleChoiceQuestionDto>();

            CreateMap<ChoiceOption, ChoiceOptionDto>();
            CreateMap<MultipleChoiceOption, MultipleChoiceOptionDto>();


            CreateMap<TestQuestionDto, Question>()
                .Include<TestOpenQuestionDto, OpenQuestion>()
                .Include<TestSingleChoiceQuestionDto, SingleChoiceQuestion>()
                .Include<TestMultipleChoiceQuestionDto, MultipleChoiceQuestion>();

            CreateMap<TestOpenQuestionDto, OpenQuestion>();
            CreateMap<TestSingleChoiceQuestionDto, SingleChoiceQuestion>();
            CreateMap<TestMultipleChoiceQuestionDto, MultipleChoiceQuestion>();

            CreateMap<ChoiceOptionDto, ChoiceOption>();
            CreateMap<MultipleChoiceOptionDto, MultipleChoiceOption>();
        }
    }
}
