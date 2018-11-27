using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartContractEthereum.Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        void AddAll(IList<TEntity> entity);
        Task AddAllAsync(IList<TEntity> entity);
        void DeleteByID(int id);
        Task DeleteByIDAsync(int id);
        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity);
        void SoftDeleteByID(int id);
        Task SoftDeleteByIDAsync(int id);
        void SoftDelete(TEntity entity);
        Task SoftDeleteAsync(TEntity entity);
        void DeleteAll(Expression<Func<TEntity, bool>> filter = null);
        Task DeleteAllAsync(Expression<Func<TEntity, bool>> filter = null);
        void Update(TEntity entity);
        Task UpdateAsync(TEntity entity);
        IList<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        TEntity GetById(int? id = null);
        Task<TEntity> GetByIdAsync(int? id);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, Object>>[] includeExps);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, Object>>[] includeExps);
        IEnumerable<TEntity> GetAllAsNoTracking(Expression<Func<TEntity, bool>> filter = null);
        Task<IEnumerable<TEntity>> GetAllAsNoTrackingAsync(Expression<Func<TEntity, bool>> filter = null);
        void Dispose();
    }
}