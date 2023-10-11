using CrossBorder.Dataverse.Attributes;
using CrossBorder.Dataverse.Entities;
using Newtonsoft.Json;

namespace Sandbox
{
    [ODataTable("cbe_jsl_persons")]
    public record Person 
    {


        [JsonProperty("cbe_birthdate")]
        public DateTime? CbeBirthdate { get; set; }


        [JsonProperty("cbe_lastname")]
        public string? CbeLastname { get; set; }

        [JsonProperty("cbe_transliteratedfirstname")]
        public string? CbeTransliteratedfirstname { get; set; }

        [JsonProperty("cbe_placeofbirthid")]
        public string? CbePlaceofbirthid { get; set; }

        [JsonProperty("cbe_genderid")]
        public string? CbeGenderid { get; set; }

        [JsonProperty("cbe_remarks")]
        public string? CbeRemarks { get; set; }

        [JsonProperty("cbe_isdeleted")]
        public bool? CbeIsdeleted { get; set; }

        [JsonProperty("cbe_personstatusid")]
        public string? CbePersonstatusid { get; set; }

        [JsonProperty("cbe_jsl_personid")]
        public string? CbeJslPersonid { get; set; }

        [JsonProperty("cbe_transliteratedlastname")]
        public string? CbeTransliteratedlastname { get; set; }

        [JsonProperty("cbe_isactive")]
        public string? CbeIsactive { get; set; }

        [JsonProperty("cbe_legallanguageid")]
        public string? CbeLegallanguageid { get; set; }

        [JsonProperty("cbe_nationalityid")]
        public string? CbeNationalityid { get; set; }

        [JsonProperty("cbe_language")]
        public string? CbeLanguage { get; set; }

        [JsonProperty("cbe_sid")]
        public string? CbeSid { get; set; }

        [JsonProperty("cbe_personrequestid")]
        public string? CbePersonrequestid { get; set; }

        [JsonProperty("cbe_placeofbirth")]
        public string? CbePlaceofbirth { get; set; }

        [JsonProperty("cbe_datedeceased")]
        public string? CbeDatedeceased { get; set; }


        [JsonProperty("cbe_angid")]
        public string? CbeAngid { get; set; }

        [JsonProperty("cbe_isverifiedperson")]
        public string? CbeIsverifiedperson { get; set; }

        [JsonProperty("cbe_focuskey")]
        public string? CbeFocuskey { get; set; }

        [JsonProperty("cbe_firstname")]
        public string? CbeFirstname { get; set; }

        [JsonProperty("cbe_legbandnumber")]
        public string? CbeLegbandnumber { get; set; }

        [JsonProperty("cbe_apfisnumber")]
        public string? CbeApfisnumber { get; set; }

        //[JsonProperty("cbe_tbd")]
        //public bool MainAddressKnown { get; set; }
        [JsonProperty("cbe_nrnbisn")]
        public string? CbeNrnbisn { get; set; }
        [JsonProperty("cbe_countryofbirthid")]
        public string? CbeCountryofbirthid { get; set; }

    }

}
