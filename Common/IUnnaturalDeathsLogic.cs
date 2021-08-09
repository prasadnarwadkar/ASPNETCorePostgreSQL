using Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common
{
    public interface IUnnaturalDeathsLogic
    {
        Task<UnnaturalDeathsDto> FindAsync(Guid id);
        Task<long> InsertAsync(UnnaturalDeathsDto UnnaturalDeaths);
        Task<bool> UnnaturalDeathsExists(Guid id);
        Task<IList<UnnaturalDeathsDto>> UnnaturalDeathsListAsync();
        Task<long> RemoveAsync(UnnaturalDeathsDto UnnaturalDeaths);
        Task<long> UpdateAsync(UnnaturalDeathsDto UnnaturalDeaths);
    }
}