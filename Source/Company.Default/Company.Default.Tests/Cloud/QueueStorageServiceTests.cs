using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Company.Default.Cloud.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Default.Tests.Cloud
{
    public class QueueStorageServiceTests
    {
        private readonly QueueStorageService _service;

        public QueueStorageServiceTests()
        {
            _service = GetQueueStorageService();
            GetSendReceipt();
        }

        [Fact]
        public void CreateIfNoExists_Void()
        {
            var metadata = GenerateMetadata();

            _service.CreateIfNotExists(metadata);
        }

        [Fact]
        public async Task CreateIfNotExistsAsync_Void()
        {
            var metadata = GenerateMetadata();

            await _service.CreateIfNotExistsAsync(metadata);
        }

        [Fact]
        public void ClearMessages_Void()
        {
            _service.ClearMessages();
        }

        [Fact]
        public async Task ClearMessagesAsync_Void()
        {
            await _service.ClearMessagesAsync();
        }

        [Fact]
        public void Exists_True()
        {
            var result = _service.Exists();

            Assert.True(result);
        }

        [Fact]
        public async Task ExistsAsync_True()
        {
            var result = await _service.ExistsAsync(); 
            
            Assert.True(result);
        }

        [Fact]
        public void GetQueueProperties_NotNull()
        {
            var result = _service.GetQueueProperties();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetQueuePropertiesAsync_NotNull()
        {
            var result = await _service.GetQueuePropertiesAsync();

            Assert.NotNull(result);
        }

        [Fact]
        public void SetQueueMetadata_Void()
        {
            var metadata = GenerateMetadata();

            _service.SetQueueMetadata(metadata);
        }

        [Fact]
        public async Task SetQueueMetadataAsync_Void()
        {
            var metadata = GenerateMetadata();

            await _service.SetQueueMetadataAsync(metadata);
        }

        [Fact]
        public void SendMessage_NotNull()
        {
            var message = GenerateMessage();

            var result = _service.SendMessage(message);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task SendMessageAsync_NotNull()
        {
            var message = GenerateMessage();

            var result = await _service.SendMessageAsync(message);

            Assert.NotNull(result);
        }

        [Fact]
        public void SendMessageAsBinaryData_NotNull()
        {
            var binaryData = GenerateBinaryData();

            var result = _service.SendMessageAsBinaryData(binaryData);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task SendMessageAsBinaryDataAsync_NotNull()
        {
            var binaryData = GenerateBinaryData();

            var result = await _service.SendMessageAsBinaryDataAsync(binaryData);

            Assert.NotNull(result);
        }

        [Fact]
        public void ReceiveMessage_NotNull()
        {
            var result = _service.ReceiveMessage();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ReceiveMessageAsync_NotNull()
        {
            var result = await _service.ReceiveMessageAsync();

            Assert.NotNull(result);
        }

        [Fact]
        public void ReceiveMessages_NotEmpty()
        {
            int maxMessages = 20;

            var result = _service.ReceiveMessages(maxMessages); 
            
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task ReceiveMessagesAsync_NotEmpty()
        {
            int maxMessages = 20;

            var result = await _service.ReceiveMessagesAsync(maxMessages);

            Assert.NotEmpty(result);
        }

        [Fact]
        public void PeekMessage_NotNull()
        {
            var result = _service.PeekMessage();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task PeekMessageAsync_NotNull()
        {
            var message = GenerateMessage();
            _service.SendMessage(message);
            var result = await _service.PeekMessageAsync();

            Assert.NotNull(result);
        }

        [Fact]
        public void PeekMessages_NotEmpty()
        {
            int maxMessages = 20;

            var result = _service.PeekMessages(maxMessages);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task PeekMessagesAsync_NotEmpty()
        {
            int maxMessages = 20;

            var result = await _service.PeekMessagesAsync(maxMessages);

            Assert.NotEmpty(result);
        }

        [Fact]
        public void DeleteMessage_Void()
        {
            var sendMessage = _service.SendMessage("test");

            _service.DeleteMessage(sendMessage.MessageId, sendMessage.PopReceipt);
        }

        [Fact]
        public async Task DeleteMessageAsync_Void()
        {
            var sendMessage = _service.SendMessage("test async");

            await _service.DeleteMessageAsync(sendMessage.MessageId, sendMessage.PopReceipt);
        }

        [Fact]
        public void UpdateMessage_NotNull()
        {
            var sendReceipt = GetSendReceipt();

            var result = _service.UpdateMessage(sendReceipt.MessageId, sendReceipt.PopReceipt, "update test updated");

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateMessageAsync_NotNull()
        {
            var sendMessage = _service.SendMessage("update test async");

            var result = await _service.UpdateMessageAsync(sendMessage.MessageId, sendMessage.PopReceipt, "update test async updated");

            Assert.NotNull(result);
        }

        [Fact]
        public void UpdateMessageAsBinaryData_NotNull()
        {
            var binaryData = GenerateBinaryData();
            var sendReceipt = _service.SendMessageAsBinaryData(binaryData);
            var binaryToUpdate = GenerateBinaryData();

            var result = _service.UpdateMessageAsBinaryData(sendReceipt.MessageId, sendReceipt.PopReceipt, binaryToUpdate);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateMessageAsBinaryDataAsync_NotNull()
        {
            var binaryData = GenerateBinaryData();
            var sendReceipt = _service.SendMessageAsBinaryData(binaryData);
            var binaryToUpdate = GenerateBinaryData();

            var result = await _service.UpdateMessageAsBinaryDataAsync(sendReceipt.MessageId, sendReceipt.PopReceipt, binaryToUpdate);

            Assert.NotNull(result);
        }

        #region Privates
        private QueueStorageService GetQueueStorageService()
        {
            var config = TestUtils.GetConfiguration();

            string connectionString = config.GetSection("Storage:ConnectionString").Value;

            QueueClient queueClient = new QueueClient(connectionString, "queuetest");

            return new QueueStorageService(queueClient);
        }

        private Dictionary<string, string> GenerateMetadata() => 
            new Dictionary<string, string>()
            {
                { $"test", Guid.NewGuid().ToString() }
            };

        private string GenerateMessage() => $"Test {Guid.NewGuid}";

        private BinaryData GenerateBinaryData()
        {
            object data = new
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now.ToString(),
                CreatedBy = Guid.NewGuid().ToString(),
            };

            var json = JsonConvert.SerializeObject(data);

            return new BinaryData(json);
        }

        private SendReceipt GetSendReceipt(bool isBinary = default)
        {
            var sendReceipt = isBinary
                ? _service.SendMessageAsBinaryData(GenerateBinaryData())
                : _service.SendMessage(GenerateMessage());
            
            return sendReceipt;            
        }
        #endregion
    }
}
