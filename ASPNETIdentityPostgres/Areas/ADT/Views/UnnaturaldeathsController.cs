using Common;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace ASPNETIdentityPostgres.Areas.ADT.Views
{
    [Area("ADT")]
    [Authorize]
    public class UnnaturaldeathsController : Controller
    {
        private readonly IUnnaturalDeathsLogic _unnaturalDeathsBusinessLogic;

        private static string access_token = null;
        IConfiguration _iconfiguration;

        public string GatewayUriApi1 { get; private set; }

        static UnnaturaldeathsController()
        {
            access_token = getAccessToken();
        }

        public UnnaturaldeathsController(IUnnaturalDeathsLogic logic,
                                        IConfiguration configuration)
        {
            _unnaturalDeathsBusinessLogic = logic;
            _iconfiguration = configuration;
        }

        // GET: UnnaturalDeaths
        public async Task<IActionResult> Index()
        {
            GatewayUriApi1 = _iconfiguration["GatewayUriApi1"];
            var client = new RestClient(GatewayUriApi1);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", string.Format("Bearer {0}", access_token));
            IRestResponse response = await client.ExecuteAsync(request);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var list = JsonSerializer.Deserialize<List<UnnaturalDeaths>>(response.Content, options);
            return View(list);
        }

        // GET: UnnaturalDeaths/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unnaturalDeath = await _unnaturalDeathsBusinessLogic.FindAsync(id.GetValueOrDefault());

            if (unnaturalDeath == null)
            {
                return NotFound();
            }

            return View(unnaturalDeath);
        }

        // GET: UnnaturalDeaths/Create
        public IActionResult Create()
        {
            return View();
        }

        private static string getAccessToken()
        {
            var client = new RestClient("https://identityserverfhir.azurewebsites.net/connect/token");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "client_id=Wgh93ZfcM4mC5nD&client_secret=80H5Rn1N3XirYjs&grant_type=client_credentials&scope=fhir", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            
            return JsonSerializer.Deserialize<AccessToken>(response.Content).access_token;
        }

        // POST: UnnaturalDeaths/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Address,Age,CidOrPassport,DateOfPostmortemExamination," +
                                                       "DeceasedName,Dzongkhag,GeneralExternalInformation," +
                                                        "History,ImformantCidNo,InformantName,InformantRelationToDeceased," +
                                                        "Isactive,Lastchanged,Nationality,PlaceOfExamination," +
                                                        "PoliceCaseNo,PoliceStation,Remark," +
                                                        "SceneOfDeath,Sex,TimeOfPostmortemExamination")] UnnaturalDeaths death)
        {


            if (ModelState.IsValid)
            {
                death.Id = Guid.NewGuid();
                death.Lastchanged = DateTime.UtcNow;
                death.Isactive = true;
                death.Transactedby = User.Identity.Name;
                death.Transacteddate = DateTime.UtcNow;

                GatewayUriApi1 = _iconfiguration["GatewayUriApi1"];

                var postUriForUnnaturalDeaths = Flurl.Url.Combine(GatewayUriApi1, "post");

                var client = new RestClient(postUriForUnnaturalDeaths);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", string.Format("Bearer {0}", access_token));
                request.AddParameter("application/json", JsonSerializer.Serialize(death), ParameterType.RequestBody);
                
                IRestResponse response = await client.ExecuteAsync(request);
                
                return RedirectToAction(nameof(Index), "UnnaturalDeaths");
            }
            return View(death);
        }

        // GET: UnnaturalDeaths/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!await UnnaturalDeathExists(id.GetValueOrDefault()))
            {
                return NotFound();
            }

            var unnaturalDeath = await _unnaturalDeathsBusinessLogic.FindAsync(id.GetValueOrDefault());
            if (unnaturalDeath == null)
            {
                return NotFound();
            }
            return View(unnaturalDeath);
        }

        // POST: UnnaturalDeaths/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Address,Age,Id,CidOrPassport,DateOfPostmortemExamination," +
                                                       "DeceasedName,Dzongkhag,GeneralExternalInformation," +
                                                        "History,ImformantCidNo,InformantName,InformantRelationToDeceased," +
                                                        "Isactive,Lastchanged,Nationality,PlaceOfExamination," +
                                                        "PoliceCaseNo,PoliceStation,Remark," +
                                                        "SceneOfDeath,Sex,TimeOfPostmortemExamination")] UnnaturalDeaths death)
        {
            if (id != death.Id)
            {
                return NotFound();
            }

            if (!await UnnaturalDeathExists(death.Id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unnaturalDeathsBusinessLogic.UpdateAsync(death);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await UnnaturalDeathExists(death.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "UnnaturalDeaths");
            }
            return View(death);
        }

        //// GET: UnnaturalDeaths/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var unnaturalDeath = await _unnaturalDeathsBusinessLogic.FindAsync(id.GetValueOrDefault());
        //    if (unnaturalDeath == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(unnaturalDeath);
        //}

        //// POST: UnnaturalDeaths/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    var death = await _unnaturalDeathsBusinessLogic.FindAsync(id);
        //    await _unnaturalDeathsBusinessLogic.RemoveAsync(death);

        //    return RedirectToAction(nameof(Index), "UnnaturalDeaths");
        //}

        private async Task<bool> UnnaturalDeathExists(Guid id)
        {
            return await _unnaturalDeathsBusinessLogic.UnnaturalDeathsExists(id);
        }
    }
}
