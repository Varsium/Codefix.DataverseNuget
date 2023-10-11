using Azure.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Codefix.Dataverse.Configs
{
    public class DataverseServiceOptions
    {
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;
        public string? BaseUrl { get; set; }
        public TokenCredential? Credential { get; set; }
        public DataverseConfig? Config { get; set; }

    }
}
