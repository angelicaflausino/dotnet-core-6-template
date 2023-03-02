using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using $ext_safeprojectname$.Cloud.KeyVault;

namespace $safeprojectname$.Cloud
{
    public class KeyVaultServiceTests
    {
        private readonly KeyVaultService _service;

        public KeyVaultServiceTests()
        {
            _service = GetKeyVaultService();
        }

        [Fact]
        public void GetSecret_Name_NotNullAndEquals()
        {
            var secret = GenerateSecret();

            var result = _service.GetSecret(secret.Key);

            Assert.NotNull(result);
            Assert.Equal(secret.Key, result.Name);
            Assert.Equal(secret.Value, result.Value);
        }

        [Fact]
        public async Task GetSecretAsync_Name_NotNullAndEquals()
        {
            var secret = GenerateSecret();

            var result = await _service.GetSecretAsync(secret.Key);

            Assert.NotNull(result);
            Assert.Equal(secret.Key, result.Name);
            Assert.Equal(secret.Value, result.Value);
        }

        [Fact]
        public void GetSecretVersions_NamePageSize_NotNull()
        {
            var secret = GenerateSecret();

            var result = _service.GetSecretVersions(secret.Key, 10);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetSecretVersionsAsync_Name_NotNull()
        {
            var secret = GenerateSecret();

            var result = await _service.GetSecretVersionsAsync(secret.Key);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetSecretProperties_Name_NotNull()
        {
            var secret = GenerateSecret();

            var result = _service.GetSecretProperties(secret.Key);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetSecretPropertiesAsync_Name_NotNullEqual()
        {
            var secret = GenerateSecret();

            var result = await _service.GetSecretPropertiesAsync(secret.Key);

            Assert.NotNull(result);
            Assert.Equal(result.Name, secret.Key);
        }

        [Fact]
        public void SetSecret_NameValue_NotNull()
        {
            var secret = GenerateSecret();

            var result = _service.SetSecret(secret.Key, secret.Value);

            Assert.NotNull(result);
        }

        [Fact]
        public async void SetSecretAsync_Name_Value_NotNull()
        {
            var secret = GenerateSecret();

            var result = await _service.SetSecretAsync(secret.Key, secret.Value);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ListSecretsAsync_Names_NotEmpty()
        {
            var secret1 = GenerateSecret();
            var secret2 = GenerateSecret();
            var secret3 = GenerateSecret();
            string[] names = new string[] { secret1.Key, secret2.Key, secret3.Key };

            var result = await _service.ListSecretsAsync(names);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task ListAllPropertiesAsync_NotEmptyAndAny()
        {
            var secret = GenerateSecret();

            var result = await _service.ListAllPropertiesAsync();

            Assert.NotEmpty(result);
            Assert.True(result.Any());
        }

        [Fact]
        public void UpdateSecret_NameValueExpiresOn_True()
        {
            var secret = GenerateSecret();
            var newValue = NewGuidString();
            var expiresOn = DateTimeOffset.Now.AddYears(1);

            _service.UpdateSecret(secret.Key, newValue, expiresOn);
        }

        [Fact]
        public void UpdateSecretAsync_NameValueExpiresOn_True()
        {
            var secret = GenerateSecret();
            var newValue = NewGuidString();
            var expiresOn = DateTimeOffset.Now.AddYears(1);

            var result = _service.UpdateSecretAsync(secret.Key, newValue, expiresOn);
            result.Wait();

            Assert.True(result.IsCompleted);
        }

        [Fact]
        public void DeleteSecret_Name_True()
        {
            var secret = GenerateSecret();

            var result = _service.DeleteSecret(secret.Key);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteSecretAsync_Name_True()
        {
            var secret = GenerateSecret();

            var result = await _service.DeleteSecretAsync(secret.Key);

            Assert.True(result);
        }

        [Fact]
        public void DeleteAndPurgeSecret_Name_True()
        {
            var secret = GenerateSecret();

            var result = _service.DeleteAndPurgeSecret(secret.Key);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAndPurgeSecretAsync_Name_True()
        {
            var secret = GenerateSecret();

            var result = await _service.DeleteAndPurgeSecretAsync(secret.Key);

            Assert.True(result);
        }

        [Fact]
        public void RecoverDeletedSecret_Name_True()
        {
            var secret = GenerateSecret();
            _service.DeleteSecret(secret.Key);

            var result = _service.RecoverDeletedSecret(secret.Key);

            Assert.True(result);
        }

        [Fact]
        public async Task RecoverDeletedSecretAsync_Name_True()
        {
            var secret = GenerateSecret();
            _service.DeleteSecret(secret.Key);

            var result = await _service.RecoverDeletedSecretAsync(secret.Key);

            Assert.True(result);
        }

        [Fact]
        public void BackupToFileSecret_NameFilePath_True()
        {
            var secret = GenerateSecret();
            var filePath = Path.Combine(Path.GetTempPath(), secret.Key);

            _service.BackupToFileSecret(secret.Key, filePath);

            Assert.True(File.Exists(filePath));            
        }

        [Fact]
        public async Task BackupToFileSecretAsync_NameFilePath_True()
        {
            var secret = GenerateSecret();
            var filePath = Path.Combine(Path.GetTempPath(), secret.Key);

            await _service.BackupToFileSecretAsync(secret.Key, filePath);

            Assert.True(File.Exists(filePath));
        }

        private string NewGuidString() => Guid.NewGuid().ToString();

        private KeyValuePair<string, string> GenerateSecret()
        {
            var secret = new KeyValuePair<string, string>(NewGuidString(), NewGuidString());

            _service.SetSecret(secret.Key, secret.Value);

            return secret;
        }

        private KeyVaultService GetKeyVaultService()
        {
            var config = TestUtils.GetConfiguration();

            var vaultUri = new Uri(config.GetSection("KeyVault:VaultUri").Value);

            SecretClient secretClient = new SecretClient(vaultUri, new DefaultAzureCredential());

            return new KeyVaultService(secretClient);
        }
    }
}
