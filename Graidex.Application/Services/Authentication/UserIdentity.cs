using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Authentication
{
    /// <summary>
    /// User identity.
    /// </summary>
    public readonly struct UserIdentity
    {
        private UserIdentity(string email, string role)
        {
            this.Email = email;
            this.Role = role;
        }

        /// <summary>
        /// Gets the user email.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Gets the user role.
        /// </summary>
        public string Role { get; }

        /// <summary>
        /// Gets the string representation of the user identity.
        /// </summary>
        /// <param name="email">
        /// User email.
        /// </param>
        /// <param name="role">
        /// User role.
        /// </param>
        /// <returns>
        /// User identity string.
        /// </returns>
        public static string Get(string email, string role)
        {
            var identity = new UserIdentity(email, role);
            return JsonSerializer.Serialize(identity);
        }

        /// <summary>
        /// Gets the string representation of the user identity.
        /// </summary>
        /// <param name="user">
        /// Domain user model.
        /// </param>
        /// <returns>
        /// User identity string or null if the user role is not supported.
        /// </returns>
        public static string? Get(User user)
        {
            return user switch
            {
                Teacher teacher => Get(teacher.Email, "Teacher"),
                Student student => Get(student.Email, "Student"),
                _ => null,
            };
        }
    }
}
