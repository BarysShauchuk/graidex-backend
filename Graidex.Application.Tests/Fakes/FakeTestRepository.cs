using Graidex.Domain.Interfaces;
using Graidex.Domain.Models;
using Graidex.Domain.Models.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Tests.Fakes
{
    internal class FakeTestRepository : FakeRepository<Test>, ITestRepository
    {
        public Task<Test?> GetById(int id)
        {
            return Task.FromResult(this.Entities.SingleOrDefault(x => x.Id == id));
        }
    }
}
