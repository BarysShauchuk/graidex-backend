using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Questions;
using Graidex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Graidex.Infrastructure.Repositories
{
    /// <summary>
    /// Repository class that implements the <see cref="IRepository{TEntity}"/> interface for the <see cref="Question"/> model.
    /// </summary>
    public class QuestionRepository : IRepository<Question>
    {
        private readonly GraidexDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="GraidexDbContext"/> object used to perform database operations.</param>
        public QuestionRepository(GraidexDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets a <see cref="Question"/> object from the database with a specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="Question"/> with the specified id.</returns>
        public async Task<Question?> GetById(int id)
        {
            return await this.context.Questions.SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Gets a list of all <see cref="Question"/> objects in the database.
        /// </summary>
        /// <returns>A result of the query of all <see cref="Question"/> objects in the database.</returns>
        public IQueryable<Question> GetAll()
        {
            return this.context.Questions;
        }

        /// <summary>
        /// Adds a <see cref="Question"/> object to the database.
        /// </summary>
        /// <param name="entity">The <see cref="Question"/> object to be added to the database.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        public async Task Add(Question entity)
        {
            await this.context.Questions.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates a <see cref="Question"/> object in the database.
        /// </summary>
        /// <param name="entity">The updated <see cref="Question"/> object to be saved to the database.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        public async Task Update(Question entity)
        {
            this.context.Questions.Update(entity);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a <see cref="Question"/> object from the database.
        /// </summary>
        /// <param name="entity">The <see cref="Question"/> object to be deleted from the database.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public async Task Delete(Question entity)
        {
            this.context.Questions.Remove(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
