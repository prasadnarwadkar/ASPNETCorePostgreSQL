using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETIdentityPostgres.Areas.ADT.Models
{
    /// <summary>
    /// Alfresco Activiti REST API utility.
    /// </summary>
    public static class AlfrescoActivitiRestApiInvoker
    {
        public static Uri ActivitiRestUri { get; internal set; }

        public static void SetActivitiRestUri(Uri restUri)
        {
            ActivitiRestUri = restUri;
        }

        /// <summary>
        /// Get all the process instances.
        /// kermit/kermit
        /// </summary>
        public static async Task<AlfrescoActivitiProcessInstanceList> GetProcessInstances(Uri baseUri,
                                                string username,
                                                string password)
        {
            var url = new Uri(baseUri, "runtime/process-instances");

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

            var list = JsonConvert.DeserializeObject<AlfrescoActivitiProcessInstanceList>(response.Content, settings);

            return list;
        }

        public static async Task<List<ActivitiProcessInstanceVar>> GetProcessInstanceVars(Uri baseUri,
                                                string processInstanceId,
                                                string username,
                                                string password)
        {
            var url = new Uri(baseUri, "runtime/process-instances/" + processInstanceId + "/variables");

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

            var list = JsonConvert.DeserializeObject<List<ActivitiProcessInstanceVar>>(response.Content, settings);

            return list;
        }

        public static async Task<MemoryStream> GetProcessInstanceDiagram(Uri baseUri,
                                                string processInstanceId,
                                                string username,
                                                string password)
        {
            var url = new Uri(baseUri, "runtime/process-instances/" + processInstanceId + "/diagram");

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

            var stream = new MemoryStream(response.RawBytes);

            return stream;
        }

        /// <summary>
        /// Get tasks by process instance id.
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <param name="baseUri"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task<AlfrescoActivitiTaskList> GetTaskInstances(int processInstanceID,
                                                Uri baseUri,
                                                string username,
                                                string password)
        {
            var url = new Uri(baseUri, "runtime/tasks");

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

            var list = JsonConvert.DeserializeObject<AlfrescoActivitiTaskList>(response.Content, settings);

            if (list.data == null)
            {
                return null;
            }

            list.data = list.data.Where(d => d.processInstanceId == processInstanceID.ToString()).ToList();

            return list;
        }

        /// <summary>
        /// Start a process instance. kermit/kermit
        /// Implement a similar class for every process.
        /// This must be done because each process is different and has different 
        /// process def key, parameters and vars.
        /// </summary>
        /// <param name="processDefID"></param>
        /// <param name="baseUri"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task<AlfrescoActivitiProcessInstance> CreateAProcessInstance(AlfrescoActivitiProcessInstancePostData data,
                                                string processDefID,
                                                Uri baseUri,
                                                string username,
                                                string password)
        {
            var url = new Uri(baseUri, "runtime/process-instances");

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

            data.employee = null;

            request.AddParameter("application/json", JsonConvert.SerializeObject(new AlfrescoActivitiProcessInstancePostData
            {
                processDefinitionKey = processDefID,
                variables = data.variables
            }, settings), ParameterType.RequestBody);

            IRestResponse response = await client.ExecuteAsync(request);

            return JsonConvert.DeserializeObject<AlfrescoActivitiProcessInstance>(response.Content, settings);
        }

        /// <summary>
        /// Complete a task (kermit/kermit)
        /// </summary>
        /// <param name="taskInstanceID"></param>
        /// <param name="baseUri"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="taskVars"></param>
        /// <returns></returns>
        public static async Task<int> CompleteATask(string taskInstanceID,
                                                Uri baseUri,
                                                string username,
                                                string password,
                                                AlfrescoTaskCompletionVars taskVars)
        {
            var url = new Uri(baseUri, "runtime/tasks/"  + taskInstanceID);

            taskVars.taskid = null;
            taskVars.taskname = null;
            taskVars.processInstanceId = null;

            var client = new RestClient(url);

            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var data = JsonConvert.SerializeObject(taskVars, settings);

            request.AddParameter("application/json", data, ParameterType.RequestBody);

            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password)));

            IRestResponse response = await client.ExecuteAsync(request);

            return (int)response.StatusCode;
        }

        
    }
}
