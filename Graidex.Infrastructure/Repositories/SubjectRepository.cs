﻿using Graidex.Domain.Interfaces;
using Graidex.Domain.Models;
using Graidex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Graidex.Infrastructure.Repositories
{
    /// <summary>
    /// Repository class that implements the <see cref="ISubjectRepository"/> interface for the <see cref="Subject"/> model.
    /// </summary>
    public class SubjectRepository : ISubjectRepository
    {
        private readonly GraidexDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="GraidexDbContext"/> object used to perform database operations.</param>
        public SubjectRepository(GraidexDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets a <see cref="Subject"/> object from the database with a specified id.
        /// </summary>
        /// <param name="id">The unique identifier of the <see cref="Subject"/> object to be returned.</param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="Subject"/> with the specified id.</returns>
        public async Task<Subject?> GetById(int id)
        {
            return await this.context.Subjects.SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Gets a list of all <see cref="Subject"/> objects in the database.
        /// </summary>
        /// <returns>A result of the query of all <see cref="Subject"/> objects in the database.</returns>
        public IQueryable<Subject> GetAll()
        {
            return this.context.Subjects;
        }

        /// <summary>
        /// Adds a <see cref="Subject"/> object to the database.
        /// </summary>
        /// <param name="entity">The <see cref="Subject"/> object to be added to the database.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        public async Task Add(Subject entity)
        {
            await this.context.Subjects.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates a <see cref="Subject"/> object in the database.
        /// </summary>
        /// <param name="entity">The updated <see cref="Subject"/> object to be saved to the database.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        public async Task Update(Subject entity)
        {
            this.context.Subjects.Update(entity);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a <see cref="Subject"/> object from the database.
        /// </summary>
        /// <param name="entity">The <see cref="Subject"/> object to be deleted from the database.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public async Task Delete(Subject entity)
        {
            this.context.Subjects.Remove(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
