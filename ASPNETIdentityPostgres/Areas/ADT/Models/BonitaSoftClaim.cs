using Newtonsoft.Json;
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

    public class ValueInfo
    {
        public string filename { get; set; }
        public string mimeType { get; set; }
        public string objectTypeName { get; set; }
        public string serializationDataFormat { get; set; }
    }

    public class CamundaProcessVar
    {
        public string type { get; set; }
        public object value { get; set; }
        public ValueInfo valueInfo { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string processDefinitionId { get; set; }
        public string processInstanceId { get; set; }
        public string executionId { get; set; }
        public object caseInstanceId { get; set; }
        public object caseExecutionId { get; set; }
        public object taskId { get; set; }
        public object batchId { get; set; }
        public string activityInstanceId { get; set; }
        public object errorMessage { get; set; }
        public object tenantId { get; set; }
    }


    public class EmployeeEvalStartProcessData
    {
        public string initiator { get; set; }
        public string employee { get; set; }

        public string performance { get; set; }

        public string reason { get; set; }
    }

    public class StartDate
    {
        [JsonProperty("java.util.Date")]
        public object JavaUtilDate { get; set; }
    }

    public class processinstance
    {
        [JsonProperty("process-instance-id")]
        public int ProcessInstanceId { get; set; }

        [JsonProperty("process-id")]
        public string ProcessId { get; set; }

        [JsonProperty("process-name")]
        public string ProcessName { get; set; }

        [JsonProperty("process-version")]
        public string ProcessVersion { get; set; }

        [JsonProperty("process-instance-state")]
        public int ProcessInstanceState { get; set; }

        [JsonProperty("container-id")]
        public string ContainerId { get; set; }
        public string initiator { get; set; }

        [JsonProperty("start-date")]
        public StartDate StartDate { get; set; }

        [JsonProperty("process-instance-desc")]
        public string ProcessInstanceDesc { get; set; }

        [JsonProperty("correlation-key")]
        public string CorrelationKey { get; set; }

        [JsonProperty("parent-instance-id")]
        public int ParentInstanceId { get; set; }

        [JsonProperty("sla-compliance")]
        public int SlaCompliance { get; set; }

        [JsonProperty("sla-due-date")]
        public object SlaDueDate { get; set; }

        [JsonProperty("active-user-tasks")]
        public object ActiveUserTasks { get; set; }

        [JsonProperty("process-instance-variables")]
        public object ProcessInstanceVariables { get; set; }
    }

    public class ProcessInstanceList
    {
        [JsonProperty("process-instance")]
        public List<processinstance> ProcessInstance { get; set; }
    }

    public class TaskCreatedOn
    {
        [JsonProperty("java.util.Date")]
        public long JavaUtilDate { get; set; }
    }

    public class TaskActivationTime
    {
        [JsonProperty("java.util.Date")]
        public long JavaUtilDate { get; set; }
    }

    public class jBPMSelfEvalTaskData
    {
        public int taskInstanceID { get; set; }

        public EmployeeEvalStartProcessData processData { get; set; }

        public string TaskStatus { get; set; }

        public string TaskOwner { get; set; }
    }


    public class TaskSummary
    {
        [JsonProperty("task-id")]
        public int TaskId { get; set; }

        [JsonProperty("task-name")]
        public string TaskName { get; set; }

        [JsonProperty("task-subject")]
        public string TaskSubject { get; set; }

        [JsonProperty("task-description")]
        public string TaskDescription { get; set; }

        [JsonProperty("task-status")]
        public string TaskStatus { get; set; }

        [JsonProperty("task-priority")]
        public int TaskPriority { get; set; }

        [JsonProperty("task-is-skipable")]
        public bool TaskIsSkipable { get; set; }

        [JsonProperty("task-actual-owner")]
        public string TaskActualOwner { get; set; }

        [JsonProperty("task-created-by")]
        public object TaskCreatedBy { get; set; }

        [JsonProperty("task-created-on")]
        public TaskCreatedOn TaskCreatedOn { get; set; }

        [JsonProperty("task-activation-time")]
        public TaskActivationTime TaskActivationTime { get; set; }

        [JsonProperty("task-expiration-time")]
        public object TaskExpirationTime { get; set; }

        [JsonProperty("task-proc-inst-id")]
        public int TaskProcInstId { get; set; }

        [JsonProperty("task-proc-def-id")]
        public string TaskProcDefId { get; set; }

        [JsonProperty("task-container-id")]
        public string TaskContainerId { get; set; }

        [JsonProperty("task-parent-id")]
        public int TaskParentId { get; set; }

        [JsonProperty("correlation-key")]
        public string CorrelationKey { get; set; }

        [JsonProperty("process-type")]
        public int ProcessType { get; set; }
    }

    public class jBPMTaskList
    {
        [JsonProperty("task-summary")]
        public List<TaskSummary> TaskSummary { get; set; }
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
        public string UserIdOnjBPMEngineOrApp { get; internal set; }
        public string UserRoleOnjBPMEngineOrApp { get; internal set; }
        public string UserNameOnjBPMEngineOrApp { get; internal set; }
        public string PasswordOnjBPMEngineOrApp { get; internal set; }
    }

    /// <summary>
    /// Normally, these should be stored in db.
    /// </summary>
    public static class BPMUsers
    {
        public static Dictionary<string, string> Users { get; }

        static BPMUsers()
        {
            Users = new Dictionary<string, string>();
            Users.Add("katy", "katy");
            Users.Add("john", "john");
            //Users.Add("wbadmin", "wbadmin");
            Users.Add("jack", "jack");
        }
    }

    public static class Constants
    {
        public static string jBPMEvalProcessContainerID = "evaluation_1.0";
        public static string jBPMEvalProcessID = "evaluation";
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
            mappings.Add(new mapping
            {
                UserNameOnWebApp = "jack@example.com",
                UserIdOnBPMEngineOrApp = "4",
                UserRoleOnBPMEngineOrApp = "claim_initiator",
                UserNameOnBPMEngineOrApp = "walter.bates",
                PasswordOnBPMEngineOrApp = "bpm",
                UserIdOnBPMEngineOrApp2 = "",
                UserRoleOnBPMEngineOrApp2 = "invoice_initiator",
                UserNameOnBPMEngineOrApp2 = "demo",
                PasswordOnBPMEngineOrApp2 = "demo",
                UserIdOnjBPMEngineOrApp = "",
                UserRoleOnjBPMEngineOrApp = "employee",
                UserNameOnjBPMEngineOrApp = "jack",
                PasswordOnjBPMEngineOrApp = "jack",
            });

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
                UserIdOnjBPMEngineOrApp = "",
                UserRoleOnjBPMEngineOrApp = "pm",
                UserNameOnjBPMEngineOrApp = "john",
                PasswordOnjBPMEngineOrApp = "john",
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
                UserIdOnjBPMEngineOrApp = "",
                UserRoleOnjBPMEngineOrApp = "hr_admin",
                UserNameOnjBPMEngineOrApp = "wbadmin",
                PasswordOnjBPMEngineOrApp = "wbadmin",
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
