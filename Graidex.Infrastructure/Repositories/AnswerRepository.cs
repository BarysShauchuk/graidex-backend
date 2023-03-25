using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Answers;
using Graidex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Graidex.Infrastructure.Repositories
{
    /// <summary>
    /// Repository class that implements the <see cref="IRepository{TEntity}"/> interface for the <see cref="Answer"/> model.
    /// </summary>
    public class AnswerRepository : IRepository<Answer>
    {
        private readonly GraidexDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnswerRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="GraidexDbContext"/> object used to perform database operations.</param>
        public AnswerRepository(GraidexDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets a <see cref="Answer"/> object from the database with a specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="Answer"/> with the specified id.</returns>
        public async Task<Answer?> GetById(int id)
        {
            return await this.context.Answers.SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Gets a list of all <see cref="Answer"/> objects in the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation to get all <see cref="Answer"/> objects in the database.</returns>
        public IQueryable<Answer> GetAll()
        {
            return this.context.Answers;
        }

        /// <summary>
        /// Adds a <see cref="Answer"/> object to the database.
        /// </summary>
        /// <param name="entity">The <see cref="Answer"/> object to be added to the database.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        public async Task Add(Answer entity)
        {
            await this.context.Answers.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates a <see cref="Answer"/> object in the database.
        /// </summary>
        /// <param name="entity">The updated <see cref="Answer"/> object to be saved to the database.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        public async Task Update(Answer entity)
        {
            this.context.Answers.Update(entity);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a <see cref="Answer"/> object from the database.
        /// </summary>
        /// <param name="entity">The <see cref="Answer"/> object to be deleted from the database.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public async Task Delete(Answer entity)
        {
            this.context.Answers.Remove(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
