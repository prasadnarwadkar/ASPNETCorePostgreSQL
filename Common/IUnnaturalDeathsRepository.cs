using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Simple CRUD operations.
    /// </summary>
    public interface IUnnaturalDeathsRepository
    {
        Task<IList<UnnaturalDeathsDto>> GetListAsync();
        Task<long> AddAsync(UnnaturalDeathsDto item);
        Task<long> RemoveWithIDAsync(Guid ID);
        Task<long> UpdateAsync(UnnaturalDeathsDto item);

        Task<bool> IsExists(Guid id);

        Task<UnnaturalDeathsDto> FindAsync(Guid id);

        Task<long> RemoveAsync(UnnaturalDeathsDto patient);
    }
}
