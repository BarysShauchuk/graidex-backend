using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Authentication
{
    public class TeacherAuthDto : UserAuthDto
    {
        /// <summary>
        /// Gets or sets the name(first name) of the user.
        /// </summary>
        [MaxLength(50)]
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the surname(last name) of the user.
        /// </summary>
        [MaxLength(50)]
        public required string Surname { get; set; }
    }
}
