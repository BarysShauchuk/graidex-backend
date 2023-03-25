using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Users;
using Graidex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Graidex.Infrastructure.Repositories
{   
    /// <summary>
    /// Repository class that implements the <see cref="IRepository{TEntity}"/> interface for the <see cref="Teacher"/> model.
    /// </summary>
    public class TeacherRepository : IRepository<Teacher>
    {
        private readonly GraidexDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="GraidexDbContext"/> object used to perform database operations.</param>
        public TeacherRepository(GraidexDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets a <see cref="Teacher"/> object from the database with a specified id.
        /// </summary>
        /// <param name="id">The unique identifier of the <see cref="Teacher"/> object to be returned.</param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="Teacher"/> with the specified id.</returns>
        public async Task<Teacher?> GetById(int id)
        {
            return await this.context.Teachers.SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Gets a list of all <see cref="Teacher"/> objects in the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation to get all <see cref="Teacher"/> objects in the database.</returns>
        public IQueryable<Teacher> GetAll()
        {
            return this.context.Teachers;
        }

        /// <summary>
        /// Adds a <see cref="Teacher"/> object to the database.
        /// </summary>
        /// <param name="entity">The <see cref="Teacher"/> object to be added to the database.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        public async Task Add(Teacher entity)
        {
            await this.context.Teachers.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates a <see cref="Teacher"/> object in the database.
        /// </summary>
        /// <param name="entity">The updated <see cref="Teacher"/> object to be saved to the database.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        public async Task Update(Teacher entity)
        {
            this.context.Teachers.Update(entity);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a <see cref="Teacher"/> object from the database.
        /// </summary>
        /// <param name="entity">The <see cref="Teacher"/> object to be deleted from the database.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public async Task Delete(Teacher entity)
        {
            this.context.Teachers.Remove(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
