using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Common;

namespace ASPNETIdentityPostgres.Areas.ADT.Views
{
    [Area("ADT")]
    [Authorize]
    public class UnnaturaldeathsController : Controller
    {
        private readonly IUnnaturalDeathsLogic _unnaturalDeathsBusinessLogic;

        public UnnaturaldeathsController(IUnnaturalDeathsLogic logic)
        {
            _unnaturalDeathsBusinessLogic = logic;
        }

        // GET: UnnaturalDeaths
        public async Task<IActionResult> Index()
        {
            return View(await _unnaturalDeathsBusinessLogic.UnnaturalDeathsListAsync());
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

        // POST: UnnaturalDeaths/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Address,Age,CidOrPassport,DateOfPostmortemExamination," +
                                                       "DeceasedName,Dzongkhag,GeneralExternalInformation," +
                                                        "History,ImformantCidNo,InformantName,InformantRelationToDeceased," +
                                                        "Isactive,Lastchanged,Nationality,PlaceOfExamination," +
                                                        "PoliceCaseNo,PoliceStation,Remark," +
                                                        "SceneOfDeath,Sex,TimeOfPostmortemExamination")] Unnaturaldeaths death)
        {


            if (ModelState.IsValid)
            {
                await _unnaturalDeathsBusinessLogic.InsertAsync(new UnnaturalDeathsDto
                {
                    Address = death.Address,
                    Age = death.Age,
                    CidOrPassport = death.CidOrPassport,
                    DateOfPostmortemExamination = death.DateOfPostmortemExamination,
                    DeceasedName = death.DeceasedName,
                    Dzongkhag = death.Dzongkhag,
                    GeneralExternalInformation = death.GeneralExternalInformation,
                    History = death.History,
                    ImformantCidNo = death.ImformantCidNo,
                    InformantName = death.InformantName,
                    InformantRelationToDeceased = death.InformantRelationToDeceased,
                    Isactive = death.Isactive,
                    Lastchanged = death.Lastchanged,
                    Nationality = death.Nationality,
                    PlaceOfExamination = death.PlaceOfExamination,
                    PoliceCaseNo = death.PoliceCaseNo,
                    PoliceStation = death.PoliceStation,
                    Remark = death.Remark,
                    SceneOfDeath = death.SceneOfDeath,
                    Sex = death.Sex.ToString(),
                    TimeOfPostmortemExamination = death.TimeOfPostmortemExamination,
                    Transactedby = death.Transactedby,
                    Transacteddate = death.Transacteddate,
                    Version = death.Version
                });

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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Address,Age,CidOrPassport,DateOfPostmortemExamination," +
                                                       "DeceasedName,Dzongkhag,GeneralExternalInformation," +
                                                        "History,ImformantCidNo,InformantName,InformantRelationToDeceased," +
                                                        "Isactive,Lastchanged,Nationality,PlaceOfExamination," +
                                                        "PoliceCaseNo,PoliceStation,Remark," +
                                                        "SceneOfDeath,Sex,TimeOfPostmortemExamination")] Unnaturaldeaths death)
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
                    await _unnaturalDeathsBusinessLogic.UpdateAsync(new UnnaturalDeathsDto
                    {
                        Address = death.Address,
                        Age = death.Age,
                        CidOrPassport = death.CidOrPassport,
                        DateOfPostmortemExamination = death.DateOfPostmortemExamination,
                        DeceasedName = death.DeceasedName,
                        Dzongkhag = death.Dzongkhag,
                        GeneralExternalInformation = death.GeneralExternalInformation,
                        History = death.History,
                        ImformantCidNo = death.ImformantCidNo,
                        InformantName = death.InformantName,
                        InformantRelationToDeceased = death.InformantRelationToDeceased,
                        Isactive = death.Isactive,
                        Lastchanged = death.Lastchanged,
                        Nationality = death.Nationality,
                        PlaceOfExamination = death.PlaceOfExamination,
                        PoliceCaseNo = death.PoliceCaseNo,
                        PoliceStation = death.PoliceStation,
                        Remark = death.Remark,
                        SceneOfDeath = death.SceneOfDeath,
                        Sex = death.Sex.ToString(),
                        TimeOfPostmortemExamination = death.TimeOfPostmortemExamination,
                        Transactedby = death.Transactedby,
                        Transacteddate = death.Transacteddate,
                        Version = death.Version,
                        Id = death.Id
                    });
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

        // GET: UnnaturalDeaths/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
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

        // POST: UnnaturalDeaths/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var death = await _unnaturalDeathsBusinessLogic.FindAsync(id);
            await _unnaturalDeathsBusinessLogic.RemoveAsync(new UnnaturalDeathsDto
            {
                Address = death.Address,
                Age = death.Age,
                CidOrPassport = death.CidOrPassport,
                DateOfPostmortemExamination = death.DateOfPostmortemExamination,
                DeceasedName = death.DeceasedName,
                Dzongkhag = death.Dzongkhag,
                GeneralExternalInformation = death.GeneralExternalInformation,
                History = death.History,
                ImformantCidNo = death.ImformantCidNo,
                InformantName = death.InformantName,
                InformantRelationToDeceased = death.InformantRelationToDeceased,
                Isactive = death.Isactive,
                Lastchanged = death.Lastchanged,
                Nationality = death.Nationality,
                PlaceOfExamination = death.PlaceOfExamination,
                PoliceCaseNo = death.PoliceCaseNo,
                PoliceStation = death.PoliceStation,
                Remark = death.Remark,
                SceneOfDeath = death.SceneOfDeath,
                Sex = death.Sex,
                TimeOfPostmortemExamination = death.TimeOfPostmortemExamination,
                Transactedby = death.Transactedby,
                Transacteddate = death.Transacteddate,
                Version = death.Version
            });

            return RedirectToAction(nameof(Index), "UnnaturalDeaths");
        }

        private async Task<bool> UnnaturalDeathExists(Guid id)
        {
            return await _unnaturalDeathsBusinessLogic.UnnaturalDeathsExists(id);
        }
    }
}
