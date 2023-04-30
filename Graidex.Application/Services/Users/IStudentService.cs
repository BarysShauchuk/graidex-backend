using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users;
using Graidex.Application.OneOfCustomTypes;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Users
{
    public interface IStudentService
    {
        public Task<OneOf<StudentInfoDto>> GetByEmailAsync(string email);

        public Task<OneOf<StudentInfoDto, NotFound>> GetCurrentAsync();

        public Task<OneOf<Success, ValidationFailed, NotFound>> UpdateCurrentInfoAsync(StudentInfoDto studentInfo);

        public Task<OneOf<Success, ValidationFailed, NotFound, WrongPassword>> UpdateCurrentPasswordAsync(ChangePasswordDto passwords);

        public Task<OneOf<Success, NotFound, WrongPassword>> DeleteCurrent(string password);
    }
}
