using AutoMapper;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users;
using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.AutoMapperProfiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            MapStudent();
            MapTeacher();
        }

        private void MapStudent()
        {
            CreateMap<StudentDto, Student>()
                .IncludeMembers(x => x.AuthInfo, x => x.StudentInfo);
            CreateMap<UserAuthDto, Student>();
            CreateMap<StudentInfoDto, Student>();

            CreateMap<Student, StudentInfoDto>();
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
