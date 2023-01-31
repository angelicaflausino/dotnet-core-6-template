using Azure.Data.Tables;
using Company.Default.Cloud.Storage;
using Company.Default.Tests.DataFaker;

namespace Company.Default.Tests.Cloud
{
    public class TableStorageServiceTests
    {
        private readonly TableStorageService _service;
        private readonly string _tableName = "fooTest";
        public TableStorageServiceTests() 
        { 
            _service = GetTableStorageService();
        }

        [Fact]
        public void AddEntity_Void()
        {
            var foo = new FooEntity();
            
            _service.AddEntity<FooEntity>(_tableName, foo);
        }

        [Fact]
        public async Task AddEntityAsync_Void()
        {
            var foo = new FooEntity();

            await _service.AddEntityAsync(_tableName, foo);
        }

        [Fact]
        public void CreateTable_NotNull()
        {
            var tableName = "createTest";

            var result = _service.CreateTable(tableName);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateTableAsync_NotNull()
        {
            var tableName = "createTestAsync";
            
            var result = await _service.CreateTableAsync(tableName);
            
            Assert.NotNull(result);
        }

        [Fact]
        public async void DeleteBatchAsync_Void()
        {
            var entities = GenerateFooEntities(2);

            await _service.DeleteBatchAsync(_tableName, entities);
        }

        [Fact]
        public void DeleteEntity_Void()
        {
            var entities = GenerateFooEntities(1);
            var partitionKey = entities.First().PartitionKey;
            var rowKey = entities.First().RowKey;

            _service.DeleteEntity(_tableName, partitionKey, rowKey);
        }

        [Fact]
        public async void DeleteEntityAsync_Void()
        {
            var entities = GenerateFooEntities(1);
            var partitionKey = entities.First().PartitionKey;
            var rowKey = entities.First().RowKey;

            await _service.DeleteEntityAsync(_tableName, partitionKey, rowKey);
        }

        [Fact]
        public void DeleteTable_Void()
        {
            var tableName = "deletetest";
            _service.CreateTable(tableName);

            _service.DeleteTable(tableName);
        }

        [Fact]
        public async void DeleteTableAsync_Void()
        {
            var tableName = "deletetestasync";
            _service.CreateTable(tableName);

            await _service.DeleteTableAsync(tableName);
        }

        [Fact]
        public void GetEntities_NotEmpty()
        {
            var entities = GenerateFooEntities(3);
            var dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            var result = _service.GetEntities<FooEntity>(_tableName, x => x.Timestamp >= dt, 20);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async void GetEntitiesAsync_NotEmpty()
        {
            var entities = GenerateFooEntities(3);

            var result = await _service.GetEntitiesAsync<FooEntity>(_tableName, x => x.FooName != null, 20);

            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetEntity_NotNull_Equals()
        {
            var entities = GenerateFooEntities(1);
            var partitionKey = entities.First().PartitionKey;
            var rowKey = entities.First().RowKey;

            var result = _service.GetEntity<FooEntity>(_tableName, partitionKey, rowKey);
            
            Assert.NotNull(result);
            Assert.Equal(result.RowKey, rowKey);
            Assert.Equal(result.PartitionKey, partitionKey);
        }

        [Fact]
        public async void GetEntityAsync_NotNull_Equals()
        {
            var entities = GenerateFooEntities(1);
            var partitionKey = entities.First().PartitionKey;
            var rowKey = entities.First().RowKey;

            var result = await _service.GetEntityAsync<FooEntity>(_tableName, partitionKey, rowKey);

            Assert.NotNull(result);
            Assert.Equal(result.RowKey, rowKey);
            Assert.Equal(result.PartitionKey, partitionKey);
        }

        [Fact]
        public void GetPageableTable_NotEmpty_Equal()
        {
            var filter = $"TableName eq '{_tableName}'";
            var result = _service.GetPageableTable(filter, 20);

            Assert.NotEmpty(result);
            Assert.Equal(_tableName, result.First().Name);
        }

        [Fact]
        public async Task GetPageableTableAsync_NotEmpty_Equal()
        {
            var filter = $"TableName eq '{_tableName}'";
            var result = await _service.GetPageableTableAsync(filter, 20);

            Assert.NotEmpty(result);
            Assert.Equal(_tableName, result.First().Name);
        }

        [Fact]
        public async Task InsertBatchAsync_NotEmpty()
        {
            var entities = new List<FooEntity>()
            {
                new FooEntity(),
                new FooEntity(),
                new FooEntity(),
            };

            var result = await _service.InsertBatchAsync(_tableName, entities);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task MixedBatchAsync_NotEmpty()
        {
            var entities = GenerateFooEntities(3);

            var first = entities.First();
            var second = entities.ElementAt(1);
            var last = entities.Last();
            first.FooName = "Foo First";
            last.BarName = "Bar Last";

            List<Tuple<TableTransactionActionType, FooEntity>> actions = new()
            {
                new Tuple<TableTransactionActionType, FooEntity>(TableTransactionActionType.UpdateMerge, first),
                new Tuple<TableTransactionActionType, FooEntity>(TableTransactionActionType.Delete, second),
                new Tuple<TableTransactionActionType, FooEntity>(TableTransactionActionType.UpdateReplace, last)
            };

            var result = await _service.MixedBatchAsync(_tableName, actions);

            Assert.NotEmpty(result);
        }

        [Fact]
        public void TryGetTable_Null()
        {
            var tableName = "NotExists";

            var result = _service.TryGetTable(tableName);

            Assert.Null(result);
        }

        [Fact]
        public void TryGetTable_NotNull()
        {
            var result = _service.TryGetTable(_tableName); 
            
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TryGetTableAsync_NotNull()
        {
            var result = await _service.TryGetTableAsync(_tableName);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateBatchAsync_Void()
        {
            var entities = GenerateFooEntities(3);

            foreach(var entity in entities)
                entity.FooName += " Updated";

            await _service.UpdateBatchAsync<FooEntity>(_tableName, entities);
        }

        [Fact]
        public void UpdateEntity_Void()
        {
            var entity = GenerateFooEntities(1).First();
            entity.FooName += " Updated";
            entity.BarName += " Updated";

            _service.UpdateEntity<FooEntity>(_tableName, entity);
        }

        [Fact]
        public async Task UpdateEntityAsync_Void()
        {
            var entity = GenerateFooEntities(1).First();
            entity.FooName += " Async";
            entity.BarName += " Async";

            await _service.UpdateEntityAsync<FooEntity>(_tableName, entity);
        }

        [Fact]
        public void UpsertEntity_Void()
        {
            var foo = new FooEntity();
            foo.FooName = "foo";
            foo.BarName= "bar";

            _service.UpsertEntity<FooEntity>(_tableName, foo, TableUpdateMode.Merge);
        }

        [Fact]
        public async Task UpsertEntityAsync_Void()
        {
            var foo = GenerateFooEntities(1).First();
            foo.FooName = "foo";
            foo.BarName = "bar";

            await _service.UpsertEntityAsync<FooEntity>(_tableName, foo, TableUpdateMode.Replace);
        }

        #region Privates
        private TableStorageService GetTableStorageService()
        {
            var config = TestUtils.GetConfiguration();
            string connectionsString = config.GetSection("Storage:ConnectionString").Value;
            TableServiceClient tableServiceClient = new TableServiceClient(connectionsString);

            TableStorageService service = new TableStorageService(tableServiceClient);
            service.CreateTable(_tableName);

            return service;
        }

        private IEnumerable<FooEntity> GenerateFooEntities(int quantity)
        {
            List<FooEntity> entities = new List<FooEntity>();

            for(int i = 0; i < quantity; i++)
            {
                var foo = new FooEntity();
                foo.FooName = $"Foo_{foo.RowKey}";
                foo.BarName = $"Bar_{foo.RowKey}";

                _service.AddEntity(_tableName, foo);

                entities.Add(foo);
            }

            return entities;
        }

        #endregion
    }
}
