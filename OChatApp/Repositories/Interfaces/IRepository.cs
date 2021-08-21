using System.Collections.Generic;
using System.Threading.Tasks;

namespace OChatApp.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity> GetByIdAsync(string id, string exceptionMessage);

        Task InsertNew(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(TEntity entity);
    }
}
