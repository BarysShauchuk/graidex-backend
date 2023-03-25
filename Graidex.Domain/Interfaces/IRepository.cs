using System.Linq;
using System.Threading.Tasks;

namespace Graidex.Domain.Interfaces
{
    /// <summary>
    /// Defines the basic operations for a generic repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity the repository is for.</typeparam>
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// Gets an entity by its id.
        /// </summary>
        /// <param name="id">The id of the entity to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="TEntity"/> object with the specified id.</returns>
        public Task<TEntity?> GetById(int id);

        /// <summary>
        /// Gets a list of all entities.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation to get all <see cref="TEntity"/> objects in the database.</returns>
        public IQueryable<TEntity> GetAll();

        /// <summary>
        /// Adds an entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        public Task Add(TEntity entity);

        /// <summary>
        /// Updates an entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        public Task Update(TEntity entity);

        /// <summary>
        /// Deletes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public Task Delete(TEntity entity);
    }
}
