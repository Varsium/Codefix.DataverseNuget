using Azure.Core;
using Azure.Identity;
using Codefix.Dataverse.Configs;
using Microsoft.Identity.Client;

namespace Codefix.Dataverse.Authentication
{
    public sealed class DataverseAuthConfig
    {
        private readonly DataverseConfig _config;
        private readonly IConfidentialClientApplication _msalClient;
        private TokenRequestContext _tokenRequest;
        private TokenCredential _credential;
        private DateTimeOffset ExpiresOn { get; set; }
        private string AccessToken { get; set; }

        public DataverseAuthConfig(DataverseConfig config)
        {
            _config = config;

            if (config.WithInterActiveCredentials())
            {
                _msalClient = ConfidentialClientApplicationBuilder
                    .Create(config.ClientId)
                    .WithClientSecret(config.ClientSecret)
                    .WithAuthority(AadAuthorityAudience.AzureAdMyOrg, true)
                    .WithTenantId(config.TenantId)
                    .Build();
            }
            throw new Exception("Dataverse Config is unvalid");
        }

        public DataverseAuthConfig(string baseurl, TokenCredential tokenRequest)
        {
            _config = new DataverseConfig(baseurl, tokenRequest);
            _credential = tokenRequest;
        }
        public DataverseAuthConfig(string baseUrl, string tenantId, string clientId, string clientSecret, string apiVersion = null, string apiODataVersion = null)
        {
            var config = new DataverseConfig(baseUrl, tenantId, clientId, clientSecret, apiVersion, apiODataVersion);

            _config = config;
            if (config.WithInterActiveCredentials())
            {
                _msalClient = ConfidentialClientApplicationBuilder
                    .Create(config.ClientId)
                    .WithClientSecret(config.ClientSecret)
                    .WithAuthority(AadAuthorityAudience.AzureAdMyOrg, true)
                    .WithTenantId(config.TenantId)
                    .Build();
                return;
            }
            throw new Exception("Dataverse Config is unvalid");
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(AccessToken) || ExpiresOn >= DateTime.UtcNow)
            {
                return AccessToken;
            }
            if (_config.WithManagedIdentity())
            {
                if (_credential is null && !string.IsNullOrEmpty(_config.ClientId))
                {
                    _credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = _config.ClientId });
                }
                _credential ??= new DefaultAzureCredential();
                if (!(_tokenRequest.Scopes?.Any()).GetValueOrDefault(false) || _tokenRequest.Scopes is null)
                {
                    _tokenRequest = new TokenRequestContext(new string[] { $"{_config.BaseUrl}/.default" });
                }
                var accesToken = await _credential.GetTokenAsync(_tokenRequest, CancellationToken.None);

                ExpiresOn = accesToken.ExpiresOn;
                AccessToken = accesToken.Token;
                return AccessToken;

            }
            var msal = await _msalClient.AcquireTokenForClient(_config.Scopes).ExecuteAsync();
            ExpiresOn = msal.ExpiresOn;
            AccessToken = msal.AccessToken;
            return AccessToken;
        }

        internal bool GetConfigValidation()
        {
            return _config.WithInterActiveCredentials();
        }

        internal DataverseConfig GetConfig()
        {
            return _config;
        }
    }
}
