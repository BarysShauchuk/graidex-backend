using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Interfaces
{
    /// <summary>
    /// Defines the basic operations for a concrete <see cref="Student"/> repository.
    /// </summary>
    public interface IStudentRepository : IRepository<Student>
    {
        /// <summary>
        /// Gets a student by its id.
        /// </summary>
        /// <param name="id">The id of the student to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="Student"/> object with the specified id.</returns>
        public Task<Student?> GetById(int id);

        /// <summary>
        /// Gets a student by its email.
        /// </summary>
        /// <param name="email">The email of the student to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="Student"/> object with the specified email.</returns>
        public Task<Student?> GetByEmail(string email);
    }
}
