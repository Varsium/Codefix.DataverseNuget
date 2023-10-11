using Codefix.Dataverse.Attributes;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Codefix.Dataverse.Entities
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public abstract class DataverseEntity
    {
        public string OdataEtag { get; set; }

        [JsonProperty("statuscode")]
        [JsonPropertyName("statusCode")]
        public string StatusCode { get; set; }

        [JsonProperty("statecode")]
        public string StateCode { get; set; }

        [JsonProperty("importsequencenumber")]
        public string ImportSequenceNumber { get; set; }

        [JsonProperty("timezoneruleversionnumber")]
        public string TimezoneRuleVersionNumber { get; set; }

        [JsonProperty("utcconversiontimezonecode")]
        public string UtcConversionTimezoneCode { get; set; }

        [JsonProperty("versionnumber")]
        public int? VersionNumber { get; set; }
        [ODataReadonly]
        [JsonProperty("owningbusinessunit")]
        public string OwningBusinessUnit { get; set; }

        [JsonProperty("createdon")]
        public DateTime? CreatedOn { get; set; }
        [ODataReadonly]
        [JsonProperty("_createdby_value")]
        public string CreatedById { get; set; }
        [ODataReadonly]
        [JsonProperty("_owningbusinessunit_value")]
        public string OwningBusinessUnitId { get; set; }

        [JsonProperty("modifiedon")]
        public DateTime? ModifiedOn { get; set; }
        [ODataReadonly]
        [JsonProperty("_owninguser_value")]
        public string OwningUserId { get; set; }
        [ODataReadonly]
        [JsonProperty("_modifiedby_value")]
        public string ModifiedById { get; set; }
        [ODataReadonly]
        [JsonProperty("_modifiedonbehalfby_value")]
        public string ModifiedOnBehalfById { get; set; }
        [ODataReadonly]
        [JsonProperty("_createdonbehalfby_value")]
        public string CreatedOnBehalfById { get; set; }
        [ODataReadonly]
        [JsonProperty("_owningteam_value")]
        public string OwningTeamId { get; set; }
        [ODataReadonly]
        [JsonProperty("_ownerid_value")]
        public string OwnerId { get; set; }

        [JsonProperty("overriddencreatedon")]
        public DateTime? OverriddenCreatedOn { get; set; }
        [JsonProperty("CallerObjectId")]
        [ODataReadonly]
        public string? OnBehalfOfAadId { get; set; }
        [ODataReadonly]
        [JsonProperty("MSCRMCallerID")]
        public string? OnBehalfOfMSCRMId { get; set; }
    }
}
