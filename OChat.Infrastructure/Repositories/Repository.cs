using System;
using System.Threading.Tasks;
using OChat.Infrastructure.Exceptions;
using OChat.Infrastructure.Persistance;

namespace OChat.Infrastructure.Repositories
{
    public abstract class Repository<TEntity> 
        where TEntity : class
    {
        protected readonly OChatContext _dbContext;

        protected Repository(OChatContext dbContext)
            => _dbContext = dbContext;

        protected async Task<TEntity> GetEntityByIdAsync(Guid id, String exceptionMessage)
        {
            var entity = await _dbContext
                           .Set<TEntity>()
                           .FindAsync(id)
                           .AsTask();

            return entity is null
                ? throw new NotFoundException(exceptionMessage)
                : entity;
        }

        protected async Task Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveEntityAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
