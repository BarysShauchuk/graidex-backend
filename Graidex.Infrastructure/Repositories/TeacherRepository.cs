using Graidex.Domain.Interfaces;
using Graidex.Domain.Models;
using Graidex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Graidex.Infrastructure.Repositories
{
    public class TeacherRepository : IRepository<Teacher>
    {
        private readonly GraidexDbContext context;

        public TeacherRepository(GraidexDbContext context)
        {
            this.context = context;
        }
        public async Task<Teacher> GetById(int id)
        {
            return await this.context.Teachers.SingleAsync(x => x.Id == id);
        }
        public async Task<List<Teacher>> GetAll()
        {
            return await this.context.Teachers.ToListAsync();
        }

        public async Task Add(Teacher entity)
        {
            await this.context.Teachers.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task Update(Teacher entity)
        {
            this.context.Teachers.Update(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task Delete(Teacher entity)
        {
            this.context.Teachers.Remove(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
