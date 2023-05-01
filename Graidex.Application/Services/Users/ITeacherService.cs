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
    public interface ITeacherService
    {
        public Task<OneOf<TeacherInfoDto>> GetByEmailAsync(string email);

        public Task<OneOf<TeacherInfoDto, NotFound>> GetCurrentAsync();

        public Task<OneOf<Success, ValidationFailed, NotFound>> UpdateCurrentInfoAsync(TeacherInfoDto teacherInfo);

        public Task<OneOf<Success, ValidationFailed, NotFound, WrongPassword>> UpdateCurrentPasswordAsync(ChangePasswordDto passwords);

        public Task<OneOf<Success, NotFound, WrongPassword>> DeleteCurrent(string password);
    }
}