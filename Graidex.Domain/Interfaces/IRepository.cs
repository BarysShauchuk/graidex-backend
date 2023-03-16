namespace Graidex.Domain.Interfaces
{
    public interface IRepository<TEntity>
    {
        public Task<TEntity> GetById(int id);
        public Task<List<TEntity>> GetAll();
        public Task Add(TEntity entity);
        public Task Update(TEntity entity);
        public Task Delete(TEntity entity);
    }
}
