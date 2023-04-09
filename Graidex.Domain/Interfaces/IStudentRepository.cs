using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        public Task<Student?> GetById(int id);
        public Task<Student?> GetByEmail(string email);
    }
}
