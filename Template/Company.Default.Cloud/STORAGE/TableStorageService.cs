using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using $safeprojectname$.Interfaces;
using System.Linq.Expressions;

namespace $safeprojectname$.Storage
{
    public class TableStorageService : ITableStorageService
    {
        private readonly TableServiceClient _tableServiceClient;
        private const string FILTER_BY_TABLE_NAME = "TableName eq '{0}'";

        public TableStorageService(TableServiceClient tableServiceClient)
        {
            _tableServiceClient = tableServiceClient;
        }

        #region Tables
        public TableItem CreateTable(string tableName)
        {
            TableItem tableItem = _tableServiceClient.CreateTableIfNotExists(tableName);

            return tableItem;
        }

        public async Task<TableItem> CreateTableAsync(string tableName, CancellationToken cancellationToken = default)
        {
            TableItem tableItem = await _tableServiceClient.CreateTableIfNotExistsAsync(tableName, cancellationToken);

            return tableItem;
        }

        public IEnumerable<TableItem> GetPageableTable(string filter, int? maxPerPage = 20)
        {
            IEnumerable<TableItem> tableItems = FilterTable(filter, maxPerPage);

            return tableItems;
        }

        public async Task<IEnumerable<TableItem>> GetPageableTableAsync(string filter, int maxPerPage = 20, CancellationToken cancellationToken = default)
        {
            IEnumerable<TableItem> tableItems = await FilterTableAsync(filter, maxPerPage, cancellationToken);

            return tableItems;
        }

        public TableItem? TryGetTable(string tableName)
        {
            string filter = string.Format(FILTER_BY_TABLE_NAME, tableName);
            IEnumerable<TableItem> tableItem = FilterTable(filter);

            return tableItem.FirstOrDefault(x => x.Name == tableName);
        }

        public async Task<TableItem?> TryGetTableAsync(string tableName, CancellationToken cancellationToken = default)
        {
            string filter = string.Format(FILTER_BY_TABLE_NAME, tableName);
            IEnumerable<TableItem> tables = await FilterTableAsync(filter, null, cancellationToken);

            return tables.FirstOrDefault(x => x.Name == tableName);
        }

        public void DeleteTable(string tableName) => _tableServiceClient.DeleteTable(tableName);

        public async Task DeleteTableAsync(string tableName, CancellationToken cancellationToken = default) =>
            await _tableServiceClient.DeleteTableAsync(tableName, cancellationToken);
        #endregion

        #region Entities
        public void AddEntity<TEntity>(string tableName, TEntity entity) where TEntity : class, ITableEntity, new()
        {
            var table = _tableServiceClient.GetTableClient(tableName);

            table.AddEntity(entity);
        }

        public TEntity GetEntity<TEntity>(string tableName, string partitionKey, string rowKey, IEnumerable<string> properties = null )
            where TEntity : class, ITableEntity, new()
        {
            var table = _tableServiceClient.GetTableClient(tableName);

            var response = table.GetEntity<TEntity>(partitionKey, rowKey, properties);

            return response.Value;
        }

        public async Task<TEntity> GetEntityAsync<TEntity>(string tableName, 
            string partitionKey, 
            string rowKey, 
            IEnumerable<string> properties = null,
            CancellationToken cancellationToken = default)
            where TEntity : class, ITableEntity, new()
        {
            var table = _tableServiceClient.GetTableClient(tableName);

            var response = await table.GetEntityAsync<TEntity>(partitionKey, rowKey, properties);

            return response.Value;
        }

        public async Task AddEntityAsync<TEntity>(string tableName, TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class, ITableEntity, new()
        {
            var table = _tableServiceClient.GetTableClient(tableName);

            await table.AddEntityAsync(entity, cancellationToken);
        }

        public void UpdateEntity<TEntity>(string tableName,
            TEntity entityToUpdate,
            TableUpdateMode updateMode = TableUpdateMode.Replace) where TEntity : class, ITableEntity, new()
        {
            var table = GetTableClient(tableName);

            table.UpdateEntity(entityToUpdate, ETag.All, updateMode);
        }

        public async Task UpdateEntityAsync<TEntity>(string tableName,
            TEntity entity,
            TableUpdateMode updateMode = TableUpdateMode.Replace,
            CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new()
        {
            var table = GetTableClient(tableName);

            await table.UpdateEntityAsync(entity, ETag.All, updateMode, cancellationToken);
        }

        public void UpsertEntity<TEntity>(string tableName,
            ITableEntity entity,
            TableUpdateMode updateMode = TableUpdateMode.Replace) where TEntity : class, ITableEntity, new()
        {
            var table = GetTableClient(tableName);

            table.UpsertEntity(entity, updateMode);
        }

        public async Task UpsertEntityAsync<TEntity>(string tableName,
            TEntity entity,
            TableUpdateMode updateMode = TableUpdateMode.Replace,
            CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new()
        {
            var table = GetTableClient(tableName);

            await table.UpsertEntityAsync(entity, updateMode, cancellationToken);
        }

        public void DeleteEntity(string tableName, string partitionKey, string rowKey)
        {
            var table = GetTableClient(tableName);

            table.DeleteEntity(partitionKey, rowKey, ETag.All);
        }

        public async Task DeleteEntityAsync(string tableName, string partitionKey, string rowKey, CancellationToken cancellationToken = default)
        {
            var table = GetTableClient(tableName);

            await table.DeleteEntityAsync(partitionKey, rowKey, ETag.All, cancellationToken);
        }

        public IEnumerable<TEntity> GetEntities<TEntity>(string tableName,
            Expression<Func<TEntity, bool>> filter,
            int? maxPerPage = default,
            IEnumerable<string> properties = null) where TEntity : class, ITableEntity, new()
        {
            var entities = FilterEntity(tableName, filter, maxPerPage, properties);

            return entities;
        }

        public async Task<IEnumerable<TEntity>> GetEntitiesAsync<TEntity>(string tableName,
            Expression<Func<TEntity, bool>> filter,
            int? maxPerPage = default,
            IEnumerable<string> properties = null,
            CancellationToken cancellation = default) where TEntity : class, ITableEntity, new()
        {
            var entities = await FilterEntityAsync(tableName, filter, maxPerPage, properties, cancellation);

            return entities;
        }
        #endregion

        #region Batch Tables
        public async Task<IDictionary<string, string>> InsertBatchAsync<TEntity>(string tableName, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            where TEntity : class, ITableEntity, new()
        {
            var table = GetTableClient(tableName);
            List<TableTransactionAction> actions = new List<TableTransactionAction>();
            actions.AddRange(entities.Select(entity => new TableTransactionAction(TableTransactionActionType.Add, entity)));

            Response<IReadOnlyList<Response>> responses = await table.SubmitTransactionAsync(actions, cancellationToken);
            Dictionary<string, string> result = new Dictionary<string, string>();

            for (int i = 0; i < entities.Count(); i++)
            {
                var rowKey = entities.ElementAt(i).RowKey;
                var eTag = responses.Value.ElementAt(i).Headers.ETag.ToString() ?? string.Empty;

                result.Add(rowKey, eTag);
            }

            return result;
        }

        public async Task DeleteBatchAsync<TEntity>(string tableName,
            IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new()
        {
            var table = GetTableClient(tableName);
            List<TableTransactionAction> actions = new List<TableTransactionAction>();

            actions.AddRange(entities.Select(entity => new TableTransactionAction(TableTransactionActionType.Delete, entity)));

            await table.SubmitTransactionAsync(actions, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateBatchAsync<TEntity>(string tableName,
            IEnumerable<TEntity> entities,
            TableUpdateMode updateMode = TableUpdateMode.Replace,
            CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new()
        {
            var table = GetTableClient(tableName);
            List<TableTransactionAction> actions = new List<TableTransactionAction>();
            var transactionType = updateMode == TableUpdateMode.Replace
                ? TableTransactionActionType.UpdateReplace
                : TableTransactionActionType.UpdateMerge;

            actions.AddRange(entities.Select(entity => new TableTransactionAction(transactionType, entity)));

            await table.SubmitTransactionAsync(actions, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Dictionary<string, string>> MixedBatchAsync<TEntity>(string tableName,
            IEnumerable<Tuple<TableTransactionActionType, TEntity>> entitiesActions,
            CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new()
        {
            var table = GetTableClient(tableName);
            List<TableTransactionAction> actions = new List<TableTransactionAction>();

            actions.AddRange(entitiesActions.Select(tuple => new TableTransactionAction(tuple.Item1, tuple.Item2)));

            Response<IReadOnlyList<Response>> responses = await table.SubmitTransactionAsync(actions, cancellationToken);
            Dictionary<string, string> result = new Dictionary<string, string>();

            for (int i = 0; i < entitiesActions.Count(); i++)
            {
                var entity = entitiesActions.ElementAt(i).Item2;
                var rowKey = entity.RowKey;
                var eTag = responses.Value.ElementAt(i).Headers.ETag.ToString() ?? string.Empty;

                result.Add(rowKey, eTag);
            }

            return result;
        }
        #endregion

        #region privates
        private IEnumerable<TableItem> FilterTable(string filter, int? maxPerPage = default)
        {
            Pageable<TableItem> pageable = _tableServiceClient.Query(filter, maxPerPage);

            return pageable.AsEnumerable();
        }

        private async Task<IEnumerable<TableItem>> FilterTableAsync(string filter, int? maxPerPage = default, CancellationToken cancellationToken = default)
        {
            AsyncPageable<TableItem> asyncPageable = _tableServiceClient.QueryAsync(filter, maxPerPage, cancellationToken);
            List<TableItem> tableItems = new List<TableItem>();

            await foreach (TableItem tableItem in asyncPageable)
                tableItems.Add(tableItem);

            return tableItems;
        }

        private TableClient GetTableClient(string tableName)
        {
            TableClient tableClient = _tableServiceClient.GetTableClient(tableName);

            return tableClient;
        }

        private IEnumerable<TEntity> FilterEntity<TEntity>(string tableName,
            Expression<Func<TEntity, bool>> filter,
            int? maxPerPage = default,
            IEnumerable<string> properties = null) where TEntity : class, ITableEntity, new()
        {
            TableClient table = GetTableClient(tableName);
            Pageable<TEntity> pageable = table.Query<TEntity>(filter, maxPerPage, properties);

            return pageable.AsEnumerable();
        }

        private async Task<IEnumerable<TEntity>> FilterEntityAsync<TEntity>(string tableName,
            Expression<Func<TEntity, bool>> filter,
            int? maxPerPage = default,
            IEnumerable<string> properties = null,
            CancellationToken cancellationToken = default) where TEntity : class, ITableEntity, new()
        {
            TableClient table = GetTableClient(tableName);
            List<TEntity> entities = new List<TEntity>();
            AsyncPageable<TEntity> pageable = table.QueryAsync<TEntity>(filter, maxPerPage, properties, cancellationToken);

            await foreach (var page in pageable)
                entities.Add(page);

            return entities;
        }
        #endregion
    }
}
