using Graidex.Domain.Interfaces;
using Graidex.Domain.Models;
using Graidex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Graidex.Infrastructure.Repositories
{
    public class TestResultRepository : IRepository<TestResult>
    {
        private readonly GraidexDbContext context;

        public TestResultRepository(GraidexDbContext context)
        {
            this.context = context;
        }
        public async Task<TestResult> GetById(int id)
        {
            return await this.context.TestResults.SingleAsync(x => x.Id == id);
        }
        public async Task<List<TestResult>> GetAll()
        {
            return await this.context.TestResults.ToListAsync();
        }

        public async Task Add(TestResult entity)
        {
            await this.context.TestResults.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task Update(TestResult entity)
        {
            this.context.TestResults.Update(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task Delete(TestResult entity)
        {
            this.context.TestResults.Remove(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
