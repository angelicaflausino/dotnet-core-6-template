using Azure.Security.KeyVault.Secrets;
using $safeprojectname$.Interfaces;

namespace $safeprojectname$.KeyVault
{
    public class KeyVaultService : IKeyVaultService
    {
        private readonly SecretClient _secretClient; 

        public KeyVaultService(SecretClient secretClient)
        {
            _secretClient = secretClient;
        }

        public KeyVaultSecret GetSecret(string name, string? version = null)
        {
            var response = _secretClient.GetSecret(name, version, CancellationToken.None);

            return response.Value;
        }

        public async Task<KeyVaultSecret> GetSecretAsync(string name, string? version = null, CancellationToken cancellationToken = default)
        {
            var response = await _secretClient.GetSecretAsync(name, version, cancellationToken);

            return response.Value;
        }

        public IEnumerable<SecretProperties> GetSecretVersions(string name, int pageSize)
        {
            List<SecretProperties> secretVersions = new List<SecretProperties>();

            var pageable = _secretClient.GetPropertiesOfSecretVersions(name).AsPages(default, pageSize);

            foreach (var versions in pageable)
            {
                var list = versions.Values.Where(x => x.Enabled.GetValueOrDefault()).ToList();

                secretVersions.AddRange(list);
            }

            return secretVersions;
        }

        public async Task<IEnumerable<SecretProperties>> GetSecretVersionsAsync(string name, CancellationToken cancellationToken = default)
        {
            List<SecretProperties> secretProperties = new List<SecretProperties>();

            var pageable = _secretClient.GetPropertiesOfSecretVersionsAsync(name, cancellationToken);

            await foreach (SecretProperties properties in pageable)
            {
                if (!properties.Enabled.GetValueOrDefault()) continue;
                secretProperties.Add(properties);
            }

            return secretProperties;
        }

        public SecretProperties GetSecretProperties(string name, string? version = null)
        {
            var secret = GetSecret(name, version);

            return secret.Properties;
        }

        public async Task<SecretProperties> GetSecretPropertiesAsync(string name, string? version = null, CancellationToken cancellationToken = default)
        {
            var secret = await GetSecretAsync(name, version, cancellationToken);

            return secret.Properties;
        }

        public KeyVaultSecret SetSecret(string name, string value)
        {
            var response = _secretClient.SetSecret(name, value);

            return response.Value;
        }

        public async Task<KeyVaultSecret> SetSecretAsync(string name, string value, CancellationToken cancellationToken = default)
        {
            var response = await _secretClient.SetSecretAsync(new KeyVaultSecret(name, value), cancellationToken);

            return response.Value;
        }

        public async Task<IEnumerable<KeyVaultSecret>> ListSecretsAsync(string[] names, CancellationToken cancellationToken = default)
        {
            if(names == null || names.Length == 0)
                throw new ArgumentException(nameof(names));

            List<KeyVaultSecret> listSecrets = new List<KeyVaultSecret>();

            foreach(var name in names)
            {
                var secret = (await _secretClient.GetSecretAsync(name, cancellationToken: CancellationToken.None)).Value;

                if (secret == null) continue;
                if(!secret.Properties.Enabled.GetValueOrDefault()) continue;

                listSecrets.Add(secret);
            }

            return listSecrets;
        }

        public async Task<IEnumerable<SecretProperties>> ListAllPropertiesAsync(CancellationToken cancellationToken = default)
        {
            var pageable = _secretClient.GetPropertiesOfSecretsAsync(cancellationToken);
            List<SecretProperties> listSecrets = new List<SecretProperties>();

            await foreach(var secretProperties in pageable)
            {
                if(!secretProperties.Enabled.GetValueOrDefault()) continue;

                listSecrets.Add(secretProperties);
            }

            return listSecrets;
        }

        public void UpdateSecret(string name, string value, DateTimeOffset expiresOn)
        {
            var secret = new KeyVaultSecret(name, value);
            secret.Properties.ExpiresOn = expiresOn;

            _secretClient.SetSecret(secret);
        }

        public async Task UpdateSecretAsync(string name, string value, DateTimeOffset expiresOn, CancellationToken cancellationToken = default)
        {
            var secret = new KeyVaultSecret(name, value);
            secret.Properties.ExpiresOn = expiresOn;

            await _secretClient.SetSecretAsync(secret, cancellationToken);            
        }

        public bool DeleteSecret(string name)
        {
            var operation = _secretClient.StartDeleteSecret(name);

            operation.WaitForCompletion();

            return operation.HasCompleted;
        }

        public async Task<bool> DeleteSecretAsync(string name, CancellationToken cancellationToken = default)
        {
            var operation = await _secretClient.StartDeleteSecretAsync(name, cancellationToken);

            await operation.WaitForCompletionAsync();

            return operation.HasCompleted;
        }

        public bool DeleteAndPurgeSecret(string name)
        {
            var operation = _secretClient.StartDeleteSecret(name);

            operation.WaitForCompletion();

            _secretClient.PurgeDeletedSecret(name);

            return operation.HasCompleted;
        }

        public async Task<bool> DeleteAndPurgeSecretAsync(string name, CancellationToken cancellationToken = default)
        {
            var operation = await _secretClient.StartDeleteSecretAsync(name, cancellationToken);

            await operation.WaitForCompletionAsync();

            await _secretClient.PurgeDeletedSecretAsync(name, cancellationToken);

            return operation.HasCompleted;
        }

        public bool RecoverDeletedSecret(string name)
        {
            var operation = _secretClient.StartRecoverDeletedSecret(name);

            operation.WaitForCompletion();

            return operation.HasCompleted;
        }

        public async Task<bool> RecoverDeletedSecretAsync(string name, CancellationToken cancellationToken = default)
        {
            var operation = await _secretClient.StartRecoverDeletedSecretAsync(name, cancellationToken);

            await operation.WaitForCompletionAsync(cancellationToken);

            return operation.HasCompleted;
        }

        public void BackupToFileSecret(string name, string filePath)
        {
            byte[] backupBytes = _secretClient.BackupSecret(name);

            File.WriteAllBytes(filePath, backupBytes);
        }

        public async Task BackupToFileSecretAsync(string name, string filePath, CancellationToken cancellationToken = default)
        {
            byte[] backup = await _secretClient.BackupSecretAsync(name, cancellationToken);

            await File.WriteAllBytesAsync(filePath, backup, cancellationToken);
        }

        
    }
}
