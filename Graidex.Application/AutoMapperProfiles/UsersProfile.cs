using AutoMapper;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users.Students;
using Graidex.Application.DTOs.Users.Teachers;
using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.AutoMapperProfiles
{   
    /// <summary>
    /// Represents a class that contains mapping profiles for users.
    /// </summary>
    public class UsersProfile : Profile
    {   
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersProfile"/> class.
        /// </summary>
        public UsersProfile()
        {
            MapStudent();
            MapTeacher();
        }

        private void MapStudent()
        {
            CreateMap<CreateStudentDto, Student>();
            CreateMap<UserAuthDto, Student>();
            CreateMap<StudentInfoDto, Student>();

            CreateMap<Student, StudentInfoDto>();
        }

        private void MapTeacher()
        {
            CreateMap<CreateTeacherDto, Teacher>();
            CreateMap<UserAuthDto, Teacher>();
            CreateMap<TeacherInfoDto, Teacher>();

            CreateMap<Teacher, TeacherInfoDto>();
        }
    }
}
