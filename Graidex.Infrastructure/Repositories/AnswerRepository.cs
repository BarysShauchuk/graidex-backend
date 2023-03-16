using Graidex.Domain.Interfaces;
using Graidex.Domain.Models;
using Graidex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Graidex.Infrastructure.Repositories
{
    public class AnswerRepository : IRepository<Answer>
    {
        private readonly GraidexDbContext context;

        public AnswerRepository(GraidexDbContext context)
        {
            this.context = context;
        }
        public async Task<Answer> GetById(int id)
        {
            return await this.context.Answers.SingleAsync(x => x.Id == id);
        }
        public async Task<List<Answer>> GetAll()
        {
            return await this.context.Answers.ToListAsync();
        }

        public async Task Add(Answer entity)
        {
            await this.context.Answers.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task Update(Answer entity)
        {
            this.context.Answers.Update(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task Delete(Answer entity)
        {
            this.context.Answers.Remove(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
