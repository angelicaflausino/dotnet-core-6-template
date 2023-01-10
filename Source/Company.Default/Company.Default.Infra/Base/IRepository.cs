﻿using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Company.Default.Infra.Base
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        void Add(TEntity entity);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);
        void AddRange(IEnumerable<TEntity> entities);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
        TEntity GetById(TKey keyId);
        ValueTask<TEntity> GetByIdAsync(TKey keyId, CancellationToken cancellationToken);
        TEntity FindById(TKey keyId);
        ValueTask<TEntity> FindByIdAsync(TKey keyId, CancellationToken cancellationToken);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression, params string[] includes);
        IEnumerable<TSelector> GetSelect<TSelector>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TSelector>> select, params string[] includes);
        PagedResult<TEntity> GetPaged(Expression<Func<TEntity, bool>> expression, int page, int size, string sort, params string[] includes);
        IQueryable<TEntity> GetQueryable();
    }
}