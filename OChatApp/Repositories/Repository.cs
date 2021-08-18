﻿using Microsoft.EntityFrameworkCore;
using OChatApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly OChatAppContext _dbContext;

        protected Repository(OChatAppContext dbContext)
            => _dbContext = dbContext;

        public Task<TEntity> GetByIdAsync(string id)
            => _dbContext
                .Set<TEntity>()
                .FindAsync(id)
                .AsTask();

        public async Task InsertNew(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}