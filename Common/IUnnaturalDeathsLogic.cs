using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common
{
    public interface IUnnaturalDeathsLogic
    {
        Task<Unnaturaldeaths> FindAsync(Guid id);
        Task<long> InsertAsync(Unnaturaldeaths UnnaturalDeaths);
        Task<bool> UnnaturalDeathsExists(Guid id);
        Task<IList<Unnaturaldeaths>> UnnaturalDeathsListAsync();
        Task<long> RemoveAsync(Unnaturaldeaths UnnaturalDeaths);
        Task<long> UpdateAsync(Unnaturaldeaths UnnaturalDeaths);
    }
}