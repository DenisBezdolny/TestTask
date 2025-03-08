using Microsoft.EntityFrameworkCore;
using TestTask.Domain.Interfaces.Repositories;

namespace TestTask.Infrastructure.Repositories.Repositories
{
    // A generic repository implementation using EF Core.
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly TestTaskDbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(TestTaskDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        //Adds a new entity.
        public async Task CreateAsync(TEntity entity)
        { 
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        // Deletes an entity by its Id.
        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        // Retrieves all entities.
        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();

        //Finds an entity by its Id asynchronously.
        public async Task<TEntity?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

        // Updates an existing entity. SaveChangesAsync is called after updating.
        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
