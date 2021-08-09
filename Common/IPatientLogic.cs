using Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common
{
    public interface IPatientLogic
    {
        Task<PatientDto> FindAsync(long id);
        Task<long> InsertAsync(PatientDto patient);
        Task<bool> PatientExists(long id);
        Task<IList<PatientDto>> PatientListAsync();
        Task<long> RemoveAsync(PatientDto patient);
        Task<long> UpdateAsync(PatientDto patient);
    }
}