using Graidex.Application.DTOs.Users;
using Graidex.Application.ResultObjects.Generic;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Users
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public Task<Result<Student>> CreateStudent(StudentDto student)
        {
            throw new NotImplementedException();
        }
    }
}
