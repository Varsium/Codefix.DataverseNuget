using CrossBorder.Dataverse.Attributes;
using CrossBorder.Dataverse.Entities;
using Newtonsoft.Json;

namespace Sandbox
{
    [ODataTable("cb_jsl_measures")]
    public record Measure : DataverseRecord
    {
        [OdataPrimaryKey]
        [JsonProperty("cb_jsl_measureid")]
        public Guid? MeasureId { get; set; }

        [JsonProperty("cb_measurerequestid")]
        public string? MeasureRequestId { get; set; }

        [JsonProperty("cb_awaitexecutioninformation")]
        public bool AwaitingExecutionInformation { get; set; }

        [JsonProperty("cb_subjecttobail")]
        public bool SubjectToBail { get; set; }

        [JsonProperty("cb_bailpaid")]
        public bool BailPaid { get; set; }

        [JsonProperty("cb_changeofresidenceallowed")]
        public bool ChangeOfResidenceAllowed { get; set; }

        [JsonProperty("cb_decisiondate")]
        public DateOnly? DecisionDate { get; set; }

        [JsonProperty("cb_startdate")]
        public DateOnly? StartDate { get; set; }

        [JsonProperty("cb_initialenddate")]
        public DateOnly? InitialEndDate { get; set; }

        [JsonProperty("cb_enddate")]
        public DateOnly? EndDate { get; set; }

        [JsonProperty("cb_ibcomment")]
        public string? IbComment { get; set; }

        [JsonProperty("cb_conditionstorespect")]
        public string? ConditionsToRespect { get; set; }

        [JsonProperty("cb_offenceid")]
        public string? OffenceId { get; set; }

        [JsonProperty("cb_signingmeasure")]
        public bool SigningMeasure { get; set; }

        [JsonProperty("cb_angid")]
        public string? AngId { get; set; }

        public Guid? CourtId { get; set; }

        [JsonProperty("cb_isdeleted")]
        public bool IsDeleted { get; set; } = false;

        [JsonProperty("_cb_justicehouseid_value")]

        public Guid? JusticeHouseId { get; set; }



        [JsonProperty("_cb_measurestatusid_value")]
        public Guid MeasureStatusId { get; set; }



        [JsonProperty("_cb_measuretypeid_value")]
        public Guid MeasureTypeId { get; set; }


        [JsonProperty("_cb_parquetid_value")]

        public Guid? ParquetId { get; set; }



        [JsonProperty("_cb_penitentiaryinstitutionid_value")]
        public Guid? PenitentiaryInstitutionId { get; set; }


        [JsonProperty("_cb_personid_value")]
        [ODataBind("cb_PersonId", typeof(Person))]
        public Guid PersonId { get; set; }

        [JsonProperty("cb_PersonId")]
        public Person? Person { get; set; }

        [JsonProperty("cb_casereferencemp")]
        public string? Casereferencemp { get; set; }


        [JsonProperty("_cb_responsibleinstanceid_value")]

        public Guid? ResponsibleInstanceId { get; set; }


        [JsonProperty("_cb_publicinstanceid_value")]

        public Guid PublicInstanceId { get; set; }


    }

}