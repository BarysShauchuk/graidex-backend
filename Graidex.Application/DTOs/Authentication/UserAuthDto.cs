using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Authentication
{
    /// <summary>
    /// Represents an object containing user authentication data.
    /// </summary>
    public class UserAuthDto
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public required string Password { get; set; }
    }
}
