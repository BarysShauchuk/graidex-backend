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

namespace Graidex.Application.Services.Authentication
{
    /// <summary>
    /// Interface for teacher authentication service.
    /// </summary>
    public interface ITeacherAuthenticationService
    {
        /// <summary>
        /// Register teacher.
        /// </summary>
        /// <param name="teacher"><see cref="TeacherDto"/> object with teacher data for registration.</param>
        /// <returns>
        /// A task that represents the asynchronous operation to get one of:
        /// <see cref="Success"/>, <see cref="ValidationFailed"/>, <see cref="UserAlreadyExists"/>.
        /// </returns>
        public Task<OneOf<Success, ValidationFailed, UserAlreadyExists>> RegisterTeacherAsync(TeacherDto teacher);

        /// <summary>
        /// Login teacher.
        /// </summary>
        /// <param name="teacher"><see cref="UserAuthDto"/> object with teacher credentials for login.</param>
        /// <returns>
        /// A task that represents the asynchronous operation to get one of:
        /// token string, <see cref="NotFound"/>, <see cref="WrongPassword"/>.
        /// </returns>
        public Task<OneOf<string, NotFound, WrongPassword>> LoginTeacherAsync(UserAuthDto teacher);
    }
}
