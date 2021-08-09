using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.FHIR
{
    /// <summary>
    /// Query parameters/Search filters for Patient
    /// </summary>
    public class PatientQueryParameters
    {
        /// <summary>
        /// Given names e.g. James, Clark etc. 
        /// </summary>
        public string given { get; set; }

        /// <summary>
        /// Family name e.g. Johnson.
        /// </summary>
        public string family { get; set; }
        /// <summary>
        /// Business Identifier (not the same as logical id set by FHIR server internally)
        /// </summary>
        public string identifier { get; set; }

        /// <summary>
        /// identifier system
        /// </summary>
        public string idsystem { get; set; }

        /// <summary>
        /// Logical id set by FHIR server internally
        /// </summary>
        public string id { get; set; }
        public string address { get; set; }
    }

    /// <summary>
    /// Query parameters/Search filters for Organization
    /// </summary>
    public class OrganizationQueryParameters
    {
        /// <summary>
        /// A portion of the organization's name or alias
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// A server defined search that may match any of the string fields in the Address, 
        /// including line, city, district, state, country, postalCode, and/or text
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// Business Identifier (not the same as logical id set by FHIR server internally)
        /// </summary>
        public string identifier { get; set; }

        /// <summary>
        /// Logical id set by FHIR server internally
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// A code for the type of organization
        /// </summary>
        public string type { get; set; }
    }

    /// <summary>
    /// Query parameters/Search filters for Immunization
    /// </summary>
    public class ImmunizationQueryParameters
    {
        /// <summary>
        /// Reference to the patient.
        /// </summary>
        public string patient { get; set; }

        /// <summary>
        /// Date that can be parsed. If this cannot be parsed 
        /// successfully, it won't be used in query.
        /// </summary>
        public string date { get; set; }

        /// <summary>
        /// Business Identifier (not the same as logical id set by FHIR server internally)
        /// </summary>
        public string identifier { get; set; }

        /// <summary>
        /// Logical id set by FHIR server internally
        /// </summary>
        public string id { get; set; }

    }

    /// <summary>
    /// Query parameters/Search filters for Encounter
    /// </summary>
    public class EncounterQueryParameters
    {
        /// <summary>
        /// Subject of the encounter.
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        /// A date within the period the Encounter lasted
        /// </summary>
        public string date { get; set; }

        /// <summary>
        /// Business Identifier (not the same as logical id set by FHIR server internally)
        /// </summary>
        public string identifier { get; set; }

        /// <summary>
        /// Logical id set by FHIR server internally
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Classification of patient encounter
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Specific type of encounter
        /// </summary>
        public string type { get; set; }
    }

    /// <summary>
    /// Query parameters/Search filters for Observation
    /// </summary>
    public class ObservationQueryParameters
    {
        /// <summary>
        /// Subject of the observation (e.g. patient)
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// The classification of the type of observation laboratory, 
        /// vital-signs or social-history.
        /// </summary>
        public string category { get; set; }

        /// <summary>
        /// Obtained datetime. If the obtained element is a date range, a date that falls in the range.
        /// </summary>
        public string date { get; set; }

        /// <summary>
        /// Business Identifier (not the same as logical id set by FHIR server internally)
        /// </summary>
        public string identifier { get; set; }

        /// <summary>
        /// Logical id set by FHIR server internally
        /// </summary>
        public string id { get; set; }
    }

    /// <summary>
    /// Query parameters/Search filters for Diagnostic Report
    /// </summary>
    public class DiagnosticReportQueryParameters
    {
        /// <summary>
        /// Subject of the Diagnostic Report (e.g. patient)
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// The classification of the type of Diagnostic Report
        /// </summary>
        public string category { get; set; }

        /// <summary>
        /// Issued datetime. If the issued element is a date range, a date that falls in the range.
        /// </summary>
        public string date { get; set; }

        /// <summary>
        /// Business Identifier (not the same as logical id set by FHIR server internally)
        /// </summary>
        public string identifier { get; set; }

        /// <summary>
        /// Logical id set by FHIR server internally
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Reference to the result in the Diagnostic Report
        /// </summary>
        public string result { get; set; }
    }

    /// <summary>
    /// Query parameters/Search filters for Procedure
    /// </summary>
    public class ProcedureQueryParameters
    {
        /// <summary>
        /// Subject of the procedure (e.g. patient)
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// The classification of the type of procedure
        /// </summary>
        public string category { get; set; }

        /// <summary>
        /// Performed datetime. If the performed element is a date range, a date that falls in the range.
        /// </summary>
        public string date { get; set; }

        /// <summary>
        /// Business Identifier (not the same as logical id set by FHIR server internally)
        /// </summary>
        public string identifier { get; set; }

        /// <summary>
        /// Logical id set by FHIR server internally
        /// </summary>
        public string id { get; set; }
    }

    /// <summary>
    /// Query parameters/Search filters for Practitioner
    /// </summary>
    public class PractitionerQueryParameters
    {
        /// <summary>
        /// Given names e.g. James, Clark etc. 
        /// </summary>
        public string given { get; set; }

        /// <summary>
        /// Family name e.g. Johnson.
        /// </summary>
        public string family { get; set; }

        /// <summary>
        /// Business Identifier (not the same as logical id set by FHIR server internally)
        /// </summary>
        public string identifier { get; set; }

        /// <summary>
        /// Logical id set by FHIR server internally
        /// </summary>
        public string id { get; set; }
        public string address { get; set; }
    }

    public class ResourceMeta
    {
        [JsonProperty(Order = 1)]
        public string versionId { get; set; }
        [JsonProperty(Order = 2)]
        public string lastUpdated { get; set; }
    }

    public class BundleMeta
    {

        [JsonProperty(Order = 1)]
        public string lastUpdated { get; set; }
    }

    public class HumanName
    {
        [JsonProperty(Order = 1)]
        public string use { get; set; }
        [JsonProperty(Order = 2)]
        public string family { get; set; }
        [JsonProperty(Order = 3)]
        public List<string> given { get; set; }

        [Key]
        public string id { get; set; }
        public string parent { get; set; }
        public string resourceType { get; set; }
    }

    public class CodeableConcept
    {
        [JsonProperty(Order = 1)]
        public List<Coding> coding { get; set; }
        [JsonProperty(Order = 2)]
        public string text { get; set; }
        public string parent { get; set; }
    }

    public class Coding
    {
        [JsonProperty(Order = 1)]
        public string system { get; set; }

        [JsonProperty(Order = 2)]
        public string code { get; set; }

        [JsonProperty(Order = 3)]
        public string display { get; set; }
        public string parent { get; set; }
    }

    public class Identifier
    {
        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }

        [JsonProperty(Order = 2)]
        public string use { get; set; }

        [JsonProperty(Order = 3)]
        public CodeableConcept type { get; set; }

        [JsonProperty(Order = 4)]
        public string system { get; set; }
        [JsonProperty(Order = 5)]
        public string value { get; set; }

        [Key]
        public string id { get; set; }

        public string parent { get; set; }
    }

    public class Address
    {
        [JsonProperty(Order = 1)]
        public string use { get; set; }
        [JsonProperty(Order = 2)]
        public List<string> line { get; set; }

        [Key]
        public string id { get; set; }
        public string parent { get; set; }
        public string resourceType { get; set; }
    }

    public class ContactPoint
    {
        [JsonProperty(Order = 1)]
        public string system { get; set; }
        [JsonProperty(Order = 2)]
        public string value { get; set; }
        [JsonProperty(Order = 3)]
        public string use { get; set; }

        [Key]
        public string id { get; set; }
        public string parent { get; set; }
        public string resourceType { get; set; }
    }

    /// <summary>
    /// FHIR Practitioner resource. A practitioner is someone who practices medicine 
    /// for treating patients. They can be general medicine practitioners or specialists in some
    /// healthcare discipline.
    /// </summary>
    public class Practitioner
    {
        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }
        [JsonProperty(Order = 2)]
        public string id { get; set; }
        [JsonProperty(Order = 3)]
        public ResourceMeta meta { get; set; }

        [JsonProperty(Order = 4)]
        public List<Identifier> identifier { get; set; }
        [JsonProperty(Order = 5)]
        public bool active { get; set; }

        [JsonProperty(Order = 6)]
        public List<HumanName> name { get; set; }

        [JsonProperty(Order = 7)]
        public List<ContactPoint> telecom { get; set; }

        [JsonProperty(Order = 8)]
        public List<Address> address { get; set; }

        [JsonProperty(Order = 9)]
        public string gender { get; set; }
        [JsonProperty(Order = 10)]
        public string birthDate { get; set; }
        [JsonProperty(Order = 11)]
        public List<Qualification> qualification { get; set; }
    }

    public class Period
    {
        [JsonProperty(Order = 1)]
        public string start { get; set; }
        [JsonProperty(Order = 2)]
        public string end { get; set; }
    }


    public class StatusReason
    {
        [JsonProperty(Order = 1)]
        public List<Coding> coding { get; set; }
        [JsonProperty(Order = 2)]
        public string text { get; set; }
    }


    public class VaccineCode
    {
        [JsonProperty(Order = 1)]
        public List<Coding> coding { get; set; }
        [JsonProperty(Order = 2)]
        public string text { get; set; }
    }

    /// <summary>
    /// Immunization resource is intended to cover the recording of current and historical administration 
    /// of vaccines to patients.
    /// </summary>
    public class Immunization
    {
        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }
        [JsonProperty(Order = 2)]
        public string id { get; set; }
        [JsonProperty(Order = 3)]
        public ResourceMeta meta { get; set; }

        [JsonProperty(Order = 4)]
        public List<Identifier> identifier { get; set; }
        [JsonProperty(Order = 5)]
        public string status { get; set; }
        [JsonProperty(Order = 6)]
        public StatusReason statusReason { get; set; }
        [JsonProperty(Order = 7)]
        public VaccineCode vaccineCode { get; set; }
        [JsonProperty(Order = 8)]
        public Reference patient { get; set; }
        [JsonProperty(Order = 9)]
        public string occurrenceDateTime { get; set; }
        [JsonProperty(Order = 10)]
        public bool primarySource { get; set; }
    }

    /// <summary>
    /// As per HL7 org, Observations are central elements in healthcare. They are used to support diagnosis, monitor progress, 
    /// determine baselines and patterns and even capture demographic characteristics. 
    /// Most observations are simple name/value pair assertions with some metadata, but some observations group other observations together logically, 
    /// or even are multi-component observations.
    /// </summary>
    public class Observation
    {
        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }
        [JsonProperty(Order = 2)]
        public string id { get; set; }
        [JsonProperty(Order = 3)]
        public ResourceMeta meta { get; set; }

        [JsonProperty(Order = 4)]
        public List<Identifier> identifier { get; set; }
        [JsonProperty(Order = 5)]
        public string status { get; set; }
        [JsonProperty(Order = 6)]
        public List<CodeableConcept> category { get; set; }
        [JsonProperty(Order = 7)]
        public CodeableConcept code { get; set; }
        [JsonProperty(Order = 8)]
        public Reference subject { get; set; }
        [JsonProperty(Order = 9)]
        public Reference encounter { get; set; }

        [JsonProperty(Order = 10)]
        public string effectiveDateTime { get; set; }

        [JsonProperty(Order = 11)]
        public Quantity valueQuantity { get; set; }
    }

    /// <summary>
    /// As per HL7 org, a diagnostic report is the set of information that is typically provided 
    /// by a diagnostic service when investigations are complete. 
    /// The information includes a mix of atomic results, text reports, images, and codes. 
    /// </summary>
    public class DiagnosticReport
    {
        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }
        [JsonProperty(Order = 2)]
        public string id { get; set; }
        [JsonProperty(Order = 3)]
        public ResourceMeta meta { get; set; }

        [JsonProperty(Order = 4)]
        public List<Identifier> identifier { get; set; }
        [JsonProperty(Order = 5)]
        public string status { get; set; }
        [JsonProperty(Order = 6)]
        public List<CodeableConcept> category { get; set; }

        [JsonProperty(Order = 7)]
        public CodeableConcept code { get; set; }

        /// <summary>
        /// The subject of this diagnosis report - usually, but not always, the patient.
        /// </summary>
        [JsonProperty(Order = 8)]
        public Reference subject { get; set; }

        /// <summary>
        /// Health care event when test ordered
        /// </summary>
        [JsonProperty(Order = 9)]
        public Reference encounter { get; set; }

        [JsonProperty(Order = 10)]
        public string effectiveDateTime { get; set; }

        [JsonProperty(Order = 11)]
        public Period effectivePeriod { get; set; }

        /// <summary>
        /// Time the report is issued.
        /// An instant in time in the format YYYY-MM-DDThh:mm:ss.sss+zz:zz 
        /// (e.g. 2015-02-07T13:28:17.239+02:00 or 2017-01-01T00:00:00Z). 
        /// A system log kind of format is needed to accurately know when the report was generated or issued.
        /// The time shall specify at least to the second and shall include a time zone.
        /// </summary>
        [JsonProperty(Order = 12)]
        public string issued { get; set; }

        /// <summary>
        /// Responsible Diagnostic Service who performed these diagnoses.
        /// Could be a list of references.
        /// </summary>
        [JsonProperty(Order = 13)]
        public List<Reference> performer { get; set; }

        /// <summary>
        /// Observations
        /// </summary>
        [JsonProperty(Order = 14)]
        public List<Reference> result { get; set; }

        /// <summary>
        /// Codes for the clinical conclusion of test results
        /// </summary>
        [JsonProperty(Order = 15)]
        public List<CodeableConcept> conclusionCode { get; set; }
    }

    /// <summary>
    /// As per HL7 org, Procedure resource is used to record the details of current and historical procedures performed on or for a patient. 
    /// A procedure is an activity that is performed on, with, or for a patient as part of the provision of care. 
    /// Examples include surgical procedures, diagnostic procedures, endoscopic procedures, biopsies, 
    /// counseling, physiotherapy, personal support services, adult day care services, 
    /// non-emergency transportation, home modification, exercise, etc. 
    /// Procedures may be performed by a healthcare professional, a service provider, 
    /// a friend or relative or in some cases by the patient themselves.
    /// </summary>
    public class Procedure
    {
        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }
        [JsonProperty(Order = 2)]
        public string id { get; set; }
        [JsonProperty(Order = 3)]
        public ResourceMeta meta { get; set; }

        [JsonProperty(Order = 4)]
        public List<Identifier> identifier { get; set; }
        [JsonProperty(Order = 5)]
        public string status { get; set; }
        [JsonProperty(Order = 6)]
        public List<CodeableConcept> category { get; set; }
        [JsonProperty(Order = 7)]
        public CodeableConcept code { get; set; }
        [JsonProperty(Order = 8)]
        public Reference subject { get; set; }
        [JsonProperty(Order = 9)]
        public Reference encounter { get; set; }

        [JsonProperty(Order = 10)]
        public string performedDateTime { get; set; }

        [JsonProperty(Order = 11)]
        public Period performedPeriod { get; set; }
    }

    /// <summary>
    /// AllergyIntolerance denotes a risk of harmful or undesirable, physiological response 
    /// which is unique to an individual and is associated with an exposure to a substance.
    /// </summary>
    public class AllergyIntolerance
    {
        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }
        [JsonProperty(Order = 2)]
        public string id { get; set; }
        [JsonProperty(Order = 3)]
        public ResourceMeta meta { get; set; }

        [JsonProperty(Order = 4)]
        public List<Identifier> identifier { get; set; }

        /// <summary>
        /// Whether it is resolved, active/inactive
        /// </summary>
        [JsonProperty(Order = 5)]
        public CodeableConcept clinicalStatus { get; set; }

        /// <summary>
        /// Whether the allergy or intolerance is verified/confirmed.
        /// </summary>
        [JsonProperty(Order = 6)]
        public CodeableConcept verificationStatus { get; set; }

        [JsonProperty(Order = 7)]
        public string type { get; set; }

        /// <summary>
        /// Category of allergy/intolerance. e.g. food | medication | environment | biologic
        /// </summary>
        [JsonProperty(Order = 8)]
        public List<string> category { get; set; }

        [JsonProperty(Order = 9)]
        public string criticality { get; set; }

        [JsonProperty(Order = 10)]
        public CodeableConcept code { get; set; }

        [JsonProperty(Order = 11)]
        public Reference patient { get; set; }

        [JsonProperty(Order = 12)]
        public Reference encounter { get; set; }

        [JsonProperty(Order = 13)]
        public string onsetDateTime { get; set; }

        [JsonProperty(Order = 14)]
        public string recordedDate { get; set; }

        /// <summary>
        /// Who recorded this sensitivity or reaction?
        /// </summary>
        [JsonProperty(Order = 15)]
        public Reference recorder { get; set; }

        /// <summary>
        /// /// <summary>
        /// Source of the information of the allergy
        /// </summary>
        /// </summary>
        [JsonProperty(Order = 16)]
        public Reference asserter { get; set; }

        /// <summary>
        /// Date(/time) of last known occurrence of a reaction
        /// </summary>
        [JsonProperty(Order = 17)]
        public string lastOccurrence { get; set; }

        /// <summary>
        /// Additional text not captured in other fields
        /// </summary>
        [JsonProperty(Order = 18)]
        public List<Annotation> note { get; set; }

        /// <summary>
        /// Reactions to the substance or allergies noted
        /// </summary>
        [JsonProperty(Order = 19)]
        public List<Reaction> reaction { get; set; }
    }

    /// <summary>
    /// Search parameters for AllergyIntolerance.
    /// </summary>
    public class AllergyIntoleranceQueryParameters
    {
        /// <summary>
        /// AllergyIntolerance.asserter
        /// </summary>
        public string asserter { get; set; }
        /// <summary>
        /// AllergyIntolerance.category
        /// </summary>
        public string category { get; set; }

        /// <summary>
        /// AllergyIntolerance.clinicalStatus
        /// </summary>
        public string clinicalstatus { get; set; }

        /// <summary>
        /// AllergyIntolerance.code | AllergyIntolerance.reaction.substance
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// AllergyIntolerance.criticality
        /// </summary>
        public string criticality { get; set; }

        /// <summary>
        /// AllergyIntolerance.recordedDate in yyyy-MM-dd format.
        /// </summary>
        public string date { get; set; }

        /// <summary>
        /// AllergyIntolerance.identifier
        /// </summary>
        public string identifier { get; set; }

        /// <summary>
        /// AllergyIntolerance.reaction.onset in yyyy-MM-dd format.
        /// </summary>
        public string onset { get; set; }

        /// <summary>
        /// AllergyIntolerance.patient
        /// </summary>
        public string patient { get; set; }
    }

    /// <summary>
    /// Records reaction or allergy to a substance.
    /// </summary>
    public class Reaction
    {
        /// <summary>
        /// Specific substance or pharmaceutical product 
        /// considered to be responsible for event
        /// </summary>
        [JsonProperty(Order = 1)]
        public CodeableConcept substance { get; set; }

        /// <summary>
        /// Clinical symptoms/signs associated with the Event
        /// </summary>
        [JsonProperty(Order = 2)]
        public List<CodeableConcept> manifestation { get; set; }

        /// <summary>
        /// Description of the event as a whole
        /// </summary>
        [JsonProperty(Order = 3)]
        public string description { get; set; }

        /// <summary>
        /// Date(/time) when manifestations showed
        /// </summary>
        [JsonProperty(Order = 4)]
        public string onset { get; set; }

        /// <summary>
        /// mild | moderate | severe (serverity of the event as a whole)
        /// </summary>
        [JsonProperty(Order = 5)]
        public string severity { get; set; }

        /// <summary>
        /// How the subject was exposed to the substance
        /// </summary>
        [JsonProperty(Order = 6)]
        public CodeableConcept exposureRoute { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        [JsonProperty(Order = 7)]
        public List<Annotation> note { get; set; }
    }

    /// <summary>
    /// Annotation on the specific record.
    /// </summary>
    public class Annotation
    {

        [JsonProperty(Order = 1)]
        public Reference authorReference { get; set; }

        [JsonProperty(Order = 2)]
        public string authorString { get; set; }

        [JsonProperty(Order = 3)]
        public string time { get; set; }
        [JsonProperty(Order = 4)]
        public string text { get; set; }
    }


    /// <summary>
    /// As per HL7 org, Encounter resource is not to be used to store appointment information. Rather the Appointment resource is intended to be used for that. 
    /// Appointment is used for establishing a date for the encounter, while Encounter resource is applicable to information 
    /// about the actual Encounter, i.e., the patient showing up.
    /// </summary>
    public class Encounter
    {
        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }
        [JsonProperty(Order = 2)]
        public string id { get; set; }
        [JsonProperty(Order = 3)]
        public ResourceMeta meta { get; set; }

        [JsonProperty(Order = 4)]
        public List<Identifier> identifier { get; set; }
        [JsonProperty(Order = 5)]
        public string status { get; set; }

        [JsonProperty(Order = 6)]
        public Coding Class { get; set; }

        [JsonProperty(Order = 7)]
        public List<CodeableConcept> type { get; set; }

        [JsonProperty(Order = 8)]
        public CodeableConcept serviceType { get; set; }

        [JsonProperty(Order = 9)]
        public CodeableConcept priority { get; set; }

        [JsonProperty(Order = 10)]
        public Reference subject { get; set; }

        [JsonProperty(Order = 11)]
        public List<Participant> participant { get; set; }

        [JsonProperty(Order = 12)]
        public Period period { get; set; }

        [JsonProperty(Order = 13)]
        public Quantity length { get; set; }

        [JsonProperty(Order = 14)]
        public List<CodeableConcept> reasonCode { get; set; }

        [JsonProperty(Order = 15)]
        public Hospitalization hospitalization { get; set; }

        [JsonProperty(Order = 16)]
        public Reference serviceProvider { get; set; }
    }

    public class Hospitalization
    {
        [JsonProperty(Order = 1)]
        public Identifier preAdmissionIdentifier { get; set; }

        [JsonProperty(Order = 2)]
        public Reference origin { get; set; }

        [JsonProperty(Order = 3)]
        public CodeableConcept admitSource { get; set; }

        [JsonProperty(Order = 4)]
        public CodeableConcept dischargeDisposition { get; set; }
    }


    public class Participant
    {
        [JsonProperty(Order = 1)]
        public List<CodeableConcept> type { get; set; }

        [JsonProperty(Order = 2)]
        public Period period { get; set; }
        [JsonProperty(Order = 3)]
        public Reference individual { get; set; }
    }

    /// <summary>
    /// Indicates values with quantity, unit 
    /// for vital signs etc.
    /// </summary>
    public class Quantity
    {
        [JsonProperty(Order = 1)]
        public int value { get; set; }
        [JsonProperty(Order = 2)]
        public string unit { get; set; }
        [JsonProperty(Order = 3)]
        public string system { get; set; }
        [JsonProperty(Order = 4)]
        public string code { get; set; }
    }

    public class Qualification
    {
        [JsonProperty(Order = 1)]
        public List<Identifier> identifier { get; set; }
        [JsonProperty(Order = 2)]
        public CodeableConcept code { get; set; }
        [JsonProperty(Order = 3)]
        public Period period { get; set; }

        [JsonProperty(Order = 4)]
        public Reference issuer { get; set; }
    }

    public class Reference
    {
        [JsonProperty(Order = 1)]
        public string reference { get; set; }
        [JsonProperty(Order = 2)]
        public string type { get; set; }

        [JsonProperty(Order = 3)]
        public Identifier identifier { get; set; }
        [JsonProperty(Order = 4)]
        public string display { get; set; }
    }

    /// <summary>
    /// FHIR Organization class.
    /// </summary>
    public class Organization
    {
        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }
        [JsonProperty(Order = 2)]
        public string id { get; set; }
        [JsonProperty(Order = 3)]
        public ResourceMeta meta { get; set; }

        [JsonProperty(Order = 4)]
        public List<Identifier> identifier { get; set; }
        [JsonProperty(Order = 5)]
        public bool active { get; set; }

        [JsonProperty(Order = 6)]
        public List<CodeableConcept> type { get; set; }

        [JsonProperty(Order = 7)]
        public string name { get; set; }

        [JsonProperty(Order = 8)]
        public List<string> alias { get; set; }

        [JsonProperty(Order = 9)]
        public List<ContactPoint> telecom { get; set; }

        [JsonProperty(Order = 10)]
        public List<Address> address { get; set; }

        [JsonProperty(Order = 11)]
        public List<Contact> contact { get; set; }
    }

    /// <summary>
    /// Contact details of a resource such as Organization or Patient. 
    /// </summary>
    public class Contact
    {
        [JsonProperty(Order = 1)]
        public CodeableConcept purpose { get; set; }

        [JsonProperty(Order = 2)]
        public HumanName name { get; set; }

        [JsonProperty(Order = 3)]
        public List<ContactPoint> telecom { get; set; }

        [JsonProperty(Order = 4)]
        public Address address { get; set; }
    }

    /// <summary>
    /// FHIR Patient class.
    /// </summary>
    public class Patient
    {


        public string parent { get; set; }

        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }
        [JsonProperty(Order = 2)]

        [Key]
        public string id { get; set; }
        [JsonProperty(Order = 3)]
        public ResourceMeta meta { get; set; }

        [JsonProperty(Order = 4)]
        public List<Identifier> identifier { get; set; }
        [JsonProperty(Order = 5)]
        public bool active { get; set; }

        [JsonProperty(Order = 6)]
        public List<HumanName> name { get; set; }

        [JsonProperty(Order = 7)]
        public List<ContactPoint> telecom { get; set; }

        [JsonProperty(Order = 8)]
        public List<Address> address { get; set; }

        [JsonProperty(Order = 9)]
        public string gender { get; set; }
        [JsonProperty(Order = 10)]
        public string birthDate { get; set; }
    }

    public class Link
    {
        [JsonProperty(Order = 1)]
        public string relation { get; set; }
        [JsonProperty(Order = 2)]
        public string url { get; set; }
    }

    public class Search
    {
        public string mode { get; set; }
    }

    public class PatientEntry
    {
        [JsonProperty(Order = 1)]
        public string fullUrl { get; set; }
        [JsonProperty(Order = 2)]
        public Patient resource { get; set; }
        [JsonProperty(Order = 3)]
        public Search search { get; set; }
    }

    public class EncounterEntry
    {
        [JsonProperty(Order = 1)]
        public string fullUrl { get; set; }
        [JsonProperty(Order = 2)]
        public Encounter resource { get; set; }
        [JsonProperty(Order = 3)]
        public Search search { get; set; }
    }

    
    public class DiagnosticReportEntry
    {
        [JsonProperty(Order = 1)]
        public string fullUrl { get; set; }
        [JsonProperty(Order = 2)]
        public DiagnosticReport resource { get; set; }
        [JsonProperty(Order = 3)]
        public Search search { get; set; }
    }

    public class AllergyIntoleranceEntry
    {
        [JsonProperty(Order = 1)]
        public string fullUrl { get; set; }
        [JsonProperty(Order = 2)]
        public AllergyIntolerance resource { get; set; }
        [JsonProperty(Order = 3)]
        public Search search { get; set; }
    }
    public class OrganizationEntry
    {
        [JsonProperty(Order = 1)]
        public string fullUrl { get; set; }
        [JsonProperty(Order = 2)]
        public Organization resource { get; set; }
        [JsonProperty(Order = 3)]
        public Search search { get; set; }
    }

    public class PractitionerEntry
    {
        [JsonProperty(Order = 1)]
        public string fullUrl { get; set; }
        [JsonProperty(Order = 2)]
        public Practitioner resource { get; set; }
        [JsonProperty(Order = 3)]
        public Search search { get; set; }
    }

    public class ProcedureEntry
    {
        [JsonProperty(Order = 1)]
        public string fullUrl { get; set; }
        [JsonProperty(Order = 2)]
        public Procedure resource { get; set; }
        [JsonProperty(Order = 3)]
        public Search search { get; set; }
    }

    public class ImmunizationEntry
    {
        [JsonProperty(Order = 1)]
        public string fullUrl { get; set; }
        [JsonProperty(Order = 2)]
        public Immunization resource { get; set; }
        [JsonProperty(Order = 3)]
        public Search search { get; set; }
    }

    public class ObservationEntry
    {
        [JsonProperty(Order = 1)]
        public string fullUrl { get; set; }
        [JsonProperty(Order = 2)]
        public Observation resource { get; set; }
        [JsonProperty(Order = 3)]
        public Search search { get; set; }
    }

    public class PatientBundle
    {
        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }
        [JsonProperty(Order = 2)]
        public string id { get; set; }
        [JsonProperty(Order = 3)]
        public BundleMeta meta { get; set; }
        [JsonProperty(Order = 4)]
        public bool active { get; set; }

        [JsonProperty(Order = 5)]
        public string type { get; set; }
        [JsonProperty(Order = 6)]
        public List<Link> link { get; set; }
        [JsonProperty(Order = 7)]
        public List<PatientEntry> entry { get; set; }
    }

    public class OrganizationBundle
    {
        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }
        [JsonProperty(Order = 2)]
        public string id { get; set; }
        [JsonProperty(Order = 3)]
        public BundleMeta meta { get; set; }
        [JsonProperty(Order = 4)]
        public bool active { get; set; }

        [JsonProperty(Order = 5)]
        public string type { get; set; }
        [JsonProperty(Order = 6)]
        public List<Link> link { get; set; }
        [JsonProperty(Order = 7)]
        public List<OrganizationEntry> entry { get; set; }
    }

    public class PractitionerBundle
    {
        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }
        [JsonProperty(Order = 2)]
        public string id { get; set; }
        [JsonProperty(Order = 3)]
        public BundleMeta meta { get; set; }
        [JsonProperty(Order = 4)]
        public bool active { get; set; }

        [JsonProperty(Order = 5)]
        public string type { get; set; }
        [JsonProperty(Order = 6)]
        public List<Link> link { get; set; }
        [JsonProperty(Order = 7)]
        public List<PractitionerEntry> entry { get; set; }
    }

    public class ImmunizationBundle
    {
        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }
        [JsonProperty(Order = 2)]
        public string id { get; set; }
        [JsonProperty(Order = 3)]
        public BundleMeta meta { get; set; }
        [JsonProperty(Order = 4)]
        public bool active { get; set; }

        [JsonProperty(Order = 5)]
        public string type { get; set; }
        [JsonProperty(Order = 6)]
        public List<Link> link { get; set; }
        [JsonProperty(Order = 7)]
        public List<ImmunizationEntry> entry { get; set; }
    }

    public class EncounterBundle
    {
        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }
        [JsonProperty(Order = 2)]
        public string id { get; set; }
        [JsonProperty(Order = 3)]
        public BundleMeta meta { get; set; }
        [JsonProperty(Order = 4)]
        public bool active { get; set; }

        [JsonProperty(Order = 5)]
        public string type { get; set; }
        [JsonProperty(Order = 6)]
        public List<Link> link { get; set; }
        [JsonProperty(Order = 7)]
        public List<EncounterEntry> entry { get; set; }
    }

    public class DiagnosticReportBundle
    {
        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }
        [JsonProperty(Order = 2)]
        public string id { get; set; }
        [JsonProperty(Order = 3)]
        public BundleMeta meta { get; set; }
        [JsonProperty(Order = 4)]
        public bool active { get; set; }

        [JsonProperty(Order = 5)]
        public string type { get; set; }
        [JsonProperty(Order = 6)]
        public List<Link> link { get; set; }
        [JsonProperty(Order = 7)]
        public List<DiagnosticReportEntry> entry { get; set; }
    }

    public class ObservationBundle
    {
        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }
        [JsonProperty(Order = 2)]
        public string id { get; set; }
        [JsonProperty(Order = 3)]
        public BundleMeta meta { get; set; }
        [JsonProperty(Order = 4)]
        public bool active { get; set; }

        [JsonProperty(Order = 5)]
        public string type { get; set; }
        [JsonProperty(Order = 6)]
        public List<Link> link { get; set; }
        [JsonProperty(Order = 7)]
        public List<ObservationEntry> entry { get; set; }
    }

    public class ProcedureBundle
    {
        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }
        [JsonProperty(Order = 2)]
        public string id { get; set; }
        [JsonProperty(Order = 3)]
        public BundleMeta meta { get; set; }
        [JsonProperty(Order = 4)]
        public bool active { get; set; }

        [JsonProperty(Order = 5)]
        public string type { get; set; }
        [JsonProperty(Order = 6)]
        public List<Link> link { get; set; }
        [JsonProperty(Order = 7)]
        public List<ProcedureEntry> entry { get; set; }
    }

    public class AllergyIntoleranceBundle
    {
        [JsonProperty(Order = 1)]
        public string resourceType { get; set; }
        [JsonProperty(Order = 2)]
        public string id { get; set; }
        [JsonProperty(Order = 3)]
        public BundleMeta meta { get; set; }
        [JsonProperty(Order = 4)]
        public bool active { get; set; }

        [JsonProperty(Order = 5)]
        public string type { get; set; }
        [JsonProperty(Order = 6)]
        public List<Link> link { get; set; }
        [JsonProperty(Order = 7)]
        public List<AllergyIntoleranceEntry> entry { get; set; }
    }

    public class Extension2
    {
        public string url { get; set; }
        public string valueUri { get; set; }
    }

    public class Extension
    {
        public List<Extension2> extension { get; set; }
        public string url { get; set; }
    }



    public class Service
    {
        public List<Coding> coding { get; set; }
    }

    public class Security
    {
        public List<Extension> extension { get; set; }
        public bool cors { get; set; }
        public List<Service> service { get; set; }
    }

    public class Interaction
    {
        public string code { get; set; }
        public string documentation { get; set; }
    }

    public class SearchParam
    {
        public string name { get; set; }
        public string type { get; set; }
        public string documentation { get; set; }
    }

    public class Resource
    {
        public string type { get; set; }
        public List<Interaction> interaction { get; set; }
        public string versioning { get; set; }
        public bool readHistory { get; set; }
        public bool updateCreate { get; set; }
        public List<SearchParam> searchParam { get; set; }
    }

    public class Rest
    {
        public string mode { get; set; }
        public Security security { get; set; }
        public List<Resource> resource { get; set; }
    }

    public class CapabilityStatement
    {
        public string resourceType { get; set; }
        public string version { get; set; }
        public string name { get; set; }
        public bool experimental { get; set; }
        public string date { get; set; }
        public string publisher { get; set; }
        public string description { get; set; }
        public string kind { get; set; }
        public string fhirVersion { get; set; }
        public List<string> format { get; set; }
        public List<Rest> rest { get; set; }
    }


}
