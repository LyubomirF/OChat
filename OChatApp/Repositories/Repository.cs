using Microsoft.EntityFrameworkCore;
using OChatApp.Data;
using OChatApp.Repositories.Exceptions;
using OChatApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations;


namespace OChatApp.Repositories
{
    public abstract class Repository<TEntity> 
        where TEntity : class
    {
        protected readonly OChatAppContext _dbContext;

        protected Repository(OChatAppContext dbContext)
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
