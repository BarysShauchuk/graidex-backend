using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Authentication
{
    /// <summary>
    /// Represents a data transfer object for changing the password of a user.
    /// </summary>
    public class ChangePasswordDto
    {
        /// <summary>
        /// Gets or sets the current password of the user.
        /// </summary>
        public required string CurrentPassword { get; set; }

        /// <summary>
        /// Gets or sets the new password of the user.
        /// </summary>
        public required string NewPassword { get; set; }
    }
}
