using Graidex.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Interfaces
{
    /// <summary>
    /// Defines the basic operations for a concrete <see cref="Subject"/> repository.
    /// </summary>
    public interface ISubjectRepository : IRepository<Subject>
    {
        /// <summary>
        /// Gets a subject by its id.
        /// </summary>
        /// <param name="id">The id of the subject to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="Subject"/> object with the specified id.</returns>
        public Task<Subject?> GetById(int id);
    }
}
