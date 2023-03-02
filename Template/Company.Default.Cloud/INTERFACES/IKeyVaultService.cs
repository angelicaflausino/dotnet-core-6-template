using Azure.Security.KeyVault.Secrets;

namespace $safeprojectname$.Interfaces
{
    public interface IKeyVaultService
    {
        /// <summary>
        /// Get secret by Name and Version
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="KeyVaultSecret"/>
        /// </returns>
        KeyVaultSecret GetSecret(string name, string? version = null);

        /// <summary>
        /// Asynchronously Get secret by Name and Version
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="KeyVaultSecret"/>
        /// </returns>
        Task<KeyVaultSecret> GetSecretAsync(string name, string? version = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// List Secret's versions
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageSize"></param>
        /// <returns>
        /// <see cref="IEnumerable{T}"/> where T is a <see cref="SecretProperties"/>
        /// </returns>
        IEnumerable<SecretProperties> GetSecretVersions(string name, int pageSize);

        /// <summary>
        /// Asynchronously Get secret's versions
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="IEnumerable{T}"/> and T is a <see cref="SecretProperties"/>
        /// </returns>
        Task<IEnumerable<SecretProperties>> GetSecretVersionsAsync(string name, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get secret's properties by version
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns>
        /// <see cref="SecretProperties"/>
        /// </returns>
        SecretProperties GetSecretProperties(string name, string? version = null);

        /// <summary>
        /// Asynchronously Get secret's properties by version
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="SecretProperties"/>
        /// </returns>
        Task<SecretProperties> GetSecretPropertiesAsync(string name, string? version = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Set a new secret
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns>
        /// <see cref="KeyVaultSecret"/>
        /// </returns>
        KeyVaultSecret SetSecret(string name, string value);

        /// <summary>
        /// Asynchronously Set a new secret
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="KeyVaultSecret"/>
        /// </returns>
        Task<KeyVaultSecret> SetSecretAsync(string name, string value, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously list secrets by array of names
        /// </summary>
        /// <param name="names"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="IEnumerable{T}"/> and T is a <see cref="KeyVaultSecret"/>
        /// </returns>
        Task<IEnumerable<KeyVaultSecret>> ListSecretsAsync(string[] names, CancellationToken cancellationToken = default);

        /// <summary>
        /// List all secret properties from KeyVault
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="IEnumerable{T}"/> and T is a <see cref="SecretProperties"/>
        /// </returns>
        Task<IEnumerable<SecretProperties>> ListAllPropertiesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Update secret
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="expiresOn"></param>
        void UpdateSecret(string name, string value, DateTimeOffset expiresOn);

        /// <summary>
        /// Asynchronously Update secret
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="expiresOn"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task"/>
        /// </returns>
        Task UpdateSecretAsync(string name, string value, DateTimeOffset expiresOn, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete Secret
        /// </summary>
        /// <param name="name"></param>
        /// <returns>
        /// <see cref="bool"/>
        /// </returns>
        bool DeleteSecret(string name);

        /// <summary>
        /// Asynchronously Delete secret
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="bool"/>
        /// </returns>
        Task<bool> DeleteSecretAsync(string name, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete and Purge secret
        /// <para>
        /// Purge permission is disabled by default, please visit 
        /// <see href="https://learn.microsoft.com/en-us/azure/key-vault/general/soft-delete-overview#purge-protection"/>
        /// </para>
        /// </summary>
        /// <param name="name"></param>
        /// <returns>
        /// <see cref="bool"/>
        /// </returns>
        bool DeleteAndPurgeSecret(string name);

        /// <summary>
        /// Asynchronously Delete and Purge secret
        /// <para>
        /// Purge permission is disabled by default, please visit 
        /// <see href="https://learn.microsoft.com/en-us/azure/key-vault/general/soft-delete-overview#purge-protection"/>
        /// </para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="bool"/>
        /// </returns>
        Task<bool> DeleteAndPurgeSecretAsync(string name, CancellationToken cancellationToken = default);

        /// <summary>
        /// Recover only soft-deleted secret
        /// </summary>
        /// <param name="name"></param>
        /// <returns>
        /// <see cref="bool"/>
        /// </returns>
        bool RecoverDeletedSecret(string name);

        /// <summary>
        /// Asynchronously Recover only soft-deleted secret
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="bool"/>
        /// </returns>
        Task<bool> RecoverDeletedSecretAsync(string name, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create secret backup to specific File
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filePath"></param>
        void BackupToFileSecret(string name, string filePath);

        /// <summary>
        /// Asynchronously Create secret backup to specific File
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <see cref="Task"/>
        /// </returns>
        Task BackupToFileSecretAsync(string name, string filePath, CancellationToken cancellationToken = default);
    }
}
