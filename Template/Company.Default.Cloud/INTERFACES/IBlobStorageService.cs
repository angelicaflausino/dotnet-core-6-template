using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;

namespace $safeprojectname$.Interfaces
{
    public interface IBlobStorageService
    {
        /// <summary>
        /// Create storage account's container if not exists.
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="publicAccessType"></param>
        /// <param name="metadata"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="BlobContainerClient"/>
        /// </returns>
        Task<BlobContainerClient> CreateContainerAsync(string containerName, PublicAccessType publicAccessType, IDictionary<string, string> metadata, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Delete storage account's container if exists
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="bool"/>
        /// </returns>
        Task<bool> DeleteContainerAsync(string containerName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a paginated list of containers by searching by prefix
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="IAsyncEnumerable{T}"/> where T is a <see cref="Page{T}"/> and Page T is a <see cref="BlobContainerItem"/>
        /// </returns>
        IAsyncEnumerable<Page<BlobContainerItem>> GetListContainers(string prefix, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously uploads a Stream
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="contentStream"></param>
        /// <param name="blobName"></param>
        /// <param name="contentType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="BlobClient"/>
        /// </returns>
        Task<BlobClient> UploadStreamAsync(string containerName, Stream contentStream, string blobName, string contentType, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously uploads a String
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="content"></param>
        /// <param name="blobName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="BlobClient"/>
        /// </returns>
        Task<BlobClient> UploadStringAsync(string containerName, string content, string blobName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously uploads a encoded Base64 String
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="contentBase64"></param>
        /// <param name="blobName"></param>
        /// <param name="contentType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="BlobClient"/>
        /// </returns>
        Task<BlobClient> UploadBase64Async(string containerName, string contentBase64, string blobName, string contentType, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously uploads a File stored in directory
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="filePath"></param>
        /// <param name="blobName"></param>
        /// <param name="contentType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="BlobClient"/>
        /// </returns>
        Task<BlobClient> UploadFilePathAsync(string containerName, string filePath, string blobName, string contentType, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously uploads a File in Blocks
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="filePath"></param>
        /// <param name="blobName"></param>
        /// <param name="blockSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="BlockBlobClient"/>
        /// </returns>
        Task<BlockBlobClient> UploadInBlocksAsync(string containerName, string filePath, string blobName, int blockSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously uploads a compressed File in Blocks
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="folderPath"></param>
        /// <param name="blobName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="BlockBlobClient"/>
        /// </returns>
        Task<BlockBlobClient> UploadZipInBlockAsync(string containerName, string folderPath, string blobName, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Downloads the file contents as Stream
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="Stream"/>
        /// </returns>
        Task<Stream> DownloadStreamAsync(string containerName, string blobName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads the file contents to a specific directory
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <param name="filePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DownloadToFileAsync(string containerName, string blobName, string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads the file contents as string
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <param name="cancellation"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="string"/>
        /// </returns>
        Task<string> DownloadStringAsync(string containerName, string blobName, CancellationToken cancellation = default);

        /// <summary>
        /// Delete blob
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="bool"/>
        /// </returns>
        Task<bool> DeleteBlobAsync(string containerName, string blobName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously Get Blob Properties
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="BlobProperties"/>
        /// </returns>
        Task<BlobProperties> GetBlobPropertiesAsync(string containerName, string blobName, CancellationToken cancellationToken= default);

        /// <summary>
        /// Lists a certain amount of blobs from a specific container
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="IList{T}"/> where T is a <see cref="BlobItem"/>
        /// </returns>
        Task<IList<BlobItem>> ListBlobs(string containerName, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get TaggedBlobItem list by Tag
        /// <para>
        /// <see href="https://learn.microsoft.com/en-us/azure/storage/blobs/storage-blob-tags">Use blob index tags to manage and find data with .NET</see>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="IList{T}"/> where T is a <see cref="TaggedBlobItem"/>
        /// </returns>
        Task<IList<TaggedBlobItem>> ListBlobsByTags(string query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Generate SAS (Shared Access Signature) token for specific Blob
        /// <para>
        /// <see href="https://learn.microsoft.com/en-us/azure/storage/common/storage-sas-overview">Shared Access Signature overview</see>
        /// </para>
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <param name="blobSasBuilder"></param>
        /// <returns>
        /// <see cref="Uri"/>
        /// </returns>
        Uri GenerateBlobSasUri(string containerName, string blobName, BlobSasBuilder blobSasBuilder);

        /// <summary>
        /// Generate SAS (Shared Access Signature) token for specific Container
        /// <para>
        /// <see href="https://learn.microsoft.com/en-us/azure/storage/common/storage-sas-overview">Shared Access Signature overview</see>
        /// </para>
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobSasBuilder"></param>
        /// <returns>
        /// <see cref="Uri"/>
        /// </returns>
        Uri GenerateContainerSasUri(string containerName, BlobSasBuilder blobSasBuilder);
    }
}
