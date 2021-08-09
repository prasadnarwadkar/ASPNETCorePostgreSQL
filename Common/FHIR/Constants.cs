using System;
using System.Collections.Generic;
using System.Text;

namespace Common.FHIR
{
    /// <summary>
    /// Magic Strings 
    /// </summary>
    public static class Constants
    {        
        public const string fhir_patient_resourcetype = "Patient";
        public const string fhir_practitioner_resourcetype = "Practitioner";
        public const string fhir_immunization_resourcetype = "Immunization";
        public const string fhir_observation_resourcetype = "Observation";
        public const string fhir_organization_resourcetype = "Organization";
        public const string fhir_procedure_resourcetype = "Procedure";
        public const string fhir_encounter_resourcetype = "Encounter";
        public const string fhir_diagnosticreport_resourcetype = "DiagnosticReport";
        public const string fhir_allergy_intolerance_resourcetype = "AllergyIntolerance";
        public const string fhir_identifier_resourcetype = "Identifier";
        public const string fhir_name_resourcetype = "HumanName";
        public const string fhir_address_resourcetype = "Address";
    }
}
