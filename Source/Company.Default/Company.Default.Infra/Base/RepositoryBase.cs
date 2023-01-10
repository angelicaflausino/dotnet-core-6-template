using Company.Default.Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Company.Default.Infra.Base
{
    public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        private readonly AppDbContext _context;

        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }

        public virtual void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        //TODO: Utilizar ou não ValueTask
        public virtual Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
           var result = _context.Set<TEntity>().AddAsync(entity, cancellationToken);

           return result.AsTask();
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        public virtual Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            return _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }

        public virtual void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public virtual TEntity FindById(TKey keyId)
        {
            return _context.Set<TEntity>().Find(keyId);
        }

        public virtual ValueTask<TEntity> FindByIdAsync(TKey keyId, CancellationToken cancellationToken)
        {
            return _context.Set<TEntity>().FindAsync(keyId, cancellationToken);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsEnumerable();
        }

        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().Where(expression).AsEnumerable();
        }

        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            var query = _context.Set<TEntity>().Where(expression);

            if(includes.Length > 0)
            {
                foreach (var include in includes)
                    query.Include(include);
            }

            return query.AsEnumerable();
        }

        public virtual IEnumerable<TSelector> GetSelect<TSelector>(Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, TSelector>> select, params string[] includes)
        {
            var query = _context.Set<TEntity>().Where(expression);

            if(includes != null)
            {
                foreach (var include in includes)
                    query.Include(include);
            }
            
            return query.Select(select).ToList();
        }

        public virtual TEntity GetById(TKey keyId)
        {
            return _context.Set<TEntity>().Find(keyId);
        }

        public virtual ValueTask<TEntity> GetByIdAsync(TKey keyId, CancellationToken cancellationToken)
        {
            return _context.Set<TEntity>().FindAsync(keyId, cancellationToken);
        }

        public virtual PagedResult<TEntity> GetPaged(Expression<Func<TEntity, bool>> expression, int page, int size, string sort, params string[] includes)
        {
            var query = _context.Set<TEntity>().AsNoTracking().Where(expression);

            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
                
            return query.OrderBy(sort).PageResult(page, size); 
        }

        public virtual IQueryable<TEntity> GetQueryable()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public virtual void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().UpdateRange(entities);
        }
    }
}
