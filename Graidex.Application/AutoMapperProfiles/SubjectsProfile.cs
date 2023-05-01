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
    public class SubjectsProfile : Profile
    {
        public SubjectsProfile()
        {
            CreateMap<CreateSubjectDto, Subject>();
            CreateMap<Subject, SubjectDto>();
        }
    }
}
