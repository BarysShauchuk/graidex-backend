using Graidex.Domain.Interfaces;
using Graidex.Infrastructure.Data;
using Graidex.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Graidex.Domain.Models.Users;

namespace Graidex.Infrastructure.Repositories
{
    /// <summary>
    /// Repository class that implements the <see cref="ISubjectRequestRepository"/> interface for the <see cref="SubjectRequest"/> model.
    /// </summary>
    public class SubjectRequestRepository : ISubjectRequestRepository
    {
        private readonly GraidexDbContext context;


        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectRequestRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="GraidexDbContext"/> object used to perform database operations.</param>
        public SubjectRequestRepository(GraidexDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets a <see cref="SubjectRequest"/> object from the database with a specified id.
        /// </summary>
        /// <param name="id">The unique identifier of the <see cref="SubjectRequest"/> object to be returned.</param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="SubjectRequest"/> with the specified id.</returns>
        public async Task<SubjectRequest?> GetById(int id)
        {
            return await this.context.SubjectRequests.SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Gets a list of all <see cref="SubjectRequest"/> objects in the database.
        /// </summary>
        /// <returns>A result of the query of all <see cref="SubjectRequest"/> objects in the database.</returns>
        public IQueryable<SubjectRequest> GetAll()
        {
            return this.context.SubjectRequests;
        }

        /// <summary>
        /// Adds a <see cref="SubjectRequest"/> object to the database.
        /// </summary>
        /// <param name="entity">The <see cref="SubjectRequest"/> object to be added to the database.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        public async Task Add(SubjectRequest entity)
        {
            await this.context.SubjectRequests.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates a <see cref="SubjectRequest"/> object in the database.
        /// </summary>
        /// <param name="entity">The updated <see cref="SubjectRequest"/> object to be saved to the database.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        public async Task Update(SubjectRequest entity)
        {
            this.context.SubjectRequests.Update(entity);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a <see cref="SubjectRequest"/> object from the database.
        /// </summary>
        /// <param name="entity">The <see cref="SubjectRequest"/> object to be deleted from the database.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public async Task Delete(SubjectRequest entity)
        {
            this.context.SubjectRequests.Remove(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
