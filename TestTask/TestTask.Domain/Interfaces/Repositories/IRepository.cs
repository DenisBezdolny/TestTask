namespace TestTask.Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class 
    {
        Task<TEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteByIdAsync(Guid id);

    }
}
