using Azure.Storage.Queues.Models;
using Microsoft.Graph.TermStore;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace Company.Default.Cloud.Interfaces
{
    public interface IQueueStorageService
    {
        /// <summary>
        /// Delete all messages from queue
        /// </summary>
        void ClearMessages();

        /// <summary>
        /// Asynchronously Delete all messages from queue
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task"/>
        /// </returns>
        Task ClearMessagesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Create queue if not exists
        /// </summary>
        /// <param name="metadata"></param>
        void CreateIfNotExists(Dictionary<string, string>? metadata = null);

        /// <summary>
        /// Asynchronously Create queue if not exists
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task"/>
        /// </returns>
        Task CreateIfNotExistsAsync(Dictionary<string, string>? metadata, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously Delete Message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="popReceipt"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task"/>
        /// </returns>
        Task DeleteMessageAsync(string messageId, string popReceipt, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously Delete Queue if exists
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{T}"/> where T is <see cref="bool"/>
        /// </returns>
        Task<bool> DeleteIfExistAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete Queue if exists
        /// </summary>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="bool"/>
        /// </returns>
        bool DeleteIfExists();

        /// <summary>
        /// Delete Message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="popReceipt"></param>
        void DeleteMessage(string messageId, string popReceipt);

        /// <summary>
        /// Check if Queue exists on Storage Account
        /// </summary>
        /// <returns>
        /// <see cref="bool"/>
        /// </returns>
        bool Exists();

        /// <summary>
        /// Asynchronously check if Queue exists on Storage Account
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="bool"/>
        /// </returns>
        Task<bool> ExistsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get Queue's properties
        /// </summary>
        /// <returns>
        /// <see cref="QueueProperties"/>
        /// </returns>
        QueueProperties GetQueueProperties();

        /// <summary>
        /// Asynchronously Get Queue's properties
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="QueueProperties"/>
        /// </returns>
        Task<QueueProperties> GetQueuePropertiesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves one message from the front of the queue but does not alter the visibility of the message.
        /// <para>
        /// <see href="https://docs.microsoft.com/rest/api/storageservices/peek-messages">Peek Messages</see>
        /// </para>
        /// </summary>
        /// <returns>
        /// <see cref="PeekedMessage"/>
        /// </returns>
        PeekedMessage PeekMessage();

        /// <summary>
        /// Asynchronously retrieves one message from the front of the queue but does not alter the visibility of the message.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="PeekedMessage"/>
        /// </returns>
        Task<PeekedMessage> PeekMessageAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves one or more messages from the front of the queue but does not alter the visibility of the message.
        /// </summary>
        /// <param name="maxMessages"></param>
        /// <returns>
        /// <see cref="IEnumerable{T}"/> where T is a <see cref="PeekedMessage"/>
        /// </returns>
        IEnumerable<PeekedMessage> PeekMessages(int? maxMessages = null);

        /// <summary>
        /// Asynchronously retrieves one or more messages from the front of the queue but does not alter the visibility of the message.
        /// </summary>
        /// <param name="maxMessages"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="IEnumerable{T}"/> and T is a <see cref="PeekedMessage"/>
        /// </returns>
        Task<IEnumerable<PeekedMessage>> PeekMessagesAsync(int? maxMessages, CancellationToken cancellationToken = default);

        /// <summary>
        /// Receives one message from the front of the queue.
        /// <para>
        /// <see href="https://docs.microsoft.com/rest/api/storageservices/get-messages">Get Messages</see>
        /// </para>
        /// </summary>
        /// <param name="visibilityTimeout"></param>
        /// <returns>
        /// <see cref="QueueMessage"/>
        /// </returns>
        QueueMessage ReceiveMessage(TimeSpan? visibilityTimeout = null);

        /// <summary>
        /// Asynchronously receives one message from the front of the queue.
        /// <para>
        /// <see href="https://docs.microsoft.com/rest/api/storageservices/get-messages">Get Messages</see>
        /// </para>
        /// </summary>
        /// <param name="visibilityTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="QueueMessage"/>
        /// </returns>
        Task<QueueMessage> ReceiveMessageAsync(TimeSpan? visibilityTimeout = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves one or more messages from the front of the queue.
        /// <para>
        /// For more information, see
        /// <see href="https://docs.microsoft.com/rest/api/storageservices/get-messages">
        /// Get Messages</see>.
        /// </para>
        /// </summary>
        /// <param name="maxMessages"></param>
        /// <param name="visibilityTimeOut"></param>
        /// <returns>
        /// <see cref="IEnumerable{T}"/> where T is a <see cref="QueueMessage"/>
        /// </returns>
        IEnumerable<QueueMessage> ReceiveMessages(int? maxMessages = null, TimeSpan? visibilityTimeOut = null);

        /// <summary>
        /// Asynchronously retrieves one or more messages from the front of the queue.
        /// <para>
        /// <see href="https://docs.microsoft.com/rest/api/storageservices/get-messages">Get Messages</see>
        /// </para>
        /// </summary>
        /// <param name="maxMessages"></param>
        /// <param name="visibilityTimeOut"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="IEnumerable{T}"/> and T is a <see cref="QueueMessage"/>
        /// </returns>
        Task<IEnumerable<QueueMessage>> ReceiveMessagesAsync(int? maxMessages = null, TimeSpan? visibilityTimeOut = null, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Send a string message to queue
        /// </summary>
        /// <param name="message"></param>
        /// <param name="visibilityTimeout"></param>
        /// <param name="timeToLive"></param>
        /// <returns>
        /// <see cref="SendReceipt"/>
        /// </returns>
        SendReceipt SendMessage(string message, TimeSpan? visibilityTimeout = null, TimeSpan? timeToLive = null);

        /// <summary>
        /// Send a Binary Data message to queue
        /// </summary>
        /// <param name="message"></param>
        /// <param name="visibilityTimeout"></param>
        /// <param name="timeToLive"></param>
        /// <returns>
        /// <see cref="SendReceipt"/>
        /// </returns>
        SendReceipt SendMessageAsBinaryData(BinaryData message, TimeSpan? visibilityTimeout = null, TimeSpan? timeToLive = null);

        /// <summary>
        /// Asynchronously send a Binary Data message to queue
        /// </summary>
        /// <param name="message"></param>
        /// <param name="visibilityTimeout"></param>
        /// <param name="timeToLive"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="SendReceipt"/>
        /// </returns>
        Task<SendReceipt> SendMessageAsBinaryDataAsync(BinaryData message, TimeSpan? visibilityTimeout = null, TimeSpan? timeToLive = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously send a string message to queue
        /// </summary>
        /// <param name="message"></param>
        /// <param name="visibilityTimeout"></param>
        /// <param name="timeToLive"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="SendReceipt"/>
        /// </returns>
        Task<SendReceipt> SendMessageAsync(string message, TimeSpan? visibilityTimeout = null, TimeSpan? timeToLive = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets user-defined metadata on the specified queue.Metadata is associated with the queue as name-value pairs.
        /// <para>
        /// For more information, see
        /// <see href="https://docs.microsoft.com/rest/api/storageservices/set-queue-metadata">
        /// Set Queue Metadata</see>.
        /// </para> 
        /// </summary>
        /// <param name="metadata"></param>
        void SetQueueMetadata(Dictionary<string, string> metadata);

        /// <summary>
        /// Asynchronously sets user-defined metadata on the specified queue.Metadata is associated with the queue as name-value pairs.
        /// <para>
        /// For more information, see
        /// <see href="https://docs.microsoft.com/rest/api/storageservices/set-queue-metadata">
        /// Set Queue Metadata</see>.
        /// </para> 
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SetQueueMetadataAsync(Dictionary<string, string> metadata, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update Queue message as string
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="popReceipt"></param>
        /// <param name="message"></param>
        /// <param name="visibilityTimeout"></param>
        /// <returns>
        /// <see cref="UpdateReceipt"/>
        /// </returns>
        UpdateReceipt UpdateMessage(string messageId, string popReceipt, string message, TimeSpan visibilityTimeout = default);

        /// <summary>
        /// Update Queue message as Binary Data
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="popReceipt"></param>
        /// <param name="binaryData"></param>
        /// <param name="visibilityTimeout"></param>
        /// <returns>
        /// <see cref="UpdateReceipt"/>
        /// </returns>
        UpdateReceipt UpdateMessageAsBinaryData(string messageId, string popReceipt, BinaryData binaryData, TimeSpan visibilityTimeout = default);

        /// <summary>
        /// Asynchronously update Queue message as Binary Data
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="popReceipt"></param>
        /// <param name="binaryData"></param>
        /// <param name="visibilityTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="UpdateReceipt"/>
        /// </returns>
        Task<UpdateReceipt> UpdateMessageAsBinaryDataAsync(string messageId, string popReceipt, BinaryData binaryData, TimeSpan visibilityTimeout = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously update Queue message as string
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="popReceipt"></param>
        /// <param name="messsage"></param>
        /// <param name="visibilityTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="UpdateReceipt"/>
        /// </returns>
        Task<UpdateReceipt> UpdateMessageAsync(string messageId, string popReceipt, string messsage, TimeSpan visibilityTimeout = default, CancellationToken cancellationToken = default);
    }
}