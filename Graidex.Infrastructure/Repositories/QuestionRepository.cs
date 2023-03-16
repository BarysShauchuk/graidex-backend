using Graidex.Domain.Interfaces;
using Graidex.Domain.Models;
using Graidex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Graidex.Infrastructure.Repositories
{
    public class QuestionRepository : IRepository<Question>
    {
        private readonly GraidexDbContext context;

        public QuestionRepository(GraidexDbContext context)
        {
            this.context = context;
        }
        public async Task<Question> GetById(int id)
        {
            return await this.context.Questions.SingleAsync(x => x.Id == id);
        }
        public async Task<List<Question>> GetAll()
        {
            return await this.context.Questions.ToListAsync();
        }

        public async Task Add(Question entity)
        {
            await this.context.Questions.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task Update(Question entity)
        {
            this.context.Questions.Update(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task Delete(Question entity)
        {
            this.context.Questions.Remove(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
