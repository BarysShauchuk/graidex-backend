using Graidex.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Users.Teachers
{
    /// <summary>
    /// Represents dto for creating a teacher.
    /// </summary>
    public class CreateTeacherDto
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets the name(first name) of the teacher.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the surname(last name) of the teacher.
        /// </summary>
        public required string Surname { get; set; }
    }
}
