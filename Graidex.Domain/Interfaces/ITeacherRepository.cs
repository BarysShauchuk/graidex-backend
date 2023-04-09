using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Interfaces
{
    public interface ITeacherRepository : IRepository<Teacher>
    {
        public Task<Teacher?> GetById(int id);
        public Task<Teacher?> GetByEmail(string email);        
    }
}
