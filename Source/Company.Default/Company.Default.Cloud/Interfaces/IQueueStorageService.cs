using Azure.Storage.Queues.Models;
using System.Threading;

namespace Company.Default.Cloud.Interfaces
{
    public interface IQueueStorageService
    {
        void ClearMessages();
        Task ClearMessagesAsync(CancellationToken cancellationToken = default);
        void CreateIfNotExists(Dictionary<string, string>? metadata = null);
        Task CreateIfNotExistsAsync(Dictionary<string, string>? metadata, CancellationToken cancellationToken);
        Task DeleteMessageAsync(string messageId, string popReceipt, CancellationToken cancellationToken = default);
        Task<bool> DeleteIfExistAsync(CancellationToken cancellationToken = default);
        bool DeleteIfExists();
        void DeleteMessage(string messageId, string popReceipt);
        bool Exists();
        Task<bool> ExistsAsync(CancellationToken cancellationToken = default);
        QueueProperties GetQueueProperties();
        Task<QueueProperties> GetQueuePropertiesAsync(CancellationToken cancellationToken = default);
        PeekedMessage PeekMessage();
        Task<PeekedMessage> PeekMessageAsync(CancellationToken cancellationToken = default);
        IEnumerable<PeekedMessage> PeekMessages(int? maxMessages = null);
        Task<IEnumerable<PeekedMessage>> PeekMessagesAsync(int? maxMessages, CancellationToken cancellationToken = default);
        QueueMessage ReceiveMessage(TimeSpan? visibilityTimeout = null);
        Task<QueueMessage> ReceiveMessageAsync(TimeSpan? visibilityTimeout = null, CancellationToken cancellationToken = default);
        IEnumerable<QueueMessage> ReceiveMessages(int? maxMessages = null, TimeSpan? visibilityTimeOut = null);
        Task<IEnumerable<QueueMessage>> ReceiveMessagesAsync(int? maxMessages = null, TimeSpan? visibilityTimeOut = null, CancellationToken cancellationToken = default);
        SendReceipt SendMessage(string message, TimeSpan? visibilityTimeout = null, TimeSpan? timeToLive = null);
        SendReceipt SendMessageAsBinaryData(BinaryData message, TimeSpan? visibilityTimeout = null, TimeSpan? timeToLive = null);
        Task<SendReceipt> SendMessageAsBinaryDataAsync(BinaryData message, TimeSpan? visibilityTimeout = null, TimeSpan? timeToLive = null, CancellationToken cancellationToken = default);
        Task<SendReceipt> SendMessageAsync(string message, TimeSpan? visibilityTimeout = null, TimeSpan? timeToLive = null, CancellationToken cancellationToken = default);
        void SetQueueMetadata(Dictionary<string, string> metadata);
        Task SetQueueMetadataAsync(Dictionary<string, string> metadata, CancellationToken cancellationToken = default);
        UpdateReceipt UpdateMessage(string messageId, string popReceipt, string message, TimeSpan visibilityTimeout = default);
        UpdateReceipt UpdateMessageAsBinaryData(string messageId, string popReceipt, BinaryData binaryData, TimeSpan visibilityTimeout = default);
        Task<UpdateReceipt> UpdateMessageAsBinaryDataAsync(string messageId, string popReceipt, BinaryData binaryData, TimeSpan visibilityTimeout = default, CancellationToken cancellationToken = default);
        Task<UpdateReceipt> UpdateMessageAsync(string messageId, string popReceipt, string messsage, TimeSpan visibilityTimeout = default, CancellationToken cancellationToken = default);
    }
}