using Graidex.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Interfaces
{
    /// <summary>
    /// Defines the basic operations for a concrete <see cref="Test"/> repository.
    /// </summary>
    public interface ITestRepository : IRepository<Test>
    {
        /// <summary>
        /// Gets a test by its id.
        /// </summary>
        /// <param name="id">The id of the test to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="Test"/> object with the specified id.</returns>
        public Task<Test?> GetById(int id);
    }
}
