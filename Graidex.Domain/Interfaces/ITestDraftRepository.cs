using Graidex.Domain.Models.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Interfaces
{
    public interface ITestDraftRepository : IRepository<TestDraft>
    {
        /// <summary>
        /// Gets a test draft by its id.
        /// </summary>
        /// <param name="id">The id of the test to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation to find 
        /// the <see cref="TestDraft"/> object with the specified id.
        /// </returns>
        public Task<TestDraft?> GetById(int id);
    }
}
