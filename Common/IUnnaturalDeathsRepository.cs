using Common.Models;
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
        Task<IList<UnnaturalDeaths>> GetListAsync();
        Task<long> AddAsync(UnnaturalDeaths item);
        Task<long> RemoveWithIDAsync(Guid ID);
        Task<long> UpdateAsync(UnnaturalDeaths item);

        Task<bool> IsExists(Guid id);

        Task<UnnaturalDeaths> FindAsync(Guid id);

        Task<long> RemoveAsync(UnnaturalDeaths patient);
    }
}
