using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETIdentityPostgres.Areas.ADT.Models
{
    /// <summary>
    /// jBPM REST API utility.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Get all the process instances by container id.
        /// jBPM has containers for a process. wbadmin/wbadmin
        /// </summary>
        /// <param name="containerID"></param>
        public static async Task<ProcessInstanceList> GetProcessInstances(string containerID,
                                                Uri baseUri,
                                                string username,
                                                string password)
        {
            var url = new Uri(baseUri, containerID + "/processes/instances?page=0&pageSize=10&sortOrder=true");

            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password)));

            IRestResponse response = await client.ExecuteAsync(request);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var list = JsonConvert.DeserializeObject<ProcessInstanceList>(response.Content, settings);

            return list;
        }

        public static async Task<jBPMTaskList> GetTaskInstances(int processInstanceID,
                                                Uri baseUri,
                                                string username,
                                                string password)
        {
            var url = new Uri(baseUri, "kie-server/services/rest/server/queries/tasks/instances/process/" + processInstanceID + 
                                        "?page=0&pageSize=10&sortOrder=true");

            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password)));

            IRestResponse response = await client.ExecuteAsync(request);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var list = JsonConvert.DeserializeObject<jBPMTaskList>(response.Content, settings);

            return list;
        }

        /// <summary>
        /// Start a process instance. wbadmin/wbadmin
        /// </summary>
        /// <param name="containerID"></param>
        /// <param name="processDefID"></param>
        /// <param name="baseUri"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="initiator"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        public static async Task<int> CreateAProcessInstance(string containerID,
                                                string processDefID,
                                                Uri baseUri,
                                                string username,
                                                string password,
                                                string initiator,
                                                string employee)
        {
            var url = new Uri(baseUri, containerID + "/processes/" + processDefID + "/instances");

            var client = new RestClient(url);

            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password)));

            request.AddHeader("Content-Type", "application/json");

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            request.AddParameter("application/json", JsonConvert.SerializeObject(new EmployeeEvalStartProcessData
            {
                employee = employee,
                initiator = initiator
            }, settings), ParameterType.RequestBody);

            IRestResponse response = await client.ExecuteAsync(request);
            int processInstanceId;
            int.TryParse(response.Content, out processInstanceId);
            return processInstanceId;
        }

        /// <summary>
        /// Claim a task (for PM/HR eval: wbadmin/wbadmin)
        /// </summary>
        /// <param name="containerID"></param>
        /// <param name="taskInstanceID"></param>
        /// <param name="baseUri"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task<int> ClaimATask(string containerID,
                                                int taskInstanceID,
                                                Uri baseUri,
                                                string username,
                                                string password)
        {
            var url = new Uri(baseUri, containerID + "/tasks/" + taskInstanceID + "/states/claimed");

            var client = new RestClient(url);

            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password)));

            IRestResponse response = await client.ExecuteAsync(request);

            return (int)response.StatusCode;
        }

        /// <summary>
        /// Start a task (for PM/HR eval: wbadmin/wbadmin)
        /// </summary>
        /// <param name="containerID"></param>
        /// <param name="taskInstanceID"></param>
        /// <param name="baseUri"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task<int> StartATask(string containerID,
                                                int taskInstanceID,
                                                Uri baseUri,
                                                string username,
                                                string password)
        {
            var url = new Uri(baseUri, containerID + "/tasks/" + taskInstanceID + "/states/started");

            var client = new RestClient(url);

            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password)));

            IRestResponse response = await client.ExecuteAsync(request);

            return (int)response.StatusCode;
        }

        /// <summary>
        /// Complete a task (for PM/HR eval: wbadmin/wbadmin)
        /// </summary>
        /// <param name="containerID"></param>
        /// <param name="taskInstanceID"></param>
        /// <param name="baseUri"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="processVars"></param>
        /// <returns></returns>
        public static async Task<int> CompleteATask(string containerID,
                                                int taskInstanceID,
                                                Uri baseUri,
                                                string username,
                                                string password,
                                                EmployeeEvalStartProcessData processVars)
        {
            var url = new Uri(baseUri, containerID + "/tasks/" + taskInstanceID + "/states/completed");

            var client = new RestClient(url);

            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);

            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var data = JsonConvert.SerializeObject(processVars, settings);

            request.AddParameter("application/json", data, ParameterType.RequestBody);

            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password)));

            IRestResponse response = await client.ExecuteAsync(request);

            return (int)response.StatusCode;
        }

        /// <summary>
        /// Get process instance vars. wbadmin/wbadmin
        /// </summary>
        /// <param name="containerID"></param>
        /// <param name="taskInstanceID"></param>
        /// <param name="baseUri"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="processVars"></param>
        /// <returns></returns>
        public static async Task<EmployeeEvalStartProcessData> GetProcessInstanceVars(string containerID,
                                                int procInstanceID,
                                                Uri baseUri,
                                                string username,
                                                string password)
        {
            var url = new Uri(baseUri, containerID + "/processes/instances/" + procInstanceID + "/variables");

            var client = new RestClient(url);

            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("accept", "application/json");

            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password)));

            IRestResponse response = await client.ExecuteAsync(request);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return JsonConvert.DeserializeObject<EmployeeEvalStartProcessData>(response.Content, settings);
        }
    }
}
