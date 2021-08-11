using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common
{
    public interface IUnnaturalDeathsLogic
    {
        Task<UnnaturalDeaths> FindAsync(Guid id);
        Task<long> InsertAsync(UnnaturalDeaths UnnaturalDeaths);
        Task<bool> UnnaturalDeathsExists(Guid id);
        Task<IList<UnnaturalDeaths>> UnnaturalDeathsListAsync();
        Task<long> RemoveAsync(UnnaturalDeaths UnnaturalDeaths);
        Task<long> UpdateAsync(UnnaturalDeaths UnnaturalDeaths);
    }
}