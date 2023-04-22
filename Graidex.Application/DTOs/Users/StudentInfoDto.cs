using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Users
{
    /// <summary>
    /// Represents information about student.
    /// </summary>
    public class StudentInfoDto
    {
        /// <summary>
        /// Gets or sets the name(first name) of the student.
        /// </summary>
        [MaxLength(50)]
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the surname(last name) of the student.
        /// </summary>
        [MaxLength(50)]
        public required string Surname { get; set; }

        /// <summary>
        /// Gets or sets the custom ID for the student.
        /// </summary>
        /// <remarks>
        /// The custom ID is used for easy search of students, without the need to use database id or name.
        /// </remarks>
        [MaxLength(15)]
        public string? CustomId { get; set; }
    }
}
