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
        public Task<OneOf<StudentInfoDto>> GetByEmailAsync(string email);

        public Task<OneOf<StudentInfoDto, UserNotFound>> GetCurrentAsync();

        public Task<OneOf<Success, ValidationFailed, UserNotFound>> UpdateCurrentInfoAsync(StudentInfoDto studentInfo);

        public Task<OneOf<Success, ValidationFailed, UserNotFound, WrongPassword>> UpdateCurrentPasswordAsync(ChangePasswordDto passwords);

        public Task<OneOf<Success, UserNotFound, WrongPassword>> DeleteCurrent(string password);

        public Task<OneOf<Success, UserNotFound>> AddCurrentToSubjectAsync(int subjectId);

        public Task<OneOf<IEnumerable<StudentDto>, UserNotFound>> GetAllOfSubjectAsync(int subjectId);

        public Task<OneOf<Success, UserNotFound>> RemoveCurrentFromSubjectAsync(int subjectId);
    }
}
