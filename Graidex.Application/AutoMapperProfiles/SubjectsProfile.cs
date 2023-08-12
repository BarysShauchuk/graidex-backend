using AutoMapper;
using Graidex.Application.DTOs.Subject;
using Graidex.Domain.Models;
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
    public class SubjectsProfile : Profile
    {   
        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectsProfile"/> class.
        /// </summary>
        public SubjectsProfile()
        {
            CreateMap<CreateSubjectDto, Subject>();
            CreateMap<Subject, SubjectDto>();
            CreateMap<Subject, SubjectInfoDto>();
            CreateMap<UpdateSubjectDto, Subject>();
            CreateMap<SubjectContent, SubjectContentDto>();
        }
    }
}
