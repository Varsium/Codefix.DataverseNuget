using Codefix.Dataverse.Attributes;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Codefix.Dataverse.Entities
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]

    public record DataverseRecord
    {
        public string? OdataEtag { get; set; }

        [JsonPropertyName("statuscode")]
        public string? StatusCode { get; set; }

        [JsonPropertyName("statecode")]
        public string? StateCode { get; set; }

        [JsonPropertyName("importsequencenumber")]
        public string? ImportSequenceNumber { get; set; }

        [JsonPropertyName("timezoneruleversionnumber")]
        public string? TimezoneRuleVersionNumber { get; set; }

        [JsonPropertyName("utcconversiontimezonecode")]
        public string? UtcConversionTimezoneCode { get; set; }

        [JsonPropertyName("versionnumber")]
        public int? VersionNumber { get; set; }

        [JsonPropertyName("owningbusinessunit")]
        public string? OwningBusinessUnit { get; set; }

        [JsonPropertyName("createdon")]
        public DateTime? CreatedOn { get; set; }

        [JsonPropertyName("modifiedon")]
        public DateTime? ModifiedOn { get; set; }
        [ODataReadonly(true)]
        [JsonPropertyName("_createdby_value")]
        public string? CreatedById { get; set; }
        [ODataReadonly(true)]
        [JsonPropertyName("_owningbusinessunit_value")]
        public string? OwningBusinessUnitId { get; set; }
        [ODataReadonly(true)]
        [JsonPropertyName("_owninguser_value")]
        public string? OwningUserId { get; set; }
        [ODataReadonly(true)]
        [JsonPropertyName("_modifiedby_value")]
        public string? ModifiedById { get; set; }
        [ODataReadonly(true)]
        [JsonPropertyName("_modifiedonbehalfby_value")]
        public string? ModifiedOnBehalfById { get; set; }
        [ODataReadonly(true)]
        [JsonPropertyName("_createdonbehalfby_value")]
        public string? CreatedOnBehalfById { get; set; }
        [ODataReadonly(true)]
        [JsonPropertyName("_owningteam_value")]
        public string? OwningTeamId { get; set; }
        [ODataReadonly(true)]
        [JsonPropertyName("_ownerid_value")]
        public string? OwnerId { get; set; }

        [JsonPropertyName("overriddencreatedon")]
        public DateTime? OverriddenCreatedOn { get; set; }
        [JsonProperty("CallerObjectId")]
        [ODataReadonly]
        public string? OnBehalfOfAadId { get; set; }
        [ODataReadonly]
        [JsonProperty("MSCRMCallerID")]
        public string? OnBehalfOfMSCRMId { get; set; }
    }
}
