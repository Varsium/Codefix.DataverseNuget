using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;

namespace Codefix.Dataverse.Authentication
{
    internal static class AzureKeyVaultAuthConfig
    {
        internal static string GetConfigsFromKeyVault(string keyVaultUrl, string environment)
        {
            if (keyVaultUrl == string.Empty || keyVaultUrl == null)
            {
                return string.Empty;
            }

            var creds = new DefaultAzureCredential(true);
            var client = new SecretClient(new Uri(keyVaultUrl), creds);
            KeyVaultSecret result = client.GetSecretAsync("CONFIGS" + CheckEnvironment(environment)).Result;
            return result.Value;

        }

        internal static string CheckEnvironment(string environment)
        {
            if (environment.ToLowerInvariant().Contains("tst"))
            {
                return "TST";
            }
            if (environment.ToLowerInvariant().Contains("prd"))
            {
                return "PRD";
            }
            return "DEV";
        }
    }
}
