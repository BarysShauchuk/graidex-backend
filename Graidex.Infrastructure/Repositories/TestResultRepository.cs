using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Tests;
using Graidex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Graidex.Infrastructure.Repositories
{
    /// <summary>
    /// Repository class that implements the <see cref="ITestResultRepository"/> interface for the <see cref="TestResult"/> model.
    /// </summary>
    public class TestResultRepository : ITestResultRepository
    {
        private readonly GraidexDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestResultRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="GraidexDbContext"/> object used to perform database operations.</param>
        public TestResultRepository(GraidexDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets a <see cref="TestResult"/> object from the database with a specified id.
        /// </summary>
        /// <param name="id">The unique identifier of the <see cref="TestResult"/> object to be returned.</param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="TestResult"/> with the specified id.</returns>
        public async Task<TestResult?> GetById(int id)
        {
            return await this.context.TestResults.SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Gets a list of all <see cref="TestResult"/> objects in the database.
        /// </summary>
        /// <returns>A result of the query of all <see cref="TestResult"/> objects in the database.</returns>
        public IQueryable<TestResult> GetAll()
        {
            return this.context.TestResults;
        }

        /// <summary>
        /// Adds a <see cref="TestResult"/> object to the database.
        /// </summary>
        /// <param name="entity">The <see cref="TestResult"/> object to be added to the database.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        public async Task Add(TestResult entity)
        {
            await this.context.TestResults.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates a <see cref="TestResult"/> object in the database.
        /// </summary>
        /// <param name="entity">The updated <see cref="TestResult"/> object to be saved to the database.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        public async Task Update(TestResult entity)
        {
            this.context.TestResults.Update(entity);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a <see cref="TestResult"/> object from the database.
        /// </summary>
        /// <param name="entity">The <see cref="TestResult"/> object to be deleted from the database.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public async Task Delete(TestResult entity)
        {
            this.context.TestResults.Remove(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
