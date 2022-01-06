using Common;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class PatientRepositoryEFCore : IPatientRepository
    {
        private readonly identitywithpgsqlContext _context;

        public PatientRepositoryEFCore(identitywithpgsqlContext context)
        {
            _context = context;
        }

        public async Task<IList<PatientDto>> GetListAsync()
        {
            var listFromEF =  await _context.Patient.ToListAsync();
            var list = new List<PatientDto>();

            foreach (var pat in listFromEF)
            {
                list.Add(new PatientDto { 
                    Address = pat.Address.First(),
                    ID = pat.Id,
                    Name = pat.Name.First(),
                    Uhid = pat.Uhid,
                    LastModified = pat.LastModified
                });
            }

            return list;
        }

        public async Task<long> AddAsync(PatientDto item)
        {
            _context.Add(new Patient { 
                Address = new string[1] { item.Address },
                Name = new string[1] { item.Name },
                Uhid = Guid.NewGuid(),
                LastModified = DateTime.Now
            });

            return await _context.SaveChangesAsync();
        }

        
        public async Task<long> RemoveWithIDAsync(long ID)
        {
            var patient = await _context.Patient.FindAsync(ID);
            _context.Patient.Remove(patient);
            return await _context.SaveChangesAsync();
        }

        public async Task<long> UpdateAsync(PatientDto item)
        {
            _context.Update(new Patient { 
                Address = new string[1] {item.Address},
                Name = new string[1] { item.Name },
                Id = item.ID,
                Uhid = item.Uhid,
                LastModified = DateTime.Now
            });
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> IsExists(long id)
        {
            return await _context.Patient.AnyAsync(e => e.Id == id);
        }

        public async Task<PatientDto> FindAsync(long id)
        {
            var patient = await _context.Patient
               .FirstOrDefaultAsync(m => m.Id == id);

            if (patient == null)
            {
                return null;
            }

            return new PatientDto {Address = patient.Address.First(),
                                    Name = patient.Name.First(),
                                        ID = patient.Id,
                                    Uhid = patient.Uhid,
            LastModified = patient.LastModified};
        }

        public async Task<long> RemoveAsync(PatientDto patient)
        {
            var patientObj = await _context.Patient.FindAsync(patient.ID);
            _context.Patient.Remove(patientObj);
            return await _context.SaveChangesAsync();
        }
    }
}
