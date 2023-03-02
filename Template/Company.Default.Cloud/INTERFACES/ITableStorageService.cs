using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using System.Linq.Expressions;

namespace $safeprojectname$.Interfaces
{
    public interface ITableStorageService
    {
        /// <summary>
        /// Add Entity into table
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        void AddEntity<TEntity>(string tableName, TEntity entity) where TEntity : class, ITableEntity, new();

        /// <summary>
        /// Asynchronously add Entity into table
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task"/>
        /// </returns>
        Task AddEntityAsync<TEntity>(string tableName, TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new();

        /// <summary>
        /// Create table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>
        /// <see cref="TableItem"/>
        /// </returns>
        TableItem CreateTable(string tableName);

        /// <summary>
        /// Asynchronously create table
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="TableItem"/>
        /// </returns>
        Task<TableItem> CreateTableAsync(string tableName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete records in batch
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task"/>
        /// </returns>
        Task DeleteBatchAsync<TEntity>(string tableName, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new();

        /// <summary>
        /// Delete Entity from table
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        void DeleteEntity(string tableName, string partitionKey, string rowKey);

        /// <summary>
        /// Asynchronously delete Entity from table
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task"/>
        /// </returns>
        Task DeleteEntityAsync(string tableName, string partitionKey, string rowKey, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete Table
        /// </summary>
        /// <param name="tableName"></param>
        void DeleteTable(string tableName);

        /// <summary>
        /// Asynchronously delete table
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task"/>
        /// </returns>
        Task DeleteTableAsync(string tableName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get entities from table
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="filter"></param>
        /// <param name="maxPerPage"></param>
        /// <param name="properties"></param>
        /// <returns>
        /// <see cref="IEnumerable{T}"/> where T is a object inherited from <see cref="ITableEntity"/>
        /// </returns>
        IEnumerable<TEntity> GetEntities<TEntity>(string tableName, Expression<Func<TEntity, bool>> filter, int? maxPerPage = null, IEnumerable<string> properties = null) where TEntity : class, ITableEntity, new();

        /// <summary>
        /// Asynchronously Get entities from table
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="filter"></param>
        /// <param name="maxPerPage"></param>
        /// <param name="properties"></param>
        /// <param name="cancellation"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="IEnumerable{T}"/> and T is a object inherited from <see cref="ITableEntity"/>
        /// </returns>
        Task<IEnumerable<TEntity>> GetEntitiesAsync<TEntity>(string tableName, Expression<Func<TEntity, bool>> filter, int? maxPerPage = null, IEnumerable<string> properties = null, CancellationToken cancellation = default) where TEntity : class, ITableEntity, new();

        /// <summary>
        /// Get Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <param name="properties"></param>
        /// <returns>
        /// object inherited from <see cref="ITableEntity"/> 
        /// </returns>
        TEntity GetEntity<TEntity>(string tableName, string partitionKey, string rowKey, IEnumerable<string> properties = null) where TEntity : class, ITableEntity, new();

        /// <summary>
        /// Asynchronously Get Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <param name="properties"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a object inherited from <see cref="ITableEntity"/> 
        /// </returns>
        Task<TEntity> GetEntityAsync<TEntity>(string tableName, string partitionKey, string rowKey, IEnumerable<string> properties = null, CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new();
        
        /// <summary>
        /// Get paged search tables
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="maxPerPage"></param>
        /// <returns>
        /// <see cref="IEnumerable{T}"/> where T is a <see cref="TableItem"/>
        /// </returns>
        IEnumerable<TableItem> GetPageableTable(string filter, int? maxPerPage = 20);

        /// <summary>
        /// Asynchronously Get paged search tables
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="maxPerPage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="IEnumerable{T}"/> and T is a <see cref="TableItem"/>
        /// </returns>
        Task<IEnumerable<TableItem>> GetPageableTableAsync(string filter, int maxPerPage = 20, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously Insert records in batch 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="IDictionary{TKey, TValue}"/>, TKey and TValue is typeof <see cref="string"/>
        /// </returns>
        Task<IDictionary<string, string>> InsertBatchAsync<TEntity>(string tableName, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new();

        /// <summary>
        /// Asynchronously perform multiple actions types in batch
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entitiesActions"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="IDictionary{TKey, TValue}"/>, TKey and TValue is typeof <see cref="string"/>
        /// </returns>
        Task<Dictionary<string, string>> MixedBatchAsync<TEntity>(string tableName, IEnumerable<Tuple<TableTransactionActionType, TEntity>> entitiesActions, CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new();

        /// <summary>
        /// Asynchronously try get Table by name
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>
        /// <see cref="TableItem"/>?
        /// </returns>
        TableItem? TryGetTable(string tableName);

        /// <summary>
        /// Asynchronously try get Table by name
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="TableItem"/>?
        /// </returns>
        Task<TableItem?> TryGetTableAsync(string tableName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously update records in batch
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <param name="updateMode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task"/>
        /// </returns>
        Task UpdateBatchAsync<TEntity>(string tableName, IEnumerable<TEntity> entities, TableUpdateMode updateMode = TableUpdateMode.Replace, CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new();

        /// <summary>
        /// Update Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entityToUpdate"></param>
        /// <param name="updateMode"></param>
        void UpdateEntity<TEntity>(string tableName, TEntity entityToUpdate, TableUpdateMode updateMode = TableUpdateMode.Replace) where TEntity : class, ITableEntity, new();

        /// <summary>
        /// Asynchronously update Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <param name="updateMode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateEntityAsync<TEntity>(string tableName, TEntity entity, TableUpdateMode updateMode = TableUpdateMode.Replace, CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new();
        
        /// <summary>
        /// Update or Insert entity if not exists
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <param name="updateMode"></param>
        void UpsertEntity<TEntity>(string tableName, ITableEntity entity, TableUpdateMode updateMode = TableUpdateMode.Replace) where TEntity : class, ITableEntity, new();

        /// <summary>
        /// Asynchronously Update or Insert entity if not exists
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <param name="updateMode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task"/>
        /// </returns>
        Task UpsertEntityAsync<TEntity>(string tableName, TEntity entity, TableUpdateMode updateMode = TableUpdateMode.Replace, CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new();
    }
}