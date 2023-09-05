using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Tests;
using Graidex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Graidex.Infrastructure.Repositories
{
    /// <summary>
    /// Repository class that implements the <see cref="ITestDraftRepository"/> 
    /// interface for the <see cref="TestDraft"/> and <see cref="TestDraft"/> models.
    /// </summary>
    public class TestDraftRepository : ITestDraftRepository
    {
        private readonly GraidexDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestDraftRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="GraidexDbContext"/> object used to perform database operations.</param>
        public TestDraftRepository(GraidexDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets a <see cref="TestDraft"/> object from the database with a specified id.
        /// </summary>
        /// <param name="id">The unique identifier of the <see cref="TestDraft"/> object to be returned.</param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="TestDraft"/> with the specified id.</returns>
        public async Task<TestDraft?> GetById(int id)
        {
            return await this.context.TestDrafts.SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Gets a list of all <see cref="TestDraft"/> objects in the database.
        /// </summary>
        /// <returns>A result of the query of all <see cref="TestDraft"/> objects in the database.</returns>
        public IQueryable<TestDraft> GetAll()
        {
            return this.context.TestDrafts;
        }

        /// <summary>
        /// Adds a <see cref="TestDraft"/> object to the database.
        /// </summary>
        /// <param name="entity">The <see cref="TestDraft"/> object to be added to the database.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        public async Task Add(TestDraft entity)
        {
            await this.context.TestDrafts.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates a <see cref="TestDraft"/> object in the database.
        /// </summary>
        /// <param name="entity">The updated <see cref="TestDraft"/> object to be saved to the database.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        public async Task Update(TestDraft entity)
        {
            this.context.TestDrafts.Update(entity);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a <see cref="TestDraft"/> object from the database.
        /// </summary>
        /// <param name="entity">The <see cref="TestDraft"/> object to be deleted from the database.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public async Task Delete(TestDraft entity)
        {
            this.context.TestDrafts.Remove(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
