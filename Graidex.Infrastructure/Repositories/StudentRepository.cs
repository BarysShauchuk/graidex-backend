using Graidex.Domain.Interfaces;
using Graidex.Domain.Models;
using Graidex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Graidex.Infrastructure.Repositories
{
    public class StudentRepository : IRepository<Student>
    {
        private readonly GraidexDbContext context;

        public StudentRepository(GraidexDbContext context)
        {
            this.context = context;
        }
        public async Task<Student> GetById(int id)
        {
            return await this.context.Students.SingleAsync(x => x.Id == id);
        }
        public async Task<List<Student>> GetAll()
        {
            return await this.context.Students.ToListAsync();
        }

        public async Task Add(Student entity)
        {
            await this.context.Students.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task Update(Student entity)
        {
            this.context.Students.Update(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task Delete(Student entity)
        {
            this.context.Students.Remove(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
