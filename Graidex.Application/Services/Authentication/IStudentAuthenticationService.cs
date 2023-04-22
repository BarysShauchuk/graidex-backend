using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users;
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
    /// Interface for student authentication service.
    /// </summary>
    public interface IStudentAuthenticationService
    {

        /// <summary>
        /// Register student.
        /// </summary>
        /// <param name="student"><see cref="StudentDto"/> object with student data for registration.</param>
        /// <returns>
        /// A task that represents the asynchronous operation to get <see cref="Result"/>
        /// of student registration.
        /// </returns>
        public Task<Result> RegisterStudent(StudentDto student);

        /// <summary>
        /// Login student.
        /// </summary>
        /// <param name="student"><see cref="UserAuthDto"/> object with student credentials for login.</param>
        /// <param name="keyToken">Secret key for generating token.</param>
        /// <returns>
        /// A task that represents the asynchronous operation to get <see cref="Result"/>
        /// of student login with student token in case of success.
        /// </returns>
        public Task<Result<string>> LoginStudent(UserAuthDto student, string keyToken);
    }
}
