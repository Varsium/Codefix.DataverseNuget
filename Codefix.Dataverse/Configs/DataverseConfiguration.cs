using System.Collections.Generic;
using System.Text.Json.Serialization;
using Codefix.Dataverse.Models;

namespace Codefix.Dataverse.Configs
{
    internal sealed class DataverseConfiguration
    {
        [JsonPropertyName("tenantId")]
        public string TenantId { get; set; }

        [JsonPropertyName("configurations")]
        public IList<DataverseData> Configurations { get; set; } = new List<DataverseData>();

    }
}
