using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETIdentityPostgres.Areas.ADT.Models
{
    public class BonitaSoftClaim
    {
        public claimInput claimInput { get; set; }
    }

    public class claimInput
    {
        public string description { get; set; }
        public string answer { get; set; }

        public int satisfactionLevel { get; set; }
    }

    public class assignedid
    {
        public string assigned_id { get; set; }
    }

    public class BonitaSoftProcessDesc
    {
        public string displayDescription { get; set; }
        public string deploymentDate { get; set; }
        public string displayName { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string deployedBy { get; set; }
        public string id { get; set; }
        public string activationState { get; set; }
        public string version { get; set; }
        public string configurationState { get; set; }
        public string last_update_date { get; set; }
        public string actorinitiatorid { get; set; }
    }

    public class BonitaSoftTaskDesc
    {
        public string displayDescription { get; set; }
        public string executedBy { get; set; }
        public BonitaSoftProcessDesc rootContainerId { get; set; }
        public string assigned_date { get; set; }
        public string displayName { get; set; }
        public string executedBySubstitute { get; set; }
        public string dueDate { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string priority { get; set; }
        public string actorId { get; set; }
        public string processId { get; set; }
        public string caseId { get; set; }
        public string name { get; set; }
        public string reached_state_date { get; set; }
        public string rootCaseId { get; set; }
        public string id { get; set; }
        public string state { get; set; }
        public string parentCaseId { get; set; }
        public string last_update_date { get; set; }
        public string assigned_id { get; set; }
    }

    public class claimRef
    {
        public string name { get; set; }
        public string type { get; set; }
        public string link { get; set; }
        public int storageId { get; set; }
        public string storageId_string { get; set; }
    }

    public class claimDetails
    {
        public int persistenceId { get; set; }
        public string persistenceId_string { get; set; }
        public int persistenceVersion { get; set; }
        public string persistenceVersion_string { get; set; }
        public string description { get; set; }
        public string answer { get; set; }
        public int satisfactionLevel { get; set; }
    }

    /// <summary>
    /// Example of mapping between BPM user and Web app user.
    /// </summary>
    public class mapping
    {
        public string UserNameOnWebApp { get; set; }
        public string UserIdOnBPMEngineOrApp { get; set; }
        public string UserRoleOnBPMEngineOrApp { get; set; }
        public string UserNameOnBPMEngineOrApp { get; internal set; }
        public string PasswordOnBPMEngineOrApp { get; internal set; }

        public string UserIdOnBPMEngineOrApp2 { get; set; }
        public string UserRoleOnBPMEngineOrApp2 { get; set; }
        public string UserNameOnBPMEngineOrApp2 { get; internal set; }
        public string PasswordOnBPMEngineOrApp2 { get; internal set; }
    }

    public static class WebAppAndBPMUserMap
    {
        public static List<mapping> mappings { get; }
        static WebAppAndBPMUserMap()
        {
            mappings = new List<mapping>();
            mappings.Clear();

            // These are example mappings. Please edit for your own
            // env. 
            mappings.Add(new mapping { 
                UserNameOnWebApp = "a@b.com",
                UserIdOnBPMEngineOrApp = "4",
                UserRoleOnBPMEngineOrApp = "claim_initiator",
                UserNameOnBPMEngineOrApp = "walter.bates",
                PasswordOnBPMEngineOrApp = "bpm",
                UserIdOnBPMEngineOrApp2 = "",
                UserRoleOnBPMEngineOrApp2 = "invoice_initiator",
                UserNameOnBPMEngineOrApp2 = "demo",
                PasswordOnBPMEngineOrApp2 = "demo",
            });

            mappings.Add(new mapping
            {
                UserNameOnWebApp = "p@q.com",
                UserIdOnBPMEngineOrApp = "15",
                UserRoleOnBPMEngineOrApp = "claim_reviewer",
                UserNameOnBPMEngineOrApp = "mauro.zetticci",
                PasswordOnBPMEngineOrApp = "bpm",
                UserIdOnBPMEngineOrApp2 = "",
                UserRoleOnBPMEngineOrApp2 = "invoice_approver",
                UserNameOnBPMEngineOrApp2 = "demo",
                PasswordOnBPMEngineOrApp2 = "demo",
            });
        }
    }

    public class CamundaUserTask
    {
        public string id { get; set; }
        public string name { get; set; }
        public object assignee { get; set; }
        public DateTime created { get; set; }
        public DateTime due { get; set; }
        public object followUp { get; set; }
        public object delegationState { get; set; }
        public string description { get; set; }
        public string executionId { get; set; }
        public object owner { get; set; }
        public object parentTaskId { get; set; }
        public int priority { get; set; }
        public string processDefinitionId { get; set; }
        public string processInstanceId { get; set; }
        public string taskDefinitionKey { get; set; }
        public object caseExecutionId { get; set; }
        public object caseInstanceId { get; set; }
        public object caseDefinitionId { get; set; }
        public bool suspended { get; set; }
        public string formKey { get; set; }
        public object camundaFormRef { get; set; }
        public object tenantId { get; set; }
    }

    

    public class Link
    {
        public string method { get; set; }
        public string href { get; set; }
        public string rel { get; set; }
    }

    /// <summary>
    /// Camunda process instance. This is created when the start event of a process
    /// is triggered by a user or the system.
    /// </summary>
    public class ProcessInstance
    {
        public List<Link> links { get; set; }
        public string id { get; set; }
        public string definitionId { get; set; }
        public string businessKey { get; set; }
        public object caseInstanceId { get; set; }
        public object tenantId { get; set; }
        public bool ended { get; set; }
        public bool suspended { get; set; }
    }

    public class Amount
    {
        public int value { get; set; }
        public string type { get; set; }
    }

    public class Creditor
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class InvoiceCategory
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class InvoiceNumber
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class Variables
    {
        public Amount amount { get; set; }
        public Creditor creditor { get; set; }
        public InvoiceCategory invoiceCategory { get; set; }
        public InvoiceNumber invoiceNumber { get; set; }
    }

    public class variables
    {
        public approved approved { get; set; }
    }

    public class emptyJson
    { }

    public class approved
    {
        public bool value { get; set; }
    }

    public class ApproveInvoicePost
    {
        public variables variables { get; set; }
        public bool withVariablesInReturn { get; set; }

        public string taskId { get; set; }
    }

    public class InvoiceDetails
    {
        public Variables variables { get; set; }
        public string businessKey { get; set; }
        public bool withVariablesInReturn { get; set; }
    }

    public class ReviewAndAnswerTask
    {
        public string description { get; set; }
        public string answer { get; set; }
        public int satisfactionLevel { get; set; }

        public string caseId { get; set; }

        public string TaskId { get; set; }
    }

    public class claimSubmittedInfo
    {
        public string caseId { get; set; }
        public List<BonitaSoftTaskDesc> tasks { get; set; }
    }
}
