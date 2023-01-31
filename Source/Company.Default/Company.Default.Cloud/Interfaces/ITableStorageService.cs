using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using System.Linq.Expressions;

namespace Company.Default.Cloud.Interfaces
{
    public interface ITableStorageService
    {
        void AddEntity<TEntity>(string tableName, TEntity entity) where TEntity : class, ITableEntity, new();
        Task AddEntityAsync<TEntity>(string tableName, TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new();
        TableItem CreateTable(string tableName);
        Task<TableItem> CreateTableAsync(string tableName, CancellationToken cancellationToken = default);
        Task DeleteBatchAsync<TEntity>(string tableName, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new();
        void DeleteEntity(string tableName, string partitionKey, string rowKey);
        Task DeleteEntityAsync(string tableName, string partitionKey, string rowKey, CancellationToken cancellationToken = default);
        void DeleteTable(string tableName);
        Task DeleteTableAsync(string tableName, CancellationToken cancellationToken = default);
        IEnumerable<TEntity> GetEntities<TEntity>(string tableName, Expression<Func<TEntity, bool>> filter, int? maxPerPage = null, IEnumerable<string> properties = null) where TEntity : class, ITableEntity, new();
        Task<IEnumerable<TEntity>> GetEntitiesAsync<TEntity>(string tableName, Expression<Func<TEntity, bool>> filter, int? maxPerPage = null, IEnumerable<string> properties = null, CancellationToken cancellation = default) where TEntity : class, ITableEntity, new();
        TEntity GetEntity<TEntity>(string tableName, string partitionKey, string rowKey, IEnumerable<string> properties = null) where TEntity : class, ITableEntity, new();
        Task<TEntity> GetEntityAsync<TEntity>(string tableName, string partitionKey, string rowKey, IEnumerable<string> properties = null, CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new();
        IEnumerable<TableItem> GetPageableTable(string filter, int? maxPerPage = 20);
        Task<IEnumerable<TableItem>> GetPageableTableAsync(string filter, int maxPerPage = 20, CancellationToken cancellationToken = default);
        Task<IDictionary<string, string>> InsertBatchAsync<TEntity>(string tableName, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new();
        Task<Dictionary<string, string>> MixedBatchAsync<TEntity>(string tableName, IEnumerable<Tuple<TableTransactionActionType, TEntity>> entitiesActions, CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new();
        TableItem? TryGetTable(string tableName);
        Task<TableItem?> TryGetTableAsync(string tableName, CancellationToken cancellationToken = default);
        Task UpdateBatchAsync<TEntity>(string tableName, IEnumerable<TEntity> entities, TableUpdateMode updateMode = TableUpdateMode.Replace, CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new();
        void UpdateEntity<TEntity>(string tableName, TEntity entityToUpdate, TableUpdateMode updateMode = TableUpdateMode.Replace) where TEntity : class, ITableEntity, new();
        Task UpdateEntityAsync<TEntity>(string tableName, TEntity entity, TableUpdateMode updateMode = TableUpdateMode.Replace, CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new();
        void UpsertEntity<TEntity>(string tableName, ITableEntity entity, TableUpdateMode updateMode = TableUpdateMode.Replace) where TEntity : class, ITableEntity, new();
        Task UpsertEntityAsync<TEntity>(string tableName, TEntity entity, TableUpdateMode updateMode = TableUpdateMode.Replace, CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new();
    }
}