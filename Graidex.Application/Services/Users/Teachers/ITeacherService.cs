using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users.Teachers;
using Graidex.Application.OneOfCustomTypes;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Users.Teachers
{
    public interface ITeacherService
    {
        public Task<OneOf<TeacherInfoDto>> GetByEmailAsync(string email);

        public Task<OneOf<TeacherInfoDto, UserNotFound>> GetCurrentAsync();

        public Task<OneOf<Success, ValidationFailed, UserNotFound>> UpdateCurrentInfoAsync(TeacherInfoDto teacherInfo);

        public Task<OneOf<Success, ValidationFailed, UserNotFound, WrongPassword>> UpdateCurrentPasswordAsync(ChangePasswordDto passwords);

        public Task<OneOf<Success, UserNotFound, WrongPassword>> DeleteCurrent(string password);
    }
}