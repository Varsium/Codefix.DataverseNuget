using Azure.Core;

namespace Codefix.Dataverse.Configs
{
    public sealed class DataverseConfig
    {
        public string BaseUrl { get; private set; }
        public string[] Scopes { get; private set; }
        public string TenantId { get; private set; }
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }
        public string AuthUrl => "https://login.microsoftonline.com/" + TenantId + "/oauth2/v2.0/token";
        public string ApiUrl => "/api/data/v" + ApiVersion + "/";
        public string ApiVersion { get; private set; }
        public string ApiODataVersion { get; private set; }
        public TokenRequestContext TokenRequest { get; private set; }
        public TokenCredential TokenCredential { get; private set; }

        public DataverseConfig(string baseUrl, TokenCredential tokenRequest)
        {
            BaseUrl = baseUrl?.TrimEnd('/');
            Scopes = new string[] { $"{BaseUrl}/.default" };
            ApiVersion = "9.2";
            ApiODataVersion = "4.0";
            TokenCredential = tokenRequest;
        }
        public DataverseConfig(string baseUrl, string tenantId, string clientId, string clientSecret, string apiVersion = null, string apiODataVersion = null)
            : this(baseUrl, new string[] { $"{baseUrl}/.default" }, apiVersion, apiODataVersion, tenantId, clientId, clientSecret)
        {
        }

        public DataverseConfig(string baseUrl, string[] scopes, string apiVersion, string apiODataVersion, string tenantId, string clientId, string clientSecret)
        {
            BaseUrl = baseUrl?.TrimEnd('/');
            Scopes = scopes ?? new string[] { $"{BaseUrl}/.default" };
            TenantId = tenantId;
            ClientId = clientId;
            ClientSecret = clientSecret;
            ApiVersion = apiVersion ?? "9.2";
            ApiODataVersion = apiODataVersion ?? "4.0";
        }


        public bool WithInterActiveCredentials()
        {
            return !string.IsNullOrWhiteSpace(BaseUrl) &&
                   !string.IsNullOrWhiteSpace(TenantId) &&
                   !string.IsNullOrWhiteSpace(ClientId) &&
                   !string.IsNullOrWhiteSpace(ClientSecret);
        }
        public bool WithManagedIdentity()
        {
            if (TokenRequest.Scopes != null && TokenRequest.Scopes.Any() || TokenCredential is not null)
            {
                return true;
            }
            return false;
        }
    }
}
