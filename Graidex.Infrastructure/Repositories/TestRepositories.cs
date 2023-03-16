using Graidex.Domain.Interfaces;
using Graidex.Domain.Models;
using Graidex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Graidex.Infrastructure.Repositories
{
    public class TestRepository : IRepository<Test>
    {
        private readonly GraidexDbContext context;

        public TestRepository(GraidexDbContext context)
        {
            this.context = context;
        }
        public async Task<Test> GetById(int id)
        {
            return await this.context.Tests.SingleAsync(x => x.Id == id);
        }
        public async Task<List<Test>> GetAll()
        {
            return await this.context.Tests.ToListAsync();
        }
        
        public async Task Add(Test entity)
        {
            await this.context.Tests.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task Update(Test entity)
        {
            this.context.Tests.Update(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task Delete(Test entity)
        {
            this.context.Tests.Remove(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
