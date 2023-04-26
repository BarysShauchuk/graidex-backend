using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Users
{
    /// <summary>
    /// Represents information about teacher.
    /// </summary>
    public class TeacherInfoDto
    {
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
