using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Users.Students
{
    /// <summary>
    /// Represents information about student with email identifier.
    /// </summary>
    public class StudentDto
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the name(first name) of the student.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the surname(last name) of the student.
        /// </summary>
        public required string Surname { get; set; }

        /// <summary>
        /// Gets or sets the custom ID for the student.
        /// </summary>
        /// <remarks>
        /// The custom ID is used for easy search of students, without the need to use database id or name.
        /// </remarks>
        public string? CustomId { get; set; }
    }
}
