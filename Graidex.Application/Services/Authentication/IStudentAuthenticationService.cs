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

namespace Graidex.Application.Services.Authentication
{
    /// <summary>
    /// Interface for student authentication service.
    /// </summary>
    public interface IStudentAuthenticationService
    {

        /// <summary>
        /// Register student.
        /// </summary>
        /// <param name="student"><see cref="CreateStudentDto"/> object with student data for registration.</param>
        /// <returns>
        /// A task that represents the asynchronous operation to get one of:
        /// <see cref="Success"/>, <see cref="ValidationFailed"/>, <see cref="UserAlreadyExists"/>.
        /// </returns>
        public Task<OneOf<Success, ValidationFailed, UserAlreadyExists>> RegisterStudentAsync(CreateStudentDto student);

        /// <summary>
        /// Login student.
        /// </summary>
        /// <param name="student"><see cref="UserAuthDto"/> object with student credentials for login.</param>
        /// <returns>
        /// A task that represents the asynchronous operation to get one of:
        /// token string, <see cref="NotFound"/>, <see cref="WrongPassword"/>.
        /// </returns>
        public Task<OneOf<string, UserNotFound, WrongPassword>> LoginStudentAsync(UserAuthDto student);
    }
}
