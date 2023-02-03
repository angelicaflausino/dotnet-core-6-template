using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Company.Default.Cloud.Interfaces;

namespace Company.Default.Cloud.Storage
{
    public class QueueStorageService : IQueueStorageService
    {
        private readonly QueueClient _queueClient;

        public QueueStorageService(QueueClient queueClient)
        {
            _queueClient = queueClient;

            CreateIfNotExists();
        }

        #region Queue Operations
        public void CreateIfNotExists(Dictionary<string, string>? metadata = null) => _queueClient.CreateIfNotExists(metadata);

        public async Task CreateIfNotExistsAsync(Dictionary<string, string>? metadata, CancellationToken cancellationToken = default) => await _queueClient.CreateIfNotExistsAsync(metadata, cancellationToken);

        public bool Exists() => _queueClient.Exists();

        public async Task<bool> ExistsAsync(CancellationToken cancellationToken = default) => await _queueClient.ExistsAsync(cancellationToken);

        public bool DeleteIfExists() => _queueClient.DeleteIfExists();

        public async Task<bool> DeleteIfExistAsync(CancellationToken cancellationToken = default) => await _queueClient.DeleteIfExistsAsync(cancellationToken);

        public QueueProperties GetQueueProperties() => _queueClient.GetProperties();

        public async Task<QueueProperties> GetQueuePropertiesAsync(CancellationToken cancellationToken = default) => await _queueClient.GetPropertiesAsync(cancellationToken);

        public void SetQueueMetadata(Dictionary<string, string> metadata) => _queueClient.SetMetadata(metadata);

        public async Task SetQueueMetadataAsync(Dictionary<string, string> metadata, CancellationToken cancellationToken = default) => await _queueClient.SetMetadataAsync(metadata, cancellationToken);
        #endregion

        #region Clear Message
        public void ClearMessages() => _queueClient.ClearMessages();

        public async Task ClearMessagesAsync(CancellationToken cancellationToken = default) => await _queueClient.ClearMessagesAsync(cancellationToken);
        #endregion

        #region Send Message
        public SendReceipt SendMessage(string message, TimeSpan? visibilityTimeout = default, TimeSpan? timeToLive = default) =>
            _queueClient.SendMessage(message, visibilityTimeout, timeToLive);

        public async Task<SendReceipt> SendMessageAsync(string message, TimeSpan? visibilityTimeout = default, TimeSpan? timeToLive = default, CancellationToken cancellationToken = default) =>
           await _queueClient.SendMessageAsync(message, visibilityTimeout, timeToLive, cancellationToken);

        public SendReceipt SendMessageAsBinaryData(BinaryData message, TimeSpan? visibilityTimeout = default, TimeSpan? timeToLive = default) =>
            _queueClient.SendMessage(message, visibilityTimeout, timeToLive);

        public async Task<SendReceipt> SendMessageAsBinaryDataAsync(BinaryData message, TimeSpan? visibilityTimeout = default, TimeSpan? timeToLive = default, CancellationToken cancellationToken = default) =>
            await _queueClient.SendMessageAsync(message, visibilityTimeout, timeToLive, cancellationToken);
        #endregion

        #region Receive Messages
        public QueueMessage ReceiveMessage(TimeSpan? visibilityTimeout = default) => _queueClient.ReceiveMessage(visibilityTimeout);

        public async Task<QueueMessage> ReceiveMessageAsync(TimeSpan? visibilityTimeout = default, CancellationToken cancellationToken = default) =>
            await _queueClient.ReceiveMessageAsync(visibilityTimeout, cancellationToken);

        public IEnumerable<QueueMessage> ReceiveMessages(int? maxMessages = default, TimeSpan? visibilityTimeOut = default)
        {
            QueueMessage[] messages = _queueClient.ReceiveMessages(maxMessages, visibilityTimeOut);

            return messages;
        }

        public async Task<IEnumerable<QueueMessage>> ReceiveMessagesAsync(int? maxMessages = default, TimeSpan? visibilityTimeOut = default, CancellationToken cancellationToken = default)
        {
            QueueMessage[] messages = await _queueClient.ReceiveMessagesAsync(maxMessages, visibilityTimeOut, cancellationToken);

            return messages;
        }
        #endregion

        #region Peek Messages
        public PeekedMessage PeekMessage() => _queueClient.PeekMessage();

        public async Task<PeekedMessage> PeekMessageAsync(CancellationToken cancellationToken = default) =>
            await _queueClient.PeekMessageAsync(cancellationToken);

        public IEnumerable<PeekedMessage> PeekMessages(int? maxMessages = default)
        {
            PeekedMessage[] peekedMessages = _queueClient.PeekMessages(maxMessages);

            return peekedMessages;
        }

        public async Task<IEnumerable<PeekedMessage>> PeekMessagesAsync(int? maxMessages, CancellationToken cancellationToken = default)
        {
            PeekedMessage[] peekedMessages = await _queueClient.PeekMessagesAsync(maxMessages, cancellationToken);

            return peekedMessages;
        }
        #endregion

        #region Delete Message
        public void DeleteMessage(string messageId, string popReceipt) => _queueClient.DeleteMessage(messageId, popReceipt);

        public async Task DeleteMessageAsync(string messageId, string popReceipt, CancellationToken cancellationToken = default) =>
            await _queueClient.DeleteMessageAsync(messageId, popReceipt, cancellationToken);
        #endregion

        #region Update Message
        public UpdateReceipt UpdateMessage(string messageId, string popReceipt, string message, TimeSpan visibilityTimeout = default) =>
            _queueClient.UpdateMessage(messageId, popReceipt, message, visibilityTimeout);

        public async Task<UpdateReceipt> UpdateMessageAsync(string messageId, string popReceipt, string messsage, TimeSpan visibilityTimeout = default,
            CancellationToken cancellationToken = default)
            => await _queueClient.UpdateMessageAsync(messageId, popReceipt, messsage, visibilityTimeout, cancellationToken);

        public UpdateReceipt UpdateMessageAsBinaryData(string messageId, string popReceipt, BinaryData binaryData, TimeSpan visibilityTimeout = default) =>
            _queueClient.UpdateMessage(messageId, popReceipt, binaryData, visibilityTimeout);

        public async Task<UpdateReceipt> UpdateMessageAsBinaryDataAsync(string messageId, string popReceipt, BinaryData binaryData, TimeSpan visibilityTimeout = default, CancellationToken cancellationToken = default) =>
           await _queueClient.UpdateMessageAsync(messageId, popReceipt, binaryData, visibilityTimeout, cancellationToken);
        #endregion

    }
}
