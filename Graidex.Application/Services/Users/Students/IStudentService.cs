using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users.Students;
using Graidex.Application.OneOfCustomTypes;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Users.Students
{
    public interface IStudentService
    {
        public Task<OneOf<StudentInfoDto, UserNotFound>> GetByEmailAsync(string email);

        public Task<OneOf<StudentInfoDto, UserNotFound>> GetCurrentAsync();

        public Task<OneOf<Success, ValidationFailed, UserNotFound>> UpdateCurrentInfoAsync(StudentInfoDto studentInfo);

        public Task<OneOf<Success, ValidationFailed, UserNotFound, WrongPassword>> UpdateCurrentPasswordAsync(ChangePasswordDto passwords);

        public Task<OneOf<Success, UserNotFound, WrongPassword>> DeleteCurrent(string password);

        public Task<OneOf<Success, UserNotFound, NotFound>> AddCurrentToSubjectAsync(int subjectId, string studentEmail);

        public Task<OneOf<List<StudentDto>, NotFound>> GetAllOfSubjectAsync(int subjectId);

        public Task<OneOf<Success, UserNotFound, NotFound>> RemoveCurrentFromSubjectAsync(int subjectId, string studentEmail);
    }
}
