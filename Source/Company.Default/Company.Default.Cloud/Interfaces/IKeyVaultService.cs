using Azure.Security.KeyVault.Secrets;

namespace Company.Default.Cloud.Interfaces
{
    public interface IKeyVaultService
    {
        KeyVaultSecret GetSecret(string name, string? version = null);
        Task<KeyVaultSecret> GetSecretAsync(string name, string? version = null, CancellationToken cancellationToken = default);
        IEnumerable<SecretProperties> GetSecretVersions(string name, int pageSize);
        Task<IEnumerable<SecretProperties>> GetSecretVersionsAsync(string name, CancellationToken cancellationToken = default);
        SecretProperties GetSecretProperties(string name, string? version = null);
        Task<SecretProperties> GetSecretPropertiesAsync(string name, string? version = null, CancellationToken cancellationToken = default);
        KeyVaultSecret SetSecret(string name, string value);
        Task<KeyVaultSecret> SetSecretAsync(string name, string value, CancellationToken cancellationToken = default);
        Task<IEnumerable<KeyVaultSecret>> ListSecretsAsync(string[] names, CancellationToken cancellationToken = default);
        Task<IEnumerable<SecretProperties>> ListAllPropertiesAsync(CancellationToken cancellationToken = default);
        void UpdateSecret(string name, string value, DateTimeOffset expiresOn);
        Task UpdateSecretAsync(string name, string value, DateTimeOffset expiresOn, CancellationToken cancellationToken = default);
        bool DeleteSecret(string name);
        Task<bool> DeleteSecretAsync(string name, CancellationToken cancellationToken = default);
        bool DeleteAndPurgeSecret(string name);
        Task<bool> DeleteAndPurgeSecretAsync(string name, CancellationToken cancellationToken = default);
        bool RecoverDeletedSecret(string name);
        Task<bool> RecoverDeletedSecretAsync(string name, CancellationToken cancellationToken = default);
        void BackupToFileSecret(string name, string filePath);
        Task BackupToFileSecretAsync(string name, string filePath, CancellationToken cancellationToken = default);
    }
}
