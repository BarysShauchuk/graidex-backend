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
    /// <summary>
    /// Interface for teacher authentication service.
    /// </summary>
    public interface ITeacherAuthenticationService
    {
        /// <summary>
        /// Register teacher.
        /// </summary>
        /// <param name="teacher"><see cref="TeacherAuthDto"/> object with teacher data for registration.</param>
        /// <returns>
        /// A task that represents the asynchronous operation to get <see cref="Result"/>
        /// of teacher registration.
        /// </returns>
        public Task<Result> RegisterTeacher(TeacherAuthDto teacher);

        /// <summary>
        /// Login teacher.
        /// </summary>
        /// <param name="teacher"><see cref="UserAuthDto"/> object with teacher credentials for login.</param>
        /// <param name="keyToken">Secret key for generating token.</param>
        /// <returns>
        /// A task that represents the asynchronous operation to get <see cref="Result"/>
        /// of teacher login with teacher token in case of success.
        /// </returns>
        public Task<Result<string>> LoginTeacher(UserAuthDto teacher, string keyToken);
    }
}
