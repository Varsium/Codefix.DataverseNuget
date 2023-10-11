namespace Codefix.Dataverse.Models.Dataverse
{
    internal sealed class DataverseRequest
    {
        internal HttpMethod HttpMethod { get; set; }

        internal string ApiRequest { get; set; }

        internal Dictionary<string, string> Headers { get; set; }

        internal string Content { get; set; }

    }
}
