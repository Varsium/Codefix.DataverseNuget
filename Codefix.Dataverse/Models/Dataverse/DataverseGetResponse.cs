using Newtonsoft.Json;

namespace Codefix.Dataverse.Models.Dataverse
{
    public sealed class DataverseGetResponse<TEntity> where TEntity : class
    {
        [JsonProperty("@odata.context", NullValueHandling = NullValueHandling.Ignore)]
        public string? OdataContext { get; set; } = null;

        [JsonProperty("@odata.nextLink", NullValueHandling = NullValueHandling.Ignore)]
        public Uri? OdataNextLink { get; set; } = null;

        [JsonProperty("@odata.count")]
        public int Count { get; set; }
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public TEntity Value { get; set; }
    }
}

