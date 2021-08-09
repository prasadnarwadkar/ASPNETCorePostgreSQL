using Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class PatientLogic : IPatientLogic
    {
        private IPatientRepository _PatientRepository;
        // GET: Patients
        public async Task<IList<PatientDto>> PatientListAsync()
        {
            return await _PatientRepository.GetListAsync();
        }



        public async Task<PatientDto> FindAsync(long id)
        {
            return await _PatientRepository.FindAsync(id);
        }

        public PatientLogic(IPatientRepository repo)
        {
            _PatientRepository = repo;
        }

        public async Task<long> InsertAsync(PatientDto patient)
        {
            return await _PatientRepository.AddAsync(patient);
        }

        public async Task<long> RemoveAsync(PatientDto patient)
        {
            return await _PatientRepository.RemoveAsync(patient);
        }

        public async Task<long> UpdateAsync(PatientDto patient)
        {
            return await _PatientRepository.UpdateAsync(patient);
        }


        public async Task<bool> PatientExists(long id)
        {
            return await _PatientRepository.IsExists(id);
        }
    }
}
