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
        Task<IList<Unnaturaldeaths>> GetListAsync();
        Task<long> AddAsync(Unnaturaldeaths item);
        Task<long> RemoveWithIDAsync(Guid ID);
        Task<long> UpdateAsync(Unnaturaldeaths item);

        Task<bool> IsExists(Guid id);

        Task<Unnaturaldeaths> FindAsync(Guid id);

        Task<long> RemoveAsync(Unnaturaldeaths patient);
    }
}
