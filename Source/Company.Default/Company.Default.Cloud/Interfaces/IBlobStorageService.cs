using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace Company.Default.Cloud.Interfaces
{
    public interface IBlobStorageService
    {
        Task<BlobContainerClient> CreateContainerAsync(string containerName, PublicAccessType publicAccessType, IDictionary<string, string> metadata, CancellationToken cancellationToken = default);
        Task<bool> DeleteContainerAsync(string containerName, CancellationToken cancellationToken = default);
        IAsyncEnumerable<Page<BlobContainerItem>> GetListContainers(string prefix, int pageSize, CancellationToken cancellationToken = default);
        Task<BlobClient> UploadStreamAsync(string containerName, Stream contentStream, string blobName, string contentType, CancellationToken cancellationToken = default);
        Task<BlobClient> UploadStringAsync(string containerName, string content, string blobName, CancellationToken cancellationToken = default);
        Task<BlobClient> UploadBase64Async(string containerName, string contentBase64, string blobName, string contentType, CancellationToken cancellationToken = default);
        Task<BlobClient> UploadFilePathAsync(string containerName, string filePath, string blobName, string contentType, CancellationToken cancellationToken = default);
        Task<BlockBlobClient> UploadInBlocksAsync(string containerName, string filePath, string blobName, int blockSize, CancellationToken cancellationToken = default);
        Task<BlockBlobClient> UploadZipInBlockAsync(string containerName, string folderPath, string blobName, CancellationToken cancellationToken = default);
        Task<Stream> DownloadStreamAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
        Task DownloadToFileAsync(string containerName, string blobName, string filePath, CancellationToken cancellationToken = default);
        Task<string> DownloadStringAsync(string containerName, string blobName, CancellationToken cancellation = default);
        Task<bool> DeleteBlobAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
        Task<BlobProperties> GetBlobPropertiesAsync(string containerName, string blobName, CancellationToken cancellationToken= default);
        Task<IList<BlobItem>> ListBlobs(string containerName, int pageSize, CancellationToken cancellationToken = default);
        Task<IList<TaggedBlobItem>> ListBlobsByTags(string query, CancellationToken cancellationToken = default);
    }
}
