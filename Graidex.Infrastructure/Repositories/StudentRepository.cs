using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Users;
using Graidex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Graidex.Infrastructure.Repositories
{
    /// <summary>
    /// Repository class that implements the <see cref="IStudentRepository"/> interface for the <see cref="Student"/> model.
    /// </summary>
    public class StudentRepository : IStudentRepository
    {
        private readonly GraidexDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="GraidexDbContext"/> object used to perform database operations.</param>
        public StudentRepository(GraidexDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets a <see cref="Student"/> object from the database with a specified id.
        /// </summary>
        /// <param name="id">The unique identifier of the <see cref="Student"/> object to be returned.</param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="Student"/> with the specified id.</returns>
        public async Task<Student?> GetById(int id)
        {
            return await this.context.Students.SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Gets a <see cref="Student"/> object from the database with a specified email.
        /// </summary>
        /// <param name="email">The unique email of the <see cref="Student"/> object to be returned.</param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="Student"/> with the specified email.</returns>
        public async Task<Student?> GetByEmail(string email)
        {
            return await this.context.Students.SingleOrDefaultAsync(x => x.Email == email);
        }

        /// <summary>
        /// Gets a list of all <see cref="Student"/> objects in the database.
        /// </summary>
        /// <returns>A result of the query of all <see cref="Student"/> objects in the database.</returns>
        public IQueryable<Student> GetAll()
        {
            return this.context.Students;
        }

        /// <summary>
        /// Adds a <see cref="Student"/> object to the database.
        /// </summary>
        /// <param name="entity">The <see cref="Student"/> object to be added to the database.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        public async Task Add(Student entity)
        {
            await this.context.Students.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates a <see cref="Student"/> object in the database.
        /// </summary>
        /// <param name="entity">The updated <see cref="Student"/> object to be saved to the database.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        public async Task Update(Student entity)
        {
            this.context.Students.Update(entity);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a <see cref="Student"/> object from the database.
        /// </summary>
        /// <param name="entity">The <see cref="Student"/> object to be deleted from the database.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public async Task Delete(Student entity)
        {
            this.context.Students.Remove(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
