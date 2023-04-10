using Graidex.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Interfaces
{
    /// <summary>
    /// Defines the basic operations for a concrete <see cref="TestResult"/> repository.
    /// </summary>
    public interface ITestResultRepository : IRepository<TestResult>
    {
        /// <summary>
        /// Gets a test result by its id.
        /// </summary>
        /// <param name="id">The id of the test result to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="TestResult"/> object with the specified id.</returns>
        public Task<TestResult?> GetById(int id);
    }
}
