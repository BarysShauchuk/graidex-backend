using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models
{
    /// <summary>
    /// Represents a user of the application.
    /// </summary>
    public abstract class User
    {   
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the name(first name) of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the surname(last name) of the user.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string Password { get; set; }
    }
}
