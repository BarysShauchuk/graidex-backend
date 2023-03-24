using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Users
{
    public abstract class User
    {
        [EmailAddress]
        public required string Email { get; set; }

        [MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(50)]
        public required string Surname { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
