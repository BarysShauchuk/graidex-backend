using AutoMapper;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users;
using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.MapStudent();
            this.MapTeacher();
        }

        private void MapStudent()
        {
            CreateMap<StudentDto, Student>()
                .IncludeMembers(x => x.AuthInfo, x => x.StudentInfo);
            CreateMap<UserAuthDto, Student>();
            CreateMap<StudentInfoDto, Student>();
        }

        private void MapTeacher()
        {
            CreateMap<TeacherDto, Teacher>()
                .IncludeMembers(x => x.AuthInfo, x => x.TeacherInfo);
            CreateMap<UserAuthDto, Teacher>();
            CreateMap<TeacherInfoDto, Teacher>();
        }
    }
}
