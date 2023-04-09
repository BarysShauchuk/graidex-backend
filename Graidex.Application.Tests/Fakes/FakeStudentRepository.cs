using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Tests.Fakes
{
    internal class FakeStudentRepository : FakeRepository<Student>, IStudentRepository
    {
        public override Task<Student?> GetById(int id)
        {
            return Task.FromResult(this.Entities.SingleOrDefault(x => x.Id == id));
        }

        public Task<Student?> GetByEmail(string email)
        {
            return Task.FromResult(this.Entities.SingleOrDefault(x => x.Email == email));
        }
    }
}
