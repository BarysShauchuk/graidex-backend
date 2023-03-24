using Graidex.Domain.Interfaces;
using Graidex.Domain.Models;
using Graidex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Graidex.Infrastructure.Repositories
{
    public class SubjectRepository : IRepository<Subject>
    {
        private readonly GraidexDbContext context;

        public SubjectRepository(GraidexDbContext context)
        {
            this.context = context;
        }

        public async Task<Subject?> GetById(int id)
        {
            return await this.context.Subjects.SingleOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<Subject> GetAll()
        {
            return this.context.Subjects;
        }

        public async Task Add(Subject entity)
        {
            await this.context.Subjects.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task Update(Subject entity)
        {
            this.context.Subjects.Update(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task Delete(Subject entity)
        {
            this.context.Subjects.Remove(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
