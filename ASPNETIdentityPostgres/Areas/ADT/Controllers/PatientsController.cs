using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Common.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System;
using datasyncservice.Services;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using ASPNETIdentityPostgres.Areas.ADT.Models;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace ASPNETIdentityPostgres
{
    [Area("ADT")]
    [Authorize]
    /// In this class, some URLs to BonitaSoft BPM engine are used and are hardcoded. Please specify them in appSettings.json
    /// Refer to: https://documentation.bonitasoft.com/bonita/2021.2/getting-started/draw-bpmn-diagram
    /// Download and install the BonitaSoft studio. Create a simple claims management workflow and run the 
    /// process. BonitaSoft doesn't expose a conventional REST API. However, if you play with the web app that opens when you run a process from the 
    /// studio, you can use the web app to understand how HTTP-based APIs can be used to play with the BPM process. 
    /// I have shown below an example where a process can be started using REST API from an ASP.NET Core MVC web app (this app).
    public class PatientsController : Controller
    {

        private readonly IPatientLogic _patientBusinessLogic;
        private readonly PatientSyncService _HostedService;

        private static IEnumerable<Cookie> _cookies_bonitasoft = new List<Cookie>();
        private string processDefKey = "invoice:3:d8ca49d6-8b21-11ec-aa22-0a0027000007";

        IConfiguration _iconfiguration;
        private static Uri BonitaBaseUri;
        private static Uri BonitaPortalResourceBaseUri;
        private static Uri CamundaRestBaseUri;
        private static Uri jBPMRestBaseUri;
        private static Uri jBPMRestUri;
        private static string usernameBPM = "";
        private static string passwordBPM = "";


        static PatientsController()
        {

        }

        public PatientsController(IPatientLogic logic,
            datasyncservice.Services.BackgroundService hostedService,
            IConfiguration configuration)
        {
            _patientBusinessLogic = logic;
            _HostedService = hostedService as PatientSyncService;
            _iconfiguration = configuration;
            BonitaBaseUri = new Uri(_iconfiguration["BonitaBaseUri"]);
            BonitaPortalResourceBaseUri = new Uri(_iconfiguration["BonitaPortalResourceBaseUri"]);
            CamundaRestBaseUri = new Uri(_iconfiguration["CamundaRestBaseUri"]);
            jBPMRestBaseUri = new Uri(_iconfiguration["jBPMRestBaseUri"]);
            jBPMRestUri = new Uri(_iconfiguration["jBPMRestUri"]);



            getCookies("walter.bates", "bpm").Wait(2000);

            // Call background service.
            Task.WaitAll(_HostedService.StartAsync(new System.Threading.CancellationToken()));
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            return View(await _patientBusinessLogic.PatientListAsync());
        }

        public async Task<IActionResult> MyTaskList()
        {
            var tasks = await getUserTasks();

            if (_cookies_bonitasoft.Count() == 0)
            {
                ViewData["NoConnError"] = "No connection to BonitaSoft Engine. Is the BonitaSoft Engine running?";
            }

            var claimSubmittedInfo = new claimSubmittedInfo();
            claimSubmittedInfo.caseId = "";
            claimSubmittedInfo.tasks = tasks;

            return View(claimSubmittedInfo);
        }

        /// <summary>
        /// Get all the running process instances for invoice receipt process.
        /// Each instance represents one invoice.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> MyInvoices()
        {
            var invoices = await getInvoices(processDefKey);

            return View(invoices);
        }

        public async Task<IActionResult> MyEmpEvals()
        {
            var username = User.Identity.Name;

            var mapping = ASPNETIdentityPostgres.Areas.ADT.Models.WebAppAndBPMUserMap.mappings.FirstOrDefault(m => m.UserNameOnWebApp.ToLower() == username.ToLower());

            if (mapping != null)
            {
                usernameBPM = mapping.UserNameOnjBPMEngineOrApp;
                passwordBPM = mapping.PasswordOnjBPMEngineOrApp;
            }

            var list = await Utility.GetProcessInstances(Constants.jBPMEvalProcessContainerID, jBPMRestBaseUri, usernameBPM, passwordBPM);

            return View(list.ProcessInstance);
        }

        private async Task getCookies(string username, string password)
        {
            try
            {
                CookieContainer cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = cookies;

                HttpClient client = new HttpClient(handler);
                var url = new Uri(BonitaBaseUri, "loginservice").ToString();

                var dict = new Dictionary<string, string>();
                dict.Add("username", username);
                dict.Add("password", password);

                var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(dict) };
                var res = await client.SendAsync(req);

                Uri uri = new Uri(BonitaBaseUri, "loginservice");
                IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();

                _cookies_bonitasoft = responseCookies;
                ViewData["NoConnError"] = "";
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("no connection"))
                {
                    ViewData["NoConnError"] = "No connection to BonitaSoft Engine. Is the BonitaSoft Engine running?";
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> LoginToBonitaSoftEngine()
        {
            // Get the list of BonitaSoft processes.
            var url = new Uri(BonitaBaseUri, "API/bpm/process?c=10&p=0").ToString();
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                foreach (var cookie in _cookies_bonitasoft)
                {
                    cookieContainer.Add(new Uri(url), cookie);
                }

                var result = await client.GetAsync("");
                var content = await result.Content.ReadAsStringAsync();
                result.EnsureSuccessStatusCode();
            }

            return View("Index", await _patientBusinessLogic.PatientListAsync());
        }


        public async Task<int> getClaimRefFromReviewAndAnswerTaskContext(string taskId)
        {
            var url = new Uri(BonitaPortalResourceBaseUri, "taskInstance/ClaimsManagement/1.0/Review%20and%20answer%20claim/API/bpm/userTask/" +
                                                            taskId + "/context").ToString();

            CookieContainer cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                foreach (var cookie in _cookies_bonitasoft)
                {
                    cookieContainer.Add(new Uri(url), cookie);
                }

                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", _cookies_bonitasoft.Where(ck => ck.Name == "X-Bonita-API-Token").FirstOrDefault().Value);

                var result = await client.GetAsync("");
                result.EnsureSuccessStatusCode();

                var content = await result.Content.ReadAsStringAsync();

                return (JsonConvert.DeserializeObject<claimRef>(((JProperty)((JContainer)(JsonConvert.DeserializeObject<dynamic>(content))).First).Value.ToString())).storageId;
            }
        }

        public async Task<int> getClaimRefFromReadTheAnswerAndRateItTaskContext(string taskId)
        {
            var url = new Uri(BonitaPortalResourceBaseUri, "taskInstance/ClaimsManagement/1.0/Read%20the%20answer%20and%20rate%20it/API/bpm/userTask/" +
                      taskId + "/context").ToString();

            CookieContainer cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                foreach (var cookie in _cookies_bonitasoft)
                {
                    cookieContainer.Add(new Uri(url), cookie);
                }

                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", _cookies_bonitasoft.Where(ck => ck.Name == "X-Bonita-API-Token").FirstOrDefault().Value);

                var result = await client.GetAsync("");
                result.EnsureSuccessStatusCode();

                var content = await result.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                return (JsonConvert.DeserializeObject<claimRef>(((JProperty)((JContainer)(JsonConvert.DeserializeObject<dynamic>(content))).First).Value.ToString())).storageId;
            }
        }

        public async Task<claimDetails> getClaimDetailsFromClaimRef(int claimID)
        {
            var url = new Uri(BonitaPortalResourceBaseUri, "taskInstance/ClaimsManagement/1.0/Review%20and%20answer%20claim/API/bdm/businessData/com.company.model.Claim/" + claimID).ToString();

            CookieContainer cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                foreach (var cookie in _cookies_bonitasoft)
                {
                    cookieContainer.Add(new Uri(url), cookie);
                }

                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", _cookies_bonitasoft.Where(ck => ck.Name == "X-Bonita-API-Token").FirstOrDefault().Value);

                var result = await client.GetAsync("");
                result.EnsureSuccessStatusCode();

                var content = await result.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                return JsonConvert.DeserializeObject<claimDetails>(content, settings);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claimID"></param>
        /// <returns></returns>
        public async Task<claimDetails> getClaimDetailsFromClaimRef2(int claimID)
        {
            var url = new Uri(BonitaPortalResourceBaseUri, "taskInstance/ClaimsManagement/1.0/Read%20the%20answer%20and%20rate%20it/API/bdm/businessData/com.company.model.Claim/" + claimID).ToString();

            CookieContainer cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                foreach (var cookie in _cookies_bonitasoft)
                {
                    cookieContainer.Add(new Uri(url), cookie);
                }

                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", _cookies_bonitasoft.Where(ck => ck.Name == "X-Bonita-API-Token").FirstOrDefault().Value);

                var result = await client.GetAsync("");
                result.EnsureSuccessStatusCode();

                var content = await result.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                return JsonConvert.DeserializeObject<claimDetails>(content, settings);
            }
        }

        public async Task<IActionResult> BonitaSoftTaskDetails(string id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var tasks = await getUserTasks();
            int claimId = 0;

            foreach (var taskObj in tasks)
            {
                if (taskObj.name.StartsWith("Read the answer and rate it"))
                {
                    claimId = await getClaimRefFromReadTheAnswerAndRateItTaskContext(id);

                    var claimDetails = await getClaimDetailsFromClaimRef2(claimId);

                    ReviewAndAnswerTask task = new ReviewAndAnswerTask();
                    task.TaskId = id;
                    // Claim reviewer user id in BonitaSoft BPM engine/app. Map this to the web app user appropriately for production.
                    // In the POC, this is hardcoded.

                    task.caseId = tasks.FirstOrDefault(t => t.id == id).caseId;

                    task.description = claimDetails.description;
                    task.answer = claimDetails.answer;

                    return View("ReadTheAnswerAndRateIt", task);
                }
                else if (taskObj.name.StartsWith("Review and answer claim"))
                {
                    claimId = await getClaimRefFromReviewAndAnswerTaskContext(id);

                    var claimDetails = await getClaimDetailsFromClaimRef(claimId);

                    ReviewAndAnswerTask task = new ReviewAndAnswerTask();
                    task.TaskId = id;
                    // Claim reviewer user id in BonitaSoft BPM engine/app. Map this to the web app user appropriately for production.
                    // In the POC, this is hardcoded.

                    task.caseId = tasks.FirstOrDefault(t => t.id == id).caseId;

                    task.description = claimDetails.description;

                    return View("ReviewAndAnswerClaim", task);
                }
            }

            return View();
        }

        /// <summary>
        /// Camunda Invoice (process instance) details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> CamundaInvoiceDetails(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoices = new List<ProcessInstance>();

            var tasks = await getCamundaUserTasks(id);
            var username = User.Identity.Name;

            var bpmRole = "";

            var mapping = ASPNETIdentityPostgres.Areas.ADT.Models.WebAppAndBPMUserMap.mappings.FirstOrDefault(m => m.UserNameOnWebApp.ToLower() == username.ToLower());

            if (mapping != null)
            {
                bpmRole = mapping.UserRoleOnBPMEngineOrApp2;
            }

            ViewData["notapprovermsg"] = "";

            foreach (var taskObj in tasks)
            {
                if (taskObj.taskDefinitionKey.Equals("approveInvoice"))
                {

                    if (bpmRole == "invoice_approver")
                    {
                        var post = new ApproveInvoicePost();
                        post.taskId = taskObj.id;

                        return View("ApproveInvoiceCamunda", post);
                    }
                    else
                    {
                        ViewData["notapprovermsg"] = "Please login as approver to approve invoices";
                        invoices = await getInvoices(processDefKey);

                        return View("MyInvoices", invoices);
                    }
                }
                else if (taskObj.taskDefinitionKey.Equals("prepareBankTransfer"))
                {
                    if (bpmRole == "invoice_approver")
                    {
                        var result = await prepareBankTransferForAnInvoice(taskObj.id);

                        invoices = await getInvoices(processDefKey);

                        return View("MyInvoices", invoices);
                    }
                    else
                    {
                        invoices = await getInvoices(processDefKey);

                        return View("MyInvoices", invoices);
                    }
                }
            }

            invoices = await getInvoices(processDefKey);

            return View("MyInvoices", invoices);
        }

        public async Task<IActionResult> jBPMProcInstDetails(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = new ProcessInstanceList();

            var invoices = new List<ProcessInstance>();

            var tasks = await getjBPMUserTasks(id);
            var username = User.Identity.Name;

            var bpmRole = "";

            var mapping = ASPNETIdentityPostgres.Areas.ADT.Models.WebAppAndBPMUserMap.mappings.FirstOrDefault(m => m.UserNameOnWebApp.ToLower() == username.ToLower());

            if (mapping != null)
            {
                bpmRole = mapping.UserRoleOnjBPMEngineOrApp;
            }

            ViewData["notapprovermsg"] = "";

            foreach (var taskObj in tasks.TaskSummary)
            {
                if (taskObj.TaskName.Equals("Self Evaluation"))
                {
                    if (bpmRole == "employee")
                    {
                        var data = await Utility.GetProcessInstanceVars(Constants.jBPMEvalProcessContainerID,
                                        taskObj.TaskProcInstId,
                                        jBPMRestBaseUri,
                                        usernameBPM,
                                        passwordBPM);

                        var dataForView = new jBPMSelfEvalTaskData();
                        dataForView.processData = data;
                        dataForView.TaskStatus = taskObj.TaskStatus;
                        
                        dataForView.taskInstanceID = taskObj.TaskId;
                        dataForView.TaskOwner = taskObj.TaskActualOwner;

                        return View("jBPMSelfEval", dataForView);
                    }
                    else
                    {
                        ViewData["notapprovermsg"] = "Please login as an employee (jack@example.com) to evaluate yourself.";
                        list = await Utility.GetProcessInstances(Constants.jBPMEvalProcessContainerID,
                                                                jBPMRestBaseUri,
                                                                usernameBPM,
                                                                passwordBPM);

                        return View("MyEmpEvals", list.ProcessInstance);
                    }
                }
                else if (taskObj.TaskName.Equals("PM Evaluation"))
                {
                    if (bpmRole == "pm")
                    {
                        var data = await Utility.GetProcessInstanceVars(Constants.jBPMEvalProcessContainerID,
                                        taskObj.TaskProcInstId,
                                        jBPMRestBaseUri,
                                        usernameBPM,
                                        passwordBPM);

                        var dataForView = new jBPMSelfEvalTaskData();
                        dataForView.processData = data;

                        dataForView.taskInstanceID = taskObj.TaskId;

                        return View("jBPMPMEval", dataForView);
                    }
                    else
                    {
                        ViewData["notapprovermsg"] = "Please login as a PM (a@b.com) to evaluate an employee.";
                        list = await Utility.GetProcessInstances(Constants.jBPMEvalProcessContainerID,
                                                                jBPMRestBaseUri,
                                                                usernameBPM,
                                                                passwordBPM);

                        return View("MyEmpEvals", list.ProcessInstance);
                    }
                }
                else if (taskObj.TaskName.Equals("HR Evaluation"))
                {
                    if (bpmRole == "hr_admin")
                    {
                        var data = await Utility.GetProcessInstanceVars(Constants.jBPMEvalProcessContainerID,
                                        taskObj.TaskProcInstId,
                                        jBPMRestBaseUri,
                                        usernameBPM,
                                        passwordBPM);

                        var dataForView = new jBPMSelfEvalTaskData();
                        dataForView.processData = data;

                        dataForView.taskInstanceID = taskObj.TaskId;

                        return View("jBPMHREval", dataForView);
                    }
                    else
                    {
                        ViewData["notapprovermsg"] = "Please login as an HR (p@q.com) to evaluate an employee.";
                        list = await Utility.GetProcessInstances(Constants.jBPMEvalProcessContainerID,
                                                                jBPMRestBaseUri,
                                                                usernameBPM,
                                                                passwordBPM);

                        return View("MyEmpEvals", list.ProcessInstance);
                    }
                }

            }

            list = await Utility.GetProcessInstances(Constants.jBPMEvalProcessContainerID,
                                                                jBPMRestBaseUri,
                                                                usernameBPM,
                                                                passwordBPM);

            return View("MyEmpEvals", list.ProcessInstance);
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _patientBusinessLogic.FindAsync(id.GetValueOrDefault());

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult SubmitClaimBonitaSoft()
        {
            return View();
        }

        public IActionResult SubmitInvoiceCamunda()
        {
            return View();
        }

        private async Task<string> submitClaim(string processId, BonitaSoftClaim claim)
        {
            var url = new Uri(BonitaPortalResourceBaseUri, "process/ClaimsManagement/1.0/API/bpm/process/" + processId + "/instantiation").ToString();

            CookieContainer cookieContainer = new CookieContainer();

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                foreach (var cookie in _cookies_bonitasoft)
                {
                    cookieContainer.Add(new Uri(url), cookie);
                }

                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", _cookies_bonitasoft.Where(ck => ck.Name == "X-Bonita-API-Token").FirstOrDefault().Value);

                var claimJSON = JsonContent.Create<BonitaSoftClaim>(claim);
                var result = await client.PostAsync("", claimJSON);
                result.EnsureSuccessStatusCode();
                var content = await result.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<JsonElement>(content).GetProperty("caseId").GetRawText();
            }
        }

        /// <summary>
        /// This is API equivalent of taking (claiming) a task to finish it on the
        /// BonitaSoft Web App (User application).
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<bool> takeTheTask(string taskId,
                                                          string username,
                                                          string password,
                                                          string userId)
        {
            var url = new Uri(BonitaPortalResourceBaseUri, "app/userAppBonita/task-list/API/bpm/humanTask/" + taskId).ToString();

            CookieContainer cookieContainer = new CookieContainer();

            await getCookies(username, password);

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                foreach (var cookie in _cookies_bonitasoft)
                {
                    cookieContainer.Add(new Uri(url), cookie);
                }

                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", _cookies_bonitasoft.Where(ck => ck.Name == "X-Bonita-API-Token").FirstOrDefault().Value);

                var assigned = new assignedid();
                // Reviewer of claim. This needs to be mapped from BonitaSoft BPM users to web app users.
                assigned.assigned_id = userId;

                var claimJSON = JsonContent.Create<assignedid>(assigned);
                var result = await client.PutAsync("", claimJSON);
                result.EnsureSuccessStatusCode();

                return true;
            }
        }

        private async Task<bool> submitSatisfactionLevel(string taskId, ReviewAndAnswerTask task)
        {
            var url = new Uri(BonitaPortalResourceBaseUri, "taskInstance/ClaimsManagement/1.0/Read%20the%20answer%20and%20rate%20it/API/bpm/userTask/" +
                taskId + "/execution").ToString();

            CookieContainer cookieContainer = new CookieContainer();

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                foreach (var cookie in _cookies_bonitasoft)
                {
                    cookieContainer.Add(new Uri(url), cookie);
                }

                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", _cookies_bonitasoft.Where(ck => ck.Name == "X-Bonita-API-Token").FirstOrDefault().Value);

                var claimInput = new BonitaSoftClaim();
                claimInput.claimInput = new claimInput();
                claimInput.claimInput.answer = task.answer;
                claimInput.claimInput.description = task.description;
                claimInput.claimInput.satisfactionLevel = task.satisfactionLevel;

                var claimJSON = JsonContent.Create<BonitaSoftClaim>(claimInput);
                var result = await client.PostAsync("", claimJSON);
                result.EnsureSuccessStatusCode();

                return true;
            }
        }


        private async Task<bool> submitAnswerToClaim(string taskId, ReviewAndAnswerTask task)
        {
            var url = new Uri(BonitaPortalResourceBaseUri, "taskInstance/ClaimsManagement/1.0/Review%20and%20answer%20claim/API/bpm/userTask/" +
                taskId + "/execution").ToString();

            CookieContainer cookieContainer = new CookieContainer();

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                foreach (var cookie in _cookies_bonitasoft)
                {
                    cookieContainer.Add(new Uri(url), cookie);
                }

                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", _cookies_bonitasoft.Where(ck => ck.Name == "X-Bonita-API-Token").FirstOrDefault().Value);

                var claimInput = new BonitaSoftClaim();
                claimInput.claimInput = new claimInput();
                claimInput.claimInput.answer = task.answer;
                claimInput.claimInput.description = task.description;

                var claimJSON = JsonContent.Create<BonitaSoftClaim>(claimInput);
                var result = await client.PostAsync("", claimJSON);
                result.EnsureSuccessStatusCode();

                return true;
            }
        }


        private async Task<List<ProcessInstance>> getInvoices(string processDefKey)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var url = new Uri(CamundaRestBaseUri, "process-instance?processDefinitionId=" + processDefKey + "&active=false&suspended=false&withIncident=false&withoutTenantId=false&processDefinitionWithoutTenantId=false&rootProcessInstances=false&leafProcessInstances=false&variableNamesIgnoreCase=false&variableValuesIgnoreCase=false").ToString();

            HttpResponseMessage result = new HttpResponseMessage();

            using (var handler = new HttpClientHandler() { })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                try
                {
                    result = await client.GetAsync("");

                    result.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToLower().Contains("no connection"))
                    {
                        ViewData["NoConnError"] = "No connection to Camunda Engine. Is the Camunda Engine running?";
                    }
                    else if (ex.Message.ToLower().Contains("not found"))
                    {
                        ViewData["NoConnError"] = "No connection to Camunda Engine. Is the Camunda Engine running? OR the process definition you are referring to doesn't exist.";
                    }

                    return JsonConvert.DeserializeObject<List<ProcessInstance>>("[]", settings);
                }                

                var content = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<ProcessInstance>>(content, settings);
            }
        }

        private async Task<List<BonitaSoftTaskDesc>> getUserTasks()
        {
            var url = new Uri(BonitaPortalResourceBaseUri, "app/userAppBonita/task-list/API/bpm/humanTask?c=50&d=rootContainerId&f=state%3Dready&o=displayName+ASC&p=0").ToString();

            CookieContainer cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                foreach (var cookie in _cookies_bonitasoft)
                {
                    cookieContainer.Add(new Uri(url), cookie);
                }

                if (_cookies_bonitasoft.Count() == 0)
                {
                    return new List<BonitaSoftTaskDesc>();
                }

                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", _cookies_bonitasoft.Where(ck => ck.Name == "X-Bonita-API-Token").FirstOrDefault().Value);

                var result = await client.GetAsync("");

                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await getCookies("walter.bates", "bpm");

                    cookieContainer = new CookieContainer();

                    foreach (var cookie in _cookies_bonitasoft)
                    {
                        cookieContainer.Add(new Uri(url), cookie);
                    }

                    result = await client.GetAsync("");
                }


                result.EnsureSuccessStatusCode();

                var content = await result.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                return JsonConvert.DeserializeObject<List<BonitaSoftTaskDesc>>(content, settings);
            }
        }

        private async Task<List<CamundaUserTask>> getCamundaUserTasks(string processInstanceId)
        {
            var url = new Uri(CamundaRestBaseUri, "task?processInstanceId=" + processInstanceId + "&withoutTenantId=false&includeAssignedTasks=false&assigned=false&unassigned=false&withoutDueDate=false&withCandidateGroups=false&withoutCandidateGroups=false&withCandidateUsers=false&withoutCandidateUsers=false&active=false&suspended=false&variableNamesIgnoreCase=false&variableValuesIgnoreCase=false").ToString();

            using (var handler = new HttpClientHandler() { })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                var result = await client.GetAsync("");

                result.EnsureSuccessStatusCode();

                var content = await result.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                return JsonConvert.DeserializeObject<List<CamundaUserTask>>(content, settings);
            }
        }

        private async Task<jBPMTaskList> getjBPMUserTasks(string processInstanceId)
        {
            int id;
            var parsed = int.TryParse(processInstanceId, out id);
            var usernameBPM = "";
            var passwordBPM = "";

            ViewData["Title"] = "My Invoices";

            var username = User.Identity.Name;

            var mapping = ASPNETIdentityPostgres.Areas.ADT.Models.WebAppAndBPMUserMap.mappings.FirstOrDefault(m => m.UserNameOnWebApp.ToLower() == username.ToLower());

            if (mapping != null)
            {
                usernameBPM = mapping.UserNameOnjBPMEngineOrApp;
                passwordBPM = mapping.PasswordOnjBPMEngineOrApp;
            }

            if (parsed)
            {
                return await Utility.GetTaskInstances(id, jBPMRestUri, usernameBPM, passwordBPM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Approve an invoice.
        /// Creates a process instance (one instance of a process). Many instances can 
        /// be created, each catering to one instance of that process/work flow.
        /// </summary>
        /// <returns></returns>
        private async Task<HttpResponseMessage> approveInvoice(ApproveInvoicePost invoice)
        {
            invoice.withVariablesInReturn = true;

            var url = new Uri(CamundaRestBaseUri,
                    "task/" + invoice.taskId + "/complete").ToString();

            using (var handler = new HttpClientHandler() { })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                var settings = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };

                invoice.taskId = null;

                var post = JsonContent.Create<ApproveInvoicePost>(invoice, null, settings);

                var result = await client.PostAsync("", post);

                result.EnsureSuccessStatusCode();

                return result;
            }
        }

        /// <summary>
        /// Prepare bank transfer for an invoice.
        /// Creates a process instance (one instance of a process). Many instances can 
        /// be created, each catering to one instance of that process/work flow.
        /// </summary>
        /// <returns></returns>
        private async Task<HttpResponseMessage> prepareBankTransferForAnInvoice(string taskid)
        {
            var url = new Uri(CamundaRestBaseUri,
                    "task/" + taskid + "/complete").ToString();

            using (var handler = new HttpClientHandler() { })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                var post = JsonContent.Create<emptyJson>(new emptyJson());

                var result = await client.PostAsync("", post);

                result.EnsureSuccessStatusCode();

                return result;
            }
        }

        /// <summary>
        /// Submit an invoice.
        /// Creates a process instance (one instance of a process). Many instances can 
        /// be created, each catering to one instance of that process/work flow.
        /// </summary>
        /// <returns></returns>
        private async Task<ProcessInstance> submitInvoice(InvoiceDetails invoiceDetails)
        {
            invoiceDetails.withVariablesInReturn = true;

            var url = new Uri(CamundaRestBaseUri,
                    "process-definition/key/invoice/start").ToString();

            using (var handler = new HttpClientHandler() { })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                var options = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };

                var post = JsonContent.Create<InvoiceDetails>(invoiceDetails, null, options);

                var result = await client.PostAsync("", post);
                var content = await result.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                var processInstance = JsonConvert.DeserializeObject<ProcessInstance>(content, settings);

                result.EnsureSuccessStatusCode();

                return processInstance;
            }
        }

        public IActionResult jBPMStartEmpVal()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> jBPMStartEmpVal(EmployeeEvalStartProcessData data)
        {
            if (data.employee?.Length > 0)
            {
                var result = await Utility.CreateAProcessInstance(Constants.jBPMEvalProcessContainerID,
                                                                    Constants.jBPMEvalProcessID,
                                                                    jBPMRestBaseUri,
                                                                    usernameBPM,
                                                                    passwordBPM,
                                                                    data.initiator,
                                                                    data.employee);

                if (result > 0)
                {
                    var list = await Utility.GetProcessInstances(Constants.jBPMEvalProcessContainerID,
                                                                    jBPMRestBaseUri,
                                                                    usernameBPM,
                                                                    passwordBPM);

                    return View("MyEmpEvals", list.ProcessInstance);
                }
                else
                {
                    return View(data);
                }
            }
            else
            {
                return View(data);
            }
        }

        // POST: Patients/SubmitInvoiceCamunda
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitInvoiceCamunda(InvoiceDetails invoice)
        {
            var result = await submitInvoice(invoice);

            var invoices = await getInvoices(processDefKey);

            return View("MyInvoices", invoices);
        }

        // POST: Patients/ApproveInvoiceCamunda
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveInvoiceCamunda(ApproveInvoicePost invoice)
        {
            var result = await approveInvoice(invoice);

            var invoices = await getInvoices(processDefKey);

            return View("MyInvoices", invoices);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> jBPMSelfEval(jBPMSelfEvalTaskData data)
        {
            var result = 200;

            if (data.TaskStatus != "Reserved")
            {
                if (!string.IsNullOrWhiteSpace(data.TaskOwner)
                    && BPMUsers.Users.ContainsKey(data.TaskOwner))
                {
                    result = await Utility.ClaimATask(Constants.jBPMEvalProcessContainerID,
                                                                data.taskInstanceID,
                                                                jBPMRestBaseUri,
                                                                data.TaskOwner,
                                                                BPMUsers.Users[data.TaskOwner]);
                }
                else
                {
                    result = await Utility.ClaimATask(Constants.jBPMEvalProcessContainerID,
                                                            data.taskInstanceID,
                                                            jBPMRestBaseUri,
                                                            usernameBPM,
                                                            passwordBPM);
                }
            }

            if (result >= 200 && result <= 201)
            {
                if (!string.IsNullOrWhiteSpace(data.TaskOwner)
                    && BPMUsers.Users.ContainsKey(data.TaskOwner))
                {
                    result = await Utility.StartATask(Constants.jBPMEvalProcessContainerID,
                                                    data.taskInstanceID,
                                                    jBPMRestBaseUri,
                                                    data.TaskOwner,
                                                    BPMUsers.Users[data.TaskOwner]);
                }
                else
                {
                    result = await Utility.StartATask(Constants.jBPMEvalProcessContainerID,
                                                    data.taskInstanceID,
                                                    jBPMRestBaseUri,
                                                    usernameBPM,
                                                    passwordBPM);
                }

                if (result >= 200 && result <= 201)
                {
                    if (!string.IsNullOrWhiteSpace(data.TaskOwner)
                    && BPMUsers.Users.ContainsKey(data.TaskOwner))
                    {
                        result = await Utility.CompleteATask(Constants.jBPMEvalProcessContainerID,
                                                        data.taskInstanceID,
                                                        jBPMRestBaseUri,
                                                        data.TaskOwner,
                                                        BPMUsers.Users[data.TaskOwner]
,                                                       data.processData);
                    }
                    else
                    {
                        result = await Utility.CompleteATask(Constants.jBPMEvalProcessContainerID,
                                                        data.taskInstanceID,
                                                        jBPMRestBaseUri,
                                                        usernameBPM,
                                                        passwordBPM,
                                                        data.processData);
                    }

                    if (result >= 200 && result <= 201)
                    {
                        var list = await Utility.GetProcessInstances(Constants.jBPMEvalProcessContainerID, jBPMRestBaseUri, usernameBPM, passwordBPM);

                        return View("MyEmpEvals", list.ProcessInstance);
                    }
                }
            }

            return View("jBPMSelfEval", data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> jBPMPMEval(jBPMSelfEvalTaskData data)
        {
            var result = await Utility.ClaimATask(Constants.jBPMEvalProcessContainerID,
                                                    data.taskInstanceID,
                                                    jBPMRestBaseUri,
                                                    usernameBPM,
                                                    passwordBPM);

            if (result >= 200 && result <= 201)
            {
                result = await Utility.StartATask(Constants.jBPMEvalProcessContainerID,
                                                    data.taskInstanceID,
                                                    jBPMRestBaseUri,
                                                    usernameBPM,
                                                    passwordBPM);

                if (result >= 200 && result <= 201)
                {
                    result = await Utility.CompleteATask(Constants.jBPMEvalProcessContainerID,
                                                        data.taskInstanceID,
                                                        jBPMRestBaseUri,
                                                        usernameBPM,
                                                        passwordBPM,
                                                        data.processData);

                    if (result >= 200 && result <= 201)
                    {
                        var list = await Utility.GetProcessInstances(Constants.jBPMEvalProcessContainerID, jBPMRestBaseUri, usernameBPM, passwordBPM);

                        return View("MyEmpEvals", list.ProcessInstance);
                    }
                }
            }

            return View("jBPMPMEval", data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> jBPMHREval(jBPMSelfEvalTaskData data)
        {
            var result = await Utility.ClaimATask(Constants.jBPMEvalProcessContainerID,
                                                    data.taskInstanceID,
                                                    jBPMRestBaseUri,
                                                    usernameBPM,
                                                    passwordBPM);

            if (result >= 200 && result <= 201)
            {
                result = await Utility.StartATask(Constants.jBPMEvalProcessContainerID,
                                                    data.taskInstanceID,
                                                    jBPMRestBaseUri,
                                                    usernameBPM,
                                                    passwordBPM);

                if (result >= 200 && result <= 201)
                {

                    result = await Utility.CompleteATask(Constants.jBPMEvalProcessContainerID,
                                                        data.taskInstanceID,
                                                        jBPMRestBaseUri,
                                                        usernameBPM,
                                                        passwordBPM,
                                                        data.processData);

                    if (result >= 200 && result <= 201)
                    {
                        var list = await Utility.GetProcessInstances(Constants.jBPMEvalProcessContainerID, jBPMRestBaseUri, usernameBPM, passwordBPM);

                        return View("MyEmpEvals", list.ProcessInstance);
                    }
                }
            }

            return View("jBPMHREval", data);
        }

        // POST: Patients/SubmitClaimBonitaSoft
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitClaimBonitaSoft(BonitaSoftClaim claim)
        {
            if (ModelState.IsValid)
            {
                // Submit the claim using BonitaSoft API.    

                if (_cookies_bonitasoft.Count() == 0)
                {
                    await getCookies("walter.bates", "bpm");
                }

                var caseId = "";

                // Get all the processes. 
                var processes = new List<BonitaSoftProcessDesc>();

                // Our example claims management process.
                var claimsManagementProcess = new BonitaSoftProcessDesc();

                var url = new Uri(BonitaPortalResourceBaseUri,
                    "app/userAppBonita/case-list/API/bpm/process?c=9999&o=displayName%20ASC").ToString();

                var cookieContainer = new CookieContainer();
                using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
                using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
                {
                    foreach (var cookie in _cookies_bonitasoft)
                    {
                        cookieContainer.Add(new Uri(url), cookie);
                    }

                    client.DefaultRequestHeaders.Add("X-Bonita-API-Token", _cookies_bonitasoft.Where(ck => ck.Name == "X-Bonita-API-Token").FirstOrDefault().Value);

                    var result = await client.GetAsync("");
                    var content = await result.Content.ReadAsStringAsync();

                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    processes = JsonConvert.DeserializeObject<List<BonitaSoftProcessDesc>>(content, settings);
                    claimsManagementProcess = processes.FirstOrDefault(p => p.displayName.ToLower() == "claimsmanagement");

                    result.EnsureSuccessStatusCode();
                }

                caseId = await submitClaim(claimsManagementProcess?.id, claim);
                var tasks = await getUserTasks();

                var claimSubmittedInfo = new claimSubmittedInfo();
                claimSubmittedInfo.caseId = caseId;
                claimSubmittedInfo.tasks = tasks;

                return View("ClaimSubmittedToBonitaSoft", claimSubmittedInfo);
            }

            return View(claim);
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Address")] Patient patient)
        {
            

            if (ModelState.IsValid)
            {
                await _patientBusinessLogic.InsertAsync(new PatientDto { 
                    Address = patient.Address.First(),
                    Name = patient.Name.First(),
                    Uhid = Guid.NewGuid(),
                    LastModified = DateTime.Now
                });

                // Call background service.
                await _HostedService.StartAsync(new System.Threading.CancellationToken());

                return RedirectToAction(nameof(Index), "Patients");
            }
            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReviewAndAnswerClaim(ReviewAndAnswerTask task)
        {
            // Check the mapping.
            var mapping = WebAppAndBPMUserMap.mappings.FirstOrDefault(m => m.UserRoleOnBPMEngineOrApp == "claim_reviewer");

            // Take (claim) the task to complete it.
            var taken = await takeTheTask(task.TaskId,
                                                        mapping.UserNameOnBPMEngineOrApp, 
                                                        mapping.PasswordOnBPMEngineOrApp, 
                                                        mapping.UserIdOnBPMEngineOrApp);
            var result = false;

            if (taken)
            {
                result = await submitAnswerToClaim(task.TaskId, task);
            }

            if (result)
            {
                var tasks = await getUserTasks();

                var claimSubmittedInfo = new claimSubmittedInfo();

                if (tasks.Count() > 0)
                {
                    claimSubmittedInfo.caseId = tasks.FirstOrDefault().caseId;
                }
                
                claimSubmittedInfo.tasks = tasks;

                return View("ClaimSubmittedToBonitaSoft", claimSubmittedInfo);
            }
            else
            {
                return View(task);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReadTheAnswerAndRateIt(ReviewAndAnswerTask task)
        {
            // Check the mapping.
            var mapping = WebAppAndBPMUserMap.mappings.FirstOrDefault(m => m.UserRoleOnBPMEngineOrApp == "claim_initiator");

            // Take (claim) the task to complete it.
            var taken = await takeTheTask(task.TaskId,
                                                        mapping.UserNameOnBPMEngineOrApp,
                                                        mapping.PasswordOnBPMEngineOrApp,
                                                        mapping.UserIdOnBPMEngineOrApp);
            var result = false;

            if (taken)
            {
                result = await submitSatisfactionLevel(task.TaskId, task);
            }

            if (result)
            {
                var tasks = await getUserTasks();

                var claimSubmittedInfo = new claimSubmittedInfo();

                if (tasks.Count() > 0)
                {
                    claimSubmittedInfo.caseId = tasks.FirstOrDefault().caseId;
                }

                claimSubmittedInfo.tasks = tasks;

                return View("ClaimSubmittedToBonitaSoft", claimSubmittedInfo);
            }
            else
            {
                return View(task);
            }
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!await PatientExists(id.GetValueOrDefault()))
            {
                return NotFound();
            }

            var patient = await _patientBusinessLogic.FindAsync(id.GetValueOrDefault());
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Name,Id,Address, Uhid, LastModified")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (!await PatientExists(patient.Id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _patientBusinessLogic.UpdateAsync(new PatientDto { 
                        Address = patient.Address.First(),
                        ID = patient.Id,
                        Name = patient.Name.First(),
                        Uhid = patient.Uhid,
                        LastModified = DateTime.Now

                    });

                    // Call background service.
                    await _HostedService.StartAsync(new System.Threading.CancellationToken());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await PatientExists(patient.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Patients");
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _patientBusinessLogic.FindAsync(id.GetValueOrDefault());
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var patient = await _patientBusinessLogic.FindAsync(id);
            await _patientBusinessLogic.RemoveAsync(new PatientDto { Name = patient.Name,
                                                Address = patient.Address,
                                                ID = patient.ID,
                                                Uhid = patient.Uhid,
                                                LastModified = patient.LastModified});

            return RedirectToAction(nameof(Index), "Patients");
        }

        private async Task<bool> PatientExists(long id)
        {
            return await _patientBusinessLogic.PatientExists(id);
        }
    }
}
