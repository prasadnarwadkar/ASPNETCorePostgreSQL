using Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class UnnaturalDeathsLogic : IUnnaturalDeathsLogic
    {
        private IUnnaturalDeathsRepository _UnnaturalDeathsRepository;
        // GET: UnnaturalDeathss
        public async Task<IList<UnnaturalDeathsDto>> UnnaturalDeathsListAsync()
        {
            return await _UnnaturalDeathsRepository.GetListAsync();
        }



        public async Task<UnnaturalDeathsDto> FindAsync(Guid id)
        {
            return await _UnnaturalDeathsRepository.FindAsync(id);
        }

        public UnnaturalDeathsLogic(IUnnaturalDeathsRepository repo)
        {
            _UnnaturalDeathsRepository = repo;
        }

        public async Task<long> InsertAsync(UnnaturalDeathsDto UnnaturalDeaths)
        {
            return await _UnnaturalDeathsRepository.AddAsync(UnnaturalDeaths);
        }

        public async Task<long> RemoveAsync(UnnaturalDeathsDto UnnaturalDeaths)
        {
            return await _UnnaturalDeathsRepository.RemoveAsync(UnnaturalDeaths);
        }

        public async Task<long> UpdateAsync(UnnaturalDeathsDto UnnaturalDeaths)
        {
            return await _UnnaturalDeathsRepository.UpdateAsync(UnnaturalDeaths);
        }


        public async Task<bool> UnnaturalDeathsExists(Guid id)
        {
            return await _UnnaturalDeathsRepository.IsExists(id);
        }
    }
}
