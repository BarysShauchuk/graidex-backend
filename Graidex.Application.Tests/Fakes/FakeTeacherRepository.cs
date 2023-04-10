using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Tests.Fakes
{
    internal class FakeTeacherRepository : FakeRepository<Teacher>, ITeacherRepository
    {
        public Task<Teacher?> GetById(int id)
        {
            return Task.FromResult(this.Entities.SingleOrDefault(x => x.Id == id));
        }

        public Task<Teacher?> GetByEmail(string email)
        {
            return Task.FromResult(this.Entities.SingleOrDefault(x => x.Email == email));
        }
    }
}
