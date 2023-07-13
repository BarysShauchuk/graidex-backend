using AutoMapper;
using Graidex.Application.DTOs.SubjectRequest;
using Graidex.Domain.Models;
using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.AutoMapperProfiles
{
    public class SubjectRequestsProfile : Profile
    {
        public SubjectRequestsProfile()
        {
            CreateMap<OutgoingSubjectRequestDto, SubjectRequest>();
            CreateMap<SubjectRequest, IncomingSubjectRequestDto>();
            CreateMap<SubjectRequest, SubjectRequestInfoDto>();

            CreateMap<Student, SubjectRequestInfoDto>().
                ForMember(dest => dest.StudentEmail, opt => opt.MapFrom(src => src.Email));
        }
    }
}
