using Graidex.Domain.Interfaces;
using Graidex.Domain.Models;
using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Tests.Fakes
{
    internal class FakeSubjectRepository : FakeRepository<Subject>, ISubjectRepository
    {
        public Task<Subject?> GetById(int id)
        {
            return Task.FromResult(this.Entities.SingleOrDefault(x => x.Id == id));
        }

        public Task<SubjectContent[]> GetContentById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
