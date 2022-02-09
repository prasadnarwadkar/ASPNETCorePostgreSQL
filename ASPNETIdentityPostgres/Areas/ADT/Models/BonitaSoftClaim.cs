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
    }

    public static class WebAppAndBPMUserMapForBonitaSoft
    {
        public static List<mapping> mappings { get; }
        static WebAppAndBPMUserMapForBonitaSoft()
        {
            mappings = new List<mapping>();
            mappings.Clear();

            mappings.Add(new mapping { 
                UserNameOnWebApp = "a@b.com",
                UserIdOnBPMEngineOrApp = "4",
                UserRoleOnBPMEngineOrApp = "claim_initiator",
                UserNameOnBPMEngineOrApp = "walter.bates",
                PasswordOnBPMEngineOrApp = "bpm"
            });

            mappings.Add(new mapping
            {
                UserNameOnWebApp = "p@q.com",
                UserIdOnBPMEngineOrApp = "15",
                UserRoleOnBPMEngineOrApp = "claim_reviewer",
                UserNameOnBPMEngineOrApp = "mauro.zetticci",
                PasswordOnBPMEngineOrApp = "bpm"
            });
        }
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
