using Graidex.Domain.Interfaces;
using Graidex.Domain.Models;
using Graidex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<SubjectContent[]> GetContentById(int id)
        {
            var data = await context.Database.SqlQuery<string>(
                $"""
                SELECT * 
                FROM SubjectContents 
                WHERE SubjectId = {id} 
                ORDER BY OrderIndex
                FOR JSON PATH
                """)
                .ToListAsync();

            var json = string.Join("", data);
            if (string.IsNullOrWhiteSpace(json))
            {
                return Array.Empty<SubjectContent>();
            }

            var content = System.Text.Json.JsonSerializer.Deserialize<SubjectContent[]>(json);

            return content ?? Array.Empty<SubjectContent>();
        }

        /// <summary>
        /// Changes the visibility of a <see cref="SubjectContent"/> object in the database.
        /// </summary>
        /// <param name="contentId">The id of the <see cref="SubjectContent"/> object to be updated.</param>
        /// <param name="isVisible">The new visibility value of the <see cref="SubjectContent"/> object.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        public async Task UpdateContentVisibilityById(int contentId, bool isVisible)
        {
            var rowsAffected = await context.Database.ExecuteSqlRawAsync(
                """
                UPDATE SubjectContents 
                SET IsVisible = @p0
                WHERE Id = @p1
                """, 
                isVisible ? 1 : 0, 
                contentId);

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"Content item with id = {contentId} isn't found");
            }

            if (rowsAffected > 1)
            {
                throw new InvalidOperationException("More than one content item updated with the same id.");
            }
        }

        /// <summary>
        /// Gets a <see cref="SubjectContent"/> object from the database with a specified id.
        /// </summary>
        /// <param name="contentId">The unique identifier of the <see cref="SubjectContent"/> object to be returned.</param>
        /// <returns>A task that represents the asynchronous operation to find the <see cref="SubjectContent"/> with the specified id or null if not found.</returns>
        /// <exception cref="InvalidOperationException">Thrown when more than one <see cref="SubjectContent"/> object is found with the same id.</exception>
        public async Task<SubjectContent?> GetContentItemById(int contentId)
        {
            var data = await context.Database.SqlQuery<string>(
                $"""
                 SELECT * 
                 FROM SubjectContents 
                 WHERE Id = {contentId}
                 FOR JSON PATH
                """).ToListAsync();

            var json = string.Join("", data);
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            var contentList = System.Text.Json.JsonSerializer.Deserialize<SubjectContent[]>(json);

            if (contentList is null || contentList.Length == 0)
            {
                return null;
            }

            if (contentList.Length > 1)
            {
                throw new InvalidOperationException("More than one content item found with the same id.");
            }

            return contentList[0];
        }

        public async Task UpdateContentOrderById(int id, double orderIndex)
        {
            var rowsAffected = await context.Database.ExecuteSqlRawAsync(
                """
                UPDATE SubjectContents 
                SET OrderIndex = @p0
                WHERE Id = @p1
                """,
                orderIndex,
                id);

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"Content item with id = {id} isn't found");
            }

            if (rowsAffected > 1)
            {
                throw new InvalidOperationException("More than one content item updated with the same id.");
            }
        }

        public async Task RefreshSubjectContentOrderingById(int subjectId)
        {
            var rowsAffected = await context.Database.ExecuteSqlRawAsync(
                """
                UPDATE SubjectContents
                SET OrderIndex = i
                FROM (SELECT OrderIndex, ROW_NUMBER() OVER ( ORDER BY OrderIndex) AS i
                      FROM SubjectContents
                      WHERE SubjectId = @p0) SubjectContents
                """,
                subjectId);
        }
    }
}
