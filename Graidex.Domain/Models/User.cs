using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models
{
    public abstract class User
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }

        protected User(string email, string name, string surname, string password)
        {
            Email = email;
            Name = name;
            Surname = surname;
            Password = password;
        }
    }
}
