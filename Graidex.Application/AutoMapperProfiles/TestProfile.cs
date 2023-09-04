using AutoMapper;
using Graidex.Application.DTOs.Test.TestDraft;
using Graidex.Domain.Models;
using Graidex.Domain.Models.Tests;
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
        }
    }
}
