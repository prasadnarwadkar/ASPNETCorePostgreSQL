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

        private static IEnumerable<Cookie> _cookies_bonitasoft= new List<Cookie>();

        IConfiguration _iconfiguration;
        private static Uri BonitaBaseUri;
        private static Uri BonitaPortalResourceBaseUri;

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

            var claimSubmittedInfo = new claimSubmittedInfo();
            claimSubmittedInfo.caseId = "";
            claimSubmittedInfo.tasks = tasks;

            return View(claimSubmittedInfo);
        }


        private static async Task getCookies(string username, string password)
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
            var url = new Uri(BonitaPortalResourceBaseUri, "taskInstance/ClaimsManagement/1.0/Review%20and%20answer%20claim/API/bdm/businessData/com.company.model.Claim/"+ claimID).ToString();

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
            var mapping = WebAppAndBPMUserMapForBonitaSoft.mappings.FirstOrDefault(m => m.UserRoleOnBPMEngineOrApp == "claim_reviewer");

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
            var mapping = WebAppAndBPMUserMapForBonitaSoft.mappings.FirstOrDefault(m => m.UserRoleOnBPMEngineOrApp == "claim_initiator");

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
