using Graidex.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Tests.Fakes
{
    internal class FakeRepository<TEntity> : IRepository<TEntity>
    {
        public List<TEntity> Entities { get; set; } = new();

        public Task Add(TEntity entity)
        {
            this.Entities.Add(entity);
            return Task.CompletedTask;
        }

        public Task Delete(TEntity entity)
        {
            this.Entities.Remove(entity);
            return Task.CompletedTask;
        }

        public IQueryable<TEntity> GetAll()
        {
            return this.Entities.AsQueryable();
        }

        // TODO: Remove this method
        public virtual Task<TEntity?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(TEntity entity)
        {
            return Task.CompletedTask;
        }
    }
}
