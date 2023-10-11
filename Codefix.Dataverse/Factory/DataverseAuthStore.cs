using Codefix.Dataverse.Authentication;
using Codefix.Dataverse.Configs;
using Codefix.Dataverse.Enums;
using Codefix.Dataverse.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Codefix.Dataverse.Factory
{
    public sealed class DataverseAuthStore
    {
        private Dictionary<string, DataverseAuthConfig> DataverseConfigs { get; set; } = new Dictionary<string, DataverseAuthConfig>();

        private readonly string _environmentVariable;

        private readonly DataverseAuthStoreOptions _collectorOptions;

        private readonly ILogger<DataverseAuthStore> _logger;

        public DataverseAuthStore(DataverseAuthStoreOptions collectorOptions, ILogger<DataverseAuthStore> logger)
        {
            if (collectorOptions != null)
            {
                _collectorOptions = collectorOptions;
            }

            _logger = logger;
            _environmentVariable = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME");

            ConfigureEnvironmentConfigs();
            AddDataVerseConfigs();
        }

        internal DataverseAuthConfig GetDataVerseConfig(string projectName)
        {
            if (DataverseConfigs.ContainsKey(projectName))
            {
                return DataverseConfigs[projectName];
            }

            return null;
        }

        internal IList<DataverseAuthConfig> GetDataVerseConfigs()
        {
            var dataVerseConfigs = DataverseConfigs.GetDataVerseConfigs();
            return dataVerseConfigs.AddRange(DataverseConfigs.GetDataVerseConfigs());
        }

        private void AddDataVerseConfig(string dbContextname, DataverseAuthConfig config)
        {
            DataverseConfigs.Add(dbContextname, config);
        }
        private void ConfigureEnvironmentConfigs()
        {
            var env = _environmentVariable ?? "DEV";
            var result = AzureKeyVaultAuthConfig.GetConfigsFromKeyVault(_collectorOptions.KeyVaultUrl, env);
            var configs = JsonConvert.DeserializeObject<DataverseConfiguration>(result.DecodeBase64_UTF());
            if (configs != null)
            {
                LoadAllConfigs(configs);
                _logger?.LogInformation(" the basic authentication was succesfull.");
            }
            _logger?.LogWarning("there was given no Keyvault url and the basic authentication failed.");
        }

        private void LoadAllConfigs(DataverseConfiguration configs)
        {

            foreach (var config in configs?.Configurations)
            {
                var name = config?.ProjectName;
                var dataVerseConfig = new DataverseAuthConfig(config?.DataVerseUrl, configs?.TenantId, config?.ClientId, config?.ClientSecret);
                if (typeof(JusticeProject).GetProperties().Any(f => f.Name.ToUpperInvariant() == name.ToUpperInvariant()))
                {
                    AddDataVerseConfig(typeof(JusticeProject).GetProperties().FirstOrDefault(f => f.Name.ToUpperInvariant() == name.ToUpperInvariant()).Name, dataVerseConfig);
                }
                else
                {
                    AddDataVerseConfig(name, dataVerseConfig);
                }
            }
        }

        private void AddDataVerseConfigs()
        {
            foreach (var config in _collectorOptions.DataverseConfigs)
            {
                DataverseConfigs.Add(config.Key, config.Value);
            }

        }
    }
}
