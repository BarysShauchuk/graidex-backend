using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Tests;
using Graidex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Graidex.Infrastructure.Repositories
{
    /// <summary>
    /// Repository class that implements the <see cref="ITestRepository"/> 
    /// interface for the <see cref="Test"/> and <see cref="Test"/> models.
    /// </summary>
    public class TestRepository : ITestRepository
    {
        private readonly GraidexDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="GraidexDbContext"/> object used to perform database operations.</param>
        public TestRepository(GraidexDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets a <see cref="Test"/> object from the database with a specified id.
        /// </summary>
        /// <param name="id">The unique identifier of the <see cref="Test"/> object to be returned.</param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="Test"/> with the specified id.</returns>
        public async Task<Test?> GetById(int id)
        {
            return await this.context.Tests.SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Gets a list of all <see cref="Test"/> objects in the database.
        /// </summary>
        /// <returns>A result of the query of all <see cref="Test"/> objects in the database.</returns>
        public IQueryable<Test> GetAll()
        {
            return this.context.Tests;
        }

        /// <summary>
        /// Adds a <see cref="Test"/> object to the database.
        /// </summary>
        /// <param name="entity">The <see cref="Test"/> object to be added to the database.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        public async Task Add(Test entity)
        {
            await this.context.Tests.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates a <see cref="Test"/> object in the database.
        /// </summary>
        /// <param name="entity">The updated <see cref="Test"/> object to be saved to the database.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        public async Task Update(Test entity)
        {
            this.context.Tests.Update(entity);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a <see cref="Test"/> object from the database.
        /// </summary>
        /// <param name="entity">The <see cref="Test"/> object to be deleted from the database.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public async Task Delete(Test entity)
        {
            this.context.Tests.Remove(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
