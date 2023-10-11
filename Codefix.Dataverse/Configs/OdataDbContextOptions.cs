using Azure.Core;

namespace Codefix.Dataverse.Configs
{
    public class OdataDbContextOptions
    {
        public string BaseUrl { get; set; }
        public string? TenantId { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public TokenCredential? Credentials { get; set; }

        internal bool WithManagedIdentity()
        {
            if (Credentials is not null)
            {
                return true;
            }
            return false;
        }
    }
}
