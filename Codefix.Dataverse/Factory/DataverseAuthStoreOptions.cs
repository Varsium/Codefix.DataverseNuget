using Codefix.Dataverse.Authentication;

namespace Codefix.Dataverse.Factory
{
    public sealed class DataverseAuthStoreOptions
    {
        internal Dictionary<string, DataverseAuthConfig> DataverseConfigs { get; set; } = new Dictionary<string, DataverseAuthConfig>();

        internal string Clientid { get; set; } = string.Empty;

        internal string ClientSecret { get; set; } = string.Empty;

        internal string TenantId { get; set; } = string.Empty;

        internal string KeyVaultUrl { get; set; } = string.Empty;

        public void SetupBaseCredentials(string keyVaultUrl, string tenantId = null, string clientId = null, string clientSecret = null)
        {
            TenantId = tenantId;
            Clientid = clientId;
            ClientSecret = clientSecret;
            KeyVaultUrl = keyVaultUrl;
        }

        public void AddDataVerseConfig(string dbContextname, string baseUrl, string tenantId, string clientId, string clientSecret, string apiVersion = null, string apiODataVersion = null)
        {
            var config = new DataverseAuthConfig(baseUrl, tenantId, clientId, clientSecret, apiVersion, apiODataVersion);
            DataverseConfigs.Add(dbContextname, config);
        }

        public void AddDataVerseConfig(string dbContextname, DataverseAuthConfig config)
        {
            DataverseConfigs.Add(dbContextname, config);
        }
    }
}
