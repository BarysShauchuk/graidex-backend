using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Users
{
    /// <summary>
    /// Represents a user of the application.
    /// </summary>
    public abstract class User
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [EmailAddress]
        public required string Email { get; set; }

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

        /// <summary>
        /// Gets or sets the password hash of the user.
        /// </summary>
        public required string PasswordHash { get; set; }
    }
}
