using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common;
using DataAccessLayer.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ASPNETIdentityPostgres
{
    [Area("ADT")]
    [Authorize]
    public class PatientsController : Controller
    {
        
        private readonly IPatientLogic _patientBusinessLogic;

        public PatientsController(IPatientLogic logic)
        {
            _patientBusinessLogic = logic;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            return View(await _patientBusinessLogic.PatientListAsync());
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

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Address")] Patient patient)
        {
            

            if (ModelState.IsValid)
            {
                await _patientBusinessLogic.InsertAsync(new PatientDto { 
                    Address = patient.Address.First(),
                    Name = patient.Name.First()
                });

                return RedirectToAction(nameof(Index), "Patients");
            }
            return View(patient);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Name,Id,Address")] Patient patient)
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
                        Name = patient.Name.First()
                    });
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
                                                ID = patient.ID});

            return RedirectToAction(nameof(Index), "Patients");
        }

        private async Task<bool> PatientExists(long id)
        {
            return await _patientBusinessLogic.PatientExists(id);
        }
    }
}
