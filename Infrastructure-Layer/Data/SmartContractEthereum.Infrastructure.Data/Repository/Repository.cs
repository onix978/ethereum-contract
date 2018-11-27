using SmartContractEthereum.Domain.Interfaces.Repositories;
using SmartContractEthereum.Infrastructure.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartContractEthereum.Infrastructure.Data.Repository
{
    public class Repository<TEntity> : IDisposable, IRepository<TEntity> where TEntity : class
    {
        internal SmartContractEthereumContext _context;
        internal DbSet<TEntity> _dbSet;

        public Repository()
        {
            if (_context == null)
            {
                _context = new SmartContractEthereumContext();
                _dbSet = _context.Set<TEntity>();
            }
        }

        public Repository(SmartContractEthereumContext context)
        {
            this._context = context;
            this._dbSet = _context.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            //typeof(TEntity).GetProperty("Active").SetValue(entity, true);

            _dbSet.Add(entity);
            _context.Entry(entity).State = EntityState.Added;

            _context.SaveChanges();

            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            //typeof(TEntity).GetProperty("Active").SetValue(entity, true);

            _dbSet.Add(entity);
            _context.Entry(entity).State = EntityState.Added;

            await _context.SaveChangesAsync();

            return entity;
        }

        /// <summary> 
        /// Método que adiciona uma lista de novos objetos ao banco de dados da aplicação. 
        /// </summary> 
        public void AddAll(IList<TEntity> entity)
        {
            foreach (var item in entity)
            {
                //typeof(TEntity).GetProperty("Active").SetValue(item, true);

                _context.Entry(item).State = EntityState.Added;
                _dbSet.Add(item);
            }

            _context.SaveChanges();
        }

        /// <summary> 
        /// Método que adiciona uma lista de novos objetos ao banco de dados da aplicação assíncrono. 
        /// </summary> 
        public async Task AddAllAsync(IList<TEntity> entity)
        {
            foreach (var item in entity)
            {
                //typeof(TEntity).GetProperty("Active").SetValue(item, true);

                _context.Entry(item).State = EntityState.Added;
                _dbSet.Add(item);
            }

            await _context.SaveChangesAsync();
        }

        public void DeleteByID(int id)
        {
            var entity = _dbSet.Find(id);
            _dbSet.Remove(entity);

            _context.SaveChanges();
        }

        public async Task DeleteByIDAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);

            await _context.SaveChangesAsync();
        }

        /// <summary> 
        /// Método que deleta um objeto no banco de dados da aplicação. 
        /// </summary> 
        public void Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);

            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public void SoftDeleteByID(int id)
        {
            var entity = _dbSet.Find(id);

            typeof(TEntity).GetProperty("Active").SetValue(entity, false);

            Update(entity);
        }

        public async Task SoftDeleteByIDAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            typeof(TEntity).GetProperty("Active").SetValue(entity, false);

            await UpdateAsync(entity);
        }

        /// <summary> 
        /// Método que altera o ativo do objeto no banco de dados da aplicação. 
        /// </summary> 
        public void SoftDelete(TEntity entity)
        {
            typeof(TEntity).GetProperty("Active").SetValue(entity, false);

            Update(entity);
        }

        /// <summary> 
        /// Método que deleta um objeto no banco de dados da aplicação assíncrono. 
        /// </summary> 
        public async Task DeleteAsync(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary> 
        /// Método que altera o ativo do objeto no banco de dados da aplicação assíncrono. 
        /// </summary> 
        public async Task SoftDeleteAsync(TEntity entity)
        {
            typeof(TEntity).GetProperty("Active").SetValue(entity, false);

            await UpdateAsync(entity);
        }

        /// <summary> 
        /// Método que deleta um ou varios objetos no banco de dados da aplicação, mediante uma expressão LINQ. 
        /// </summary> 
        public void DeleteAll(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _dbSet;
            IList<TEntity> listDelete = filter != null ? query.Where(filter).ToList() : query.ToList();

            foreach (var item in listDelete)
                _dbSet.Remove(item);

            _context.SaveChanges();
        }

        /// <summary> 
        /// Método que deleta um ou varios objetos no banco de dados da aplicação assíncrono, mediante uma expressão LINQ. 
        /// </summary> 
        public async Task DeleteAllAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _dbSet;
            IList<TEntity> listDelete = filter != null ? await query.Where(filter).ToListAsync() : await query.ToListAsync();

            foreach (var item in listDelete)
                _dbSet.Remove(item);

            await _context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            //typeof(TEntity).GetProperty("Updated").SetValue(entity, DateTime.Now);

            var entry = _context.Entry<TEntity>(entity);
            var pkey = _dbSet.Create().GetType().GetProperty("id") != null ? _dbSet.Create().GetType().GetProperty("id").GetValue(entity) : null;

            if (pkey == null)
                pkey = _dbSet.Create().GetType().GetProperty("ID") != null ? _dbSet.Create().GetType().GetProperty("ID").GetValue(entity) : null;

            TEntity attachedEntity;

            if (entry.State == EntityState.Detached)
            {
                var set = _context.Set<TEntity>();

                if (pkey != null)
                    attachedEntity = set.Find(pkey);  // access the key 
                else
                    attachedEntity = set.Find(_dbSet.Create().GetType().GetProperty("StoreKeeperID").GetValue(entity), _dbSet.Create().GetType().GetProperty("StoreID").GetValue(entity));

                if (attachedEntity != null)
                {
                    var attachedEntry = _context.Entry(attachedEntity);

                    attachedEntry.CurrentValues.SetValues(entity);

                }
                else
                    entry.State = EntityState.Modified; // attach the entity 
            }

            _context.SaveChanges();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            //typeof(TEntity).GetProperty("UpdatedDate").SetValue(entity, DateTime.Now);

            var entry = _context.Entry<TEntity>(entity);
            var pkey = _dbSet.Create().GetType().GetProperty("id") != null ? _dbSet.Create().GetType().GetProperty("id").GetValue(entity) : _dbSet.Create().GetType().GetProperty("ID").GetValue(entity);

            TEntity attachedEntity;

            if (entry.State == EntityState.Detached)
            {
                var set = _context.Set<TEntity>();

                if (pkey != null)
                    attachedEntity = await set.FindAsync(pkey);  // access the key 
                else
                    attachedEntity = await set.FindAsync(_dbSet.Create().GetType().GetProperty("StoreKeeperID").GetValue(entity), _dbSet.Create().GetType().GetProperty("StoreID").GetValue(entity));

                if (attachedEntity != null)
                {
                    var attachedEntry = _context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                    entry.State = EntityState.Modified; // attach the entity 
            }

            await _context.SaveChangesAsync();
        }

        /// <summary> 
        /// Método que busca uma lista de objetos no banco de dados da aplicação e retorna-a no tipo IEnumerable<TEntity>. 
        /// </summary> 
        public IList<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                return orderBy(query).ToList();
            else
                return query.ToList();
        }

        /// <summary> 
        /// Método que busca uma lista de objetos no banco de dados da aplicação e retorna-a no tipo IEnumerable assíncrono<TEntity>. 
        /// </summary> 
        public async Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                return await orderBy(query).ToListAsync();
            else
                return await query.ToListAsync();
        }

        public TEntity GetById(int? id = null)
        {
            return id != null ? _dbSet.Find(id) : _dbSet.FirstOrDefault();
        }

        public async Task<TEntity> GetByIdAsync(int? id)
        {
            return id != null ? await _dbSet.FindAsync(id) : await _dbSet.FirstOrDefaultAsync();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeExps)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (includeExps.Count() > 0)
                query = includeExps.Aggregate(query, (current, exp) => current.Include(exp));

            return query.ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, Object>>[] includeExps)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (includeExps != null)
                query = includeExps.Aggregate(query, (current, exp) => current.Include(exp));

            return await query.ToListAsync();
        }

        public IEnumerable<TEntity> GetAllAsNoTracking(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            return query.AsNoTracking().ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsNoTrackingAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            return await query.AsNoTracking().ToListAsync();
        }

        public void Dispose()
        {
            _dbSet = null;
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

