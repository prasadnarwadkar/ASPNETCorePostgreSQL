using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Simple CRUD operations.
    /// </summary>
    public interface IPatientRepository 
    {
        Task<IList<PatientDto>> GetListAsync();
        Task<long> AddAsync(PatientDto item);
        Task<long> RemoveWithIDAsync(long ID);
        Task<long> UpdateAsync(PatientDto item);

        Task<bool> IsExists(long id);

        Task<PatientDto> FindAsync(long id);

        Task<long> RemoveAsync(PatientDto patient);
    }
}
