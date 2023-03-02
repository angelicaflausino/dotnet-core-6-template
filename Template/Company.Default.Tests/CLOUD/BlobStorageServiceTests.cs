using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using $ext_safeprojectname$.Cloud.Storage;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;

namespace $safeprojectname$.Cloud
{
    public class BlobStorageServiceTests
    {
        private const string _samplePng = "sample.png";
        private readonly BlobStorageService _service;
        private readonly string _containerName = "test1";

        public BlobStorageServiceTests()
        {
            _service = GetBlobStorageService();
        }

        [Fact]
        public async Task CreateContainer_Test1_NotNull()
        {
            var accessType = PublicAccessType.BlobContainer;
            var metadata = new Dictionary<string, string>
            {
                { "keyId", Guid.NewGuid().ToString() },
                { "createdBy", Guid.NewGuid().ToString() }
            };

            var result = await _service.CreateContainerAsync(_containerName, accessType, metadata, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetListContainers_NotEmpty()
        {
            var result = new List<BlobContainerItem>();
            var prefix = _containerName;
            var pageSize = 20;

            var pagedResult = _service.GetListContainers(prefix, pageSize, CancellationToken.None);

            await foreach(var containerPage in pagedResult)
            {
                result.AddRange(containerPage.Values);
            }

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task DeleteContainer_NotCreated_False()
        {
            var containerName = "unknow";

            var result = await _service.DeleteContainerAsync(containerName, CancellationToken.None);

            Assert.False(result);
        }

        [Fact]
        public async Task UploadStreamAsync_SamplePng_NotNull()
        {
            var blobName = GenerateBlobName("png");
            var contentType = TryGetContentType(blobName);
            var stream = GetFileAsStream(_samplePng);

            var result = await _service.UploadStreamAsync(_containerName, stream, blobName, contentType, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UploadStringAsync_SampleText_NotNull()
        {
            var content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry";
            var blobName = GenerateBlobName("txt");

            var result = await _service.UploadStringAsync(_containerName, content, blobName, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UploadBase64Async_SampleBase64_NotNull()
        {
            var content = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNk+A8AAQUBAScY42YAAAAASUVORK5CYII=";
            var blobName = GenerateBlobName("png");
            var contentType = TryGetContentType(blobName);

            var result = await _service.UploadBase64Async(_containerName, content, blobName, contentType, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UploadFilePathAsync_SampleFile_NotNull()
        {
            var path = Path.Combine(GetCurrectDirectory(), "Assets", _samplePng);
            var blobName = GenerateBlobName("png");
            var contentType = TryGetContentType(blobName);

            var result = await _service.UploadFilePathAsync(_containerName, path, blobName, contentType, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UploadFileInBlocksAsync_SamplePng_NotNull()
        {
            var path = Path.Combine(GetCurrectDirectory(), "Assets", _samplePng);
            var blobName = GenerateBlobName("png");
            var blockSize = 10;

            var result = await _service.UploadInBlocksAsync(_containerName, path, blobName, blockSize, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UploadZipInBlockAsync_SamplePng_NotNull()
        {
            var folderPath = Path.Combine(GetCurrectDirectory(), "Assets");
            var blobName = Guid.NewGuid().ToString();

            var result = await _service.UploadZipInBlockAsync(_containerName, folderPath, blobName, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DownloadStreamAsync_SampleBlob_NotNull()
        {
            var blobName = await GetUploadedBlobName();

            var result = await _service.DownloadStreamAsync(_containerName, blobName, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DownloadStringAsync_SampleText_Equal()
        {
            var blobName = GenerateBlobName("text");
            var content = "sample text";
            await _service.UploadStringAsync(_containerName, content, blobName, CancellationToken.None);

            var result = await _service.DownloadStringAsync(_containerName, blobName, CancellationToken.None);

            Assert.Equal(content, result);
        }

        [Fact]
        public async Task DownloadToFileAsync_Blob_True()
        {
            var blobName = await GetUploadedBlobName();
            var filePath = Path.Combine(Path.GetTempPath(), blobName);
            await _service.DownloadToFileAsync(_containerName, blobName, filePath, CancellationToken.None);

            var fileExists = File.Exists(filePath);
            
            Assert.True(fileExists);
        }

        [Fact]
        public async Task DeleteBlobAsync_Blob_True()
        {
            var blobName = await GetUploadedBlobName();

            var result = await _service.DeleteBlobAsync(_containerName, blobName, CancellationToken.None);

            Assert.True(result);
        }

        [Fact]
        public async Task GetBlobPropertiesAsync_SampleBlob_NotNull()
        {
            var blobName = await GetUploadedBlobName();

            var result = await _service.GetBlobPropertiesAsync(_containerName, blobName, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ListBlobs_NotEmpty()
        {
            await GetUploadedBlobName();
            var pageSize = 10;

            var result = await _service.ListBlobs(_containerName, pageSize, CancellationToken.None);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GenerateBlobSasUri_NotNull()
        {
            var blobName = await GetUploadedBlobName();
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _containerName,
                BlobName = blobName,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
                Resource = "b"
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var result = _service.GenerateBlobSasUri(_containerName, blobName, sasBuilder);

            Assert.NotNull(result);
        }

        [Fact]
        public void GenerateContainerSasUri_NotNull()
        {
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _containerName,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
                Resource = "c"
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var result = _service.GenerateContainerSasUri(_containerName, sasBuilder);

            Assert.NotNull(result);
        }

        private BlobStorageService GetBlobStorageService()
        {
            var config = TestUtils.GetConfiguration();

            string connectionString = config.GetSection("Storage:ConnectionString").Value;
            
            BlobServiceClient client = new BlobServiceClient(connectionString);

            return new BlobStorageService(client);
        }

        private string TryGetContentType(string fileName)
        {
            string contentType = null;

            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);

            return contentType ?? "application/octet-stream";
        }

        private string GetCurrectDirectory() => Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        private Stream GetFileAsStream(string fileName)
        {
            var path = Path.Combine(GetCurrectDirectory(), "Assets", fileName);

            return File.OpenRead(path);
        }

        private string GenerateBlobName(string fileType) => $"{Guid.NewGuid()}.{fileType}";

        private async Task<string> GetUploadedBlobName()
        {
            var blobName = GenerateBlobName("png");
            var contentType = TryGetContentType(blobName);
            var stream = GetFileAsStream("sample.png");
            await _service.UploadStreamAsync(_containerName, stream, blobName, contentType, CancellationToken.None);

            return blobName;
        }
    }
}
