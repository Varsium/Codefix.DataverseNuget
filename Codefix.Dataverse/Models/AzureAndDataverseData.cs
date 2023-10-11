using Newtonsoft.Json;

namespace Codefix.Dataverse.Models
{
    internal sealed class DataverseData
    {

        [JsonProperty("projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("clientSecret")]
        public string ClientSecret { get; set; }

        [JsonProperty("dataVerseUrl")]
        public string DataVerseUrl { get; set; }
    }
}
