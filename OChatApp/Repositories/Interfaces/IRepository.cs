﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Repositories.Interfaces
{
    public interface IRepository<TEntity> 
        where TEntity: class
    {
        Task<TEntity> GetEntityByIdAsync(Guid id);

        Task SaveEntityAsync(TEntity entity);
    }
}
