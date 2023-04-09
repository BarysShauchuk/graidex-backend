using Graidex.Application.DTOs.Authentication;
using Graidex.Application.ResultObjects.Generic;
using Graidex.Application.ResultObjects.NonGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Authentication
{
    public interface IAuthenticationService
    {
        public Task<Result> RegisterStudent(StudentAuthDto student);
        public Task<Result<string>> LoginStudent(UserAuthDto student, string keyToken);
        public Task<Result> RegisterTeacher(TeacherAuthDto student);
        public Task<Result<string>> LoginTeacher(UserAuthDto student, string keyToken);
    }
}
