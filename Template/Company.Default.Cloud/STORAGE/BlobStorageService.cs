using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using $safeprojectname$.Interfaces;
using System.Collections;
using System.IO.Compression;
using System.Text;

namespace $safeprojectname$.Storage
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _serviceClient;
        private const string SAS_ERROR = "Storage account requirements exception: {0} must be authorized with Shared Key credentials to create a service SAS.";

        private void ValidateContainerAndBlobName(string containerName, string blobName)
        {
            if (string.IsNullOrEmpty(containerName)) throw new ArgumentNullException(nameof(containerName));
            if (string.IsNullOrEmpty(blobName)) throw new ArgumentNullException(nameof(blobName));
        }

        private void ValidateUploadWithHeaders(string containerName, string blobName, string contentType)
        {
            ValidateContainerAndBlobName(containerName, blobName);
            if(string.IsNullOrEmpty(contentType)) throw new ArgumentNullException(nameof(contentType));
        }

        private BlobHttpHeaders GetUploadHeader(string contentType) => new BlobHttpHeaders { ContentType = contentType };

        private BlobContainerClient GetContainerClient(string containerName) => _serviceClient.GetBlobContainerClient(containerName);

        private BlobClient GetBlobClient(string containerName, string blobName)
        {
            var container = GetContainerClient(containerName);

            return container.GetBlobClient(blobName);
        }

        private BlockBlobClient GetBlockBlobClient(string containerName, string blobName)
        {
            var container = GetContainerClient(containerName);

            return container.GetBlockBlobClient(blobName);
        }

        public BlobStorageService(BlobServiceClient blobServiceClient)
        {
            _serviceClient = blobServiceClient;
        }

        public async Task<BlobContainerClient> CreateContainerAsync(string containerName, 
            PublicAccessType publicAccessType, 
            IDictionary<string, string> metadata, 
            CancellationToken cancellationToken)
        {
            BlobContainerClient container = _serviceClient.GetBlobContainerClient(containerName);

            if(!container.Exists())
                await _serviceClient.CreateBlobContainerAsync(containerName, publicAccessType, metadata, cancellationToken);

            return container;
        }

        public async Task<bool> DeleteContainerAsync(string containerName, CancellationToken cancellationToken = default)
        {
            BlobContainerClient container = GetContainerClient(containerName);

            return await container.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }

        public IAsyncEnumerable<Azure.Page<BlobContainerItem>> GetListContainers(string prefix, int pageSize, CancellationToken cancellationToken)
        {
            IAsyncEnumerable<Azure.Page<BlobContainerItem>> result = _serviceClient.GetBlobContainersAsync(BlobContainerTraits.Metadata, prefix, cancellationToken).AsPages(default, pageSize);

            return result;
        }

        public async Task<BlobClient> UploadStreamAsync(string containerName,
            Stream contentStream, 
            string blobName, 
            string contentType, 
            CancellationToken cancellationToken)
        {
            ValidateUploadWithHeaders(containerName, blobName, contentType);

            BlobClient blobClient = GetBlobClient(containerName, blobName);
            BlobHttpHeaders headers = GetUploadHeader(contentType);

            await blobClient.UploadAsync(contentStream, headers, cancellationToken: cancellationToken);

            return blobClient;
        }

        public async Task<BlobClient> UploadStringAsync(string containerName, 
            string content, 
            string blobName, 
            CancellationToken cancellationToken)
        {
            ValidateContainerAndBlobName(containerName, blobName);

            BlobClient blobClient = GetBlobClient(containerName, blobName);
            BinaryData data = BinaryData.FromString(content);

            await blobClient.UploadAsync(data, overwrite: true, cancellationToken: cancellationToken);

            return blobClient;            
        }

        public async Task<BlobClient> UploadBase64Async(string containerName, 
            string contentBase64, 
            string blobName, 
            string contentType, 
            CancellationToken cancellationToken = default)
        {
            ValidateUploadWithHeaders(containerName, blobName, contentType);

            BlobClient blobClient = GetBlobClient(containerName, blobName);

            byte[] bytes = Convert.FromBase64String(contentBase64);
            BlobHttpHeaders headers = GetUploadHeader(contentType);

            using(var stream = new MemoryStream(bytes))
            {
                await blobClient.UploadAsync(stream, headers, cancellationToken: cancellationToken);
            }

            return blobClient;
        }

        public async Task<BlobClient> UploadFilePathAsync(string containerName, 
            string filePath, 
            string blobName, 
            string contentType, 
            CancellationToken cancellationToken = default)
        {
            ValidateUploadWithHeaders(containerName, blobName, contentType);

            BlobClient blobClient = GetBlobClient(containerName, blobName);

            FileStream file = File.OpenRead(filePath);
            BlobHttpHeaders headers = GetUploadHeader(contentType);

            await blobClient.UploadAsync(file, headers, cancellationToken: cancellationToken);

            file.Close();

            return blobClient;
        }

        public async Task<BlockBlobClient> UploadInBlocksAsync(string containerName, 
            string filePath, 
            string blobName, 
            int blockSize, 
            CancellationToken cancellationToken = default)
        {
            ValidateContainerAndBlobName(containerName, blobName);

            BlockBlobClient blockClient = GetBlockBlobClient(containerName, blobName);

            FileStream file = File.OpenRead(filePath);

            ArrayList blockIdArrayList = new ArrayList();

            byte[] buffer;
            var bytesLeft = (file.Length - file.Position);

            while(bytesLeft > 0)
            {
                int offset = 0;
                int count = bytesLeft >= blockSize ? blockSize : Convert.ToInt32(bytesLeft);
                buffer = bytesLeft >= blockSize ? new byte[blockSize] : new byte[bytesLeft];
                await file.ReadAsync(buffer, offset, count);

                using(var stream = new MemoryStream(buffer))
                {
                    string blockId = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));

                    blockIdArrayList.Add(blockId);

                    await blockClient.StageBlockAsync(blockId, stream, cancellationToken: cancellationToken);
                }

                bytesLeft = (file.Length - file.Position);
            }

            string[] blockIdArray = (string[])blockIdArrayList.ToArray(typeof(string));

            await blockClient.CommitBlockListAsync(blockIdArray);
            file.Close();

            return blockClient;            
        }

        public async Task<BlockBlobClient> UploadZipInBlockAsync(string containerName, 
            string filePath, 
            string blobName, 
            CancellationToken cancellationToken = default)
        {
            ValidateContainerAndBlobName(containerName, blobName);

            string zipFileName = blobName + ".zip";

            BlockBlobClient blockClient = GetBlockBlobClient(containerName, zipFileName);

            using(Stream stream = await blockClient.OpenWriteAsync(true, cancellationToken: cancellationToken))
            {
                using(ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: false)) 
                {
                    foreach(var fileName in Directory.EnumerateFiles(filePath))
                    {
                        using(var fileStream = File.OpenRead(fileName))
                        {
                            var entry = zip.CreateEntry(Path.GetFileName(fileName), CompressionLevel.Optimal);

                            using(var innerFile = entry.Open())
                            {
                                await fileStream.CopyToAsync(innerFile, cancellationToken);
                            }
                        }
                    }
                }
            }

            return blockClient;
        }

        public async Task<Stream> DownloadStreamAsync(string containerName, 
            string blobName, 
            CancellationToken cancellationToken = default)
        {
            BlobClient blobClient = GetBlobClient(containerName, blobName);
            BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();

            Stream content = downloadResult.Content.ToStream();

            return content;
        }

        public async Task DownloadToFileAsync(string containerName, 
            string blobName, 
            string filePath, 
            CancellationToken cancellationToken = default)
        {
            BlobClient blobClient = GetBlobClient(containerName, blobName);

            await blobClient.DownloadToAsync(filePath, cancellationToken);
        }

        public async Task<string> DownloadStringAsync(string containerName, string blobName, CancellationToken cancellation = default)
        {
            BlobClient blobClient = GetBlobClient(containerName, blobName);
            BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();

            string content = downloadResult.Content.ToString();

            return content;            
        }

        public async Task<bool> DeleteBlobAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
        {
            ValidateContainerAndBlobName(containerName, blobName);
            BlobClient blobClient = GetBlobClient(containerName, blobName);

            var response = await blobClient.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);

            return response.Status == 202;
        }

        public async Task<IList<BlobItem>> ListBlobs(string containerName, int pageSize, CancellationToken cancellationToken = default)
        {
            BlobContainerClient container = GetContainerClient(containerName);
            var result = new List<BlobItem>();

            var page = container.GetBlobsAsync().AsPages(default, pageSize);

            await foreach(Azure.Page<BlobItem> blobPage in page)
            {
                var blobs = blobPage.Values.ToList();
                result.AddRange(blobs);
            }

            return result;
        }

        public async Task<IList<TaggedBlobItem>> ListBlobsByTags(string query, CancellationToken cancellationToken = default)
        {
            List<TaggedBlobItem> blobs = new List<TaggedBlobItem>();

            await foreach(TaggedBlobItem taggedBlobItem in _serviceClient.FindBlobsByTagsAsync(query, cancellationToken))
            {
                blobs.Add(taggedBlobItem);
            }

            return blobs;
        }

        public async Task<BlobProperties> GetBlobPropertiesAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
        {
            ValidateContainerAndBlobName(containerName, blobName);
            BlobClient blobClient = GetBlobClient(containerName, blobName);
            var properties = await blobClient.GetPropertiesAsync(cancellationToken: cancellationToken);

            return properties.Value;
        }

        public Uri GenerateBlobSasUri(string containerName, string blobName, BlobSasBuilder blobSasBuilder)
        {
            BlobContainerClient container = GetContainerClient(containerName);
            BlobClient blobClient = container.GetBlobClient(blobName);

            if (!blobClient.CanGenerateSasUri)
                throw new InvalidOperationException(string.Format(SAS_ERROR, "BlobClient"));

            Uri sasUri = blobClient.GenerateSasUri(blobSasBuilder);

            return sasUri;
        }

        public Uri GenerateContainerSasUri(string containerName, BlobSasBuilder blobSasBuilder)
        {
            BlobContainerClient container = GetContainerClient(containerName);

            if(!container.CanGenerateSasUri)
                throw new InvalidOperationException(string.Format(SAS_ERROR, "ContainerClient"));

            Uri sasUri = container.GenerateSasUri(blobSasBuilder);

            return sasUri;
        }
    }
}
