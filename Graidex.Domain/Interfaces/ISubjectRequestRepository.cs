using Graidex.Domain.Models;
using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Interfaces
{
    public interface ISubjectRequestRepository : IRepository<SubjectRequest>
    {
        /// <summary>
        /// Gets a subject request by its id.
        /// </summary>
        /// <param name="studentId">The id of the request to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="SubjectRequest"/> object with the specified id.</returns>
        public Task<SubjectRequest?> GetById(int id);
    }
}
