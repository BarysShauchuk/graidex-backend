using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Interfaces
{
    /// <summary>
    /// Defines the basic operations for a concrete <see cref="Teacher"/> repository.
    /// </summary>
    public interface ITeacherRepository : IRepository<Teacher>
    {
        /// <summary>
        /// Gets a teacher by its id.
        /// </summary>
        /// <param name="id">The id of the teacher to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="Teacher"/> object with the specified id.</returns>
        public Task<Teacher?> GetById(int id);

        /// <summary>
        /// Gets a teacher by its email.
        /// </summary>
        /// <param name="email">The email of the teacher to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="Teacher"/> object with the specified email.</returns>
        public Task<Teacher?> GetByEmail(string email);        
    }
}
