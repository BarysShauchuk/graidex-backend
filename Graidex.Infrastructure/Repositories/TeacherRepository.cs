using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Users;
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

        public async Task<Teacher?> GetById(int id)
        {
            return await this.context.Teachers.SingleOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<Teacher> GetAll()
        {
            return this.context.Teachers;
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
