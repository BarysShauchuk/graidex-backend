using AutoMapper;
using Graidex.Application.DTOs.Test.Questions;
using Graidex.Domain.Models.Tests.ChoiceOptions;
using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.AutoMapperProfiles
{
    public class TestProfile : Profile
    {
        public TestProfile()
        {
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
