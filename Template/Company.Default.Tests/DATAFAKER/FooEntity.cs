using Azure;
using Azure.Data.Tables;

namespace $safeprojectname$.DataFaker
{
    public class FooEntity : ITableEntity
    {
        public FooEntity() 
        {
            var partitionKey = Guid.NewGuid().ToString();
            var rowKey = Guid.NewGuid().ToString();
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }  

        public FooEntity(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string FooName { get; set; }
        public string BarName { get; set; }
    }
}
