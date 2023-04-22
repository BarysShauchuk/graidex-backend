using Graidex.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Users
{
    /// <summary>
    /// Represents object containing full teacher information.
    /// </summary>
    public class StudentDto
    {
        /// <summary>
        /// Gets or sets student authentication data.
        /// </summary>
        public required UserAuthDto AuthInfo { get; set; }

        /// <summary>
        /// Gets or sets student information.
        /// </summary>
        public required StudentInfoDto StudentInfo { get; set; }
    }
}
