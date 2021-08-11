using Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;

namespace DataAccessLayer
{
    public class UnnaturalDeathsRepositoryEFCore : IUnnaturalDeathsRepository
    {
        private readonly identitywithpgsqlContext _context;

        public UnnaturalDeathsRepositoryEFCore(identitywithpgsqlContext context)
        {
            _context = context;
        }

        public async Task<IList<UnnaturalDeaths>> GetListAsync()
        {
            var listFromEF =  await _context.Unnaturaldeaths.ToListAsync();
            var list = new List<UnnaturalDeaths>();

            foreach (var death in listFromEF)
            {
                list.Add(new UnnaturalDeaths {
                    Address = death.Address,
                    Age = death.Age,
                    CidOrPassport = death.CidOrPassport,
                    DateOfPostmortemExamination = death.DateOfPostmortemExamination,
                    DeceasedName = death.DeceasedName,
                    Dzongkhag = death.Dzongkhag,
                    GeneralExternalInformation = death.GeneralExternalInformation,
                    History = death.History,
                    ImformantCidNo = death.ImformantCidNo,
                    InformantName = death.InformantName,
                    InformantRelationToDeceased = death.InformantRelationToDeceased,
                    Isactive = death.Isactive,
                    Lastchanged = death.Lastchanged,
                    Nationality = death.Nationality,
                    PlaceOfExamination = death.PlaceOfExamination,
                    PoliceCaseNo = death.PoliceCaseNo,
                    PoliceStation = death.PoliceStation,
                    Remark = death.Remark,
                    SceneOfDeath = death.SceneOfDeath,
                    Sex = death.Sex.ToString(),
                    TimeOfPostmortemExamination = death.TimeOfPostmortemExamination,
                    Transactedby = death.Transactedby,
                    Transacteddate = death.Transacteddate,
                    Version = death.Version,
                    Id = death.Id
                });
            }

            return list;
        }

        public async Task<long> AddAsync(UnnaturalDeaths item)
        {
            _context.Add(new UnnaturalDeaths {
                Address = item.Address,
                Age = item.Age,
                CidOrPassport = item.CidOrPassport,
                DateOfPostmortemExamination = item.DateOfPostmortemExamination,
                DeceasedName = item.DeceasedName,
                Dzongkhag = item.Dzongkhag,
                GeneralExternalInformation = item.GeneralExternalInformation,
                History = item.History,
                ImformantCidNo = item.ImformantCidNo,
                InformantName = item.InformantName,
                InformantRelationToDeceased = item.InformantRelationToDeceased,
                Isactive = item.Isactive,
                Lastchanged = item.Lastchanged,
                Nationality = item.Nationality,
                PlaceOfExamination = item.PlaceOfExamination,
                PoliceCaseNo = item.PoliceCaseNo,
                PoliceStation = item.PoliceStation,
                Remark = item.Remark,
                SceneOfDeath = item.SceneOfDeath,
                Sex = item.Sex,
                TimeOfPostmortemExamination = item.TimeOfPostmortemExamination,
                Transactedby = item.Transactedby,
                Transacteddate = item.Transacteddate,
                Version = item.Version,
                Id = item.Id
            });

            return await _context.SaveChangesAsync();
        }

        
        public async Task<long> RemoveWithIDAsync(Guid ID)
        {
            var UnnaturalDeaths = await _context.Unnaturaldeaths.FindAsync(ID);
            _context.Unnaturaldeaths.Remove(UnnaturalDeaths);
            return await _context.SaveChangesAsync();
        }

        public async Task<long> UpdateAsync(UnnaturalDeaths item)
        {
            _context.Update(new UnnaturalDeaths { 
                Address = item.Address,
                Age = item.Age,
                CidOrPassport = item.CidOrPassport,
                DateOfPostmortemExamination = item.DateOfPostmortemExamination,
                DeceasedName = item.DeceasedName,
                Dzongkhag = item.Dzongkhag,
                GeneralExternalInformation = item.GeneralExternalInformation,
                History = item.History,
                ImformantCidNo = item.ImformantCidNo,
                InformantName = item.InformantName,
                InformantRelationToDeceased = item.InformantRelationToDeceased,
                Isactive = item.Isactive,
                Lastchanged = item.Lastchanged,
                Nationality = item.Nationality,
                PlaceOfExamination = item.PlaceOfExamination,
                PoliceCaseNo = item.PoliceCaseNo,
                PoliceStation = item.PoliceStation,
                Remark = item.Remark,
                SceneOfDeath = item.SceneOfDeath,
                Sex = item.Sex,
                TimeOfPostmortemExamination = item.TimeOfPostmortemExamination,
                Transactedby = item.Transactedby,
                Transacteddate = item.Transacteddate,
                Version = item.Version,
                Id = item.Id
            });
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> IsExists(Guid id)
        {
            return await _context.Unnaturaldeaths.AnyAsync(e => e.Id == id);
        }

        public async Task<UnnaturalDeaths> FindAsync(Guid id)
        {
            var UnnaturalDeaths = await _context.Unnaturaldeaths
               .FirstOrDefaultAsync(m => m.Id == id);

            if (UnnaturalDeaths == null)
            {
                return null;
            }

            return new UnnaturalDeaths {
                Address = UnnaturalDeaths.Address,
                Age = UnnaturalDeaths.Age,
                CidOrPassport = UnnaturalDeaths.CidOrPassport,
                DateOfPostmortemExamination = UnnaturalDeaths.DateOfPostmortemExamination,
                DeceasedName = UnnaturalDeaths.DeceasedName,
                Dzongkhag = UnnaturalDeaths.Dzongkhag,
                GeneralExternalInformation = UnnaturalDeaths.GeneralExternalInformation,
                History = UnnaturalDeaths.History,
                ImformantCidNo = UnnaturalDeaths.ImformantCidNo,
                InformantName = UnnaturalDeaths.InformantName,
                InformantRelationToDeceased = UnnaturalDeaths.InformantRelationToDeceased,
                Isactive = UnnaturalDeaths.Isactive,
                Lastchanged = UnnaturalDeaths.Lastchanged,
                Nationality = UnnaturalDeaths.Nationality,
                PlaceOfExamination = UnnaturalDeaths.PlaceOfExamination,
                PoliceCaseNo = UnnaturalDeaths.PoliceCaseNo,
                PoliceStation = UnnaturalDeaths.PoliceStation,
                Remark = UnnaturalDeaths.Remark,
                SceneOfDeath = UnnaturalDeaths.SceneOfDeath,
                Sex = UnnaturalDeaths.Sex.ToString(),
                TimeOfPostmortemExamination = UnnaturalDeaths.TimeOfPostmortemExamination,
                Transactedby = UnnaturalDeaths.Transactedby,
                Transacteddate = UnnaturalDeaths.Transacteddate,
                Version = UnnaturalDeaths.Version,
                Id = UnnaturalDeaths.Id
            };
        }

        public async Task<long> RemoveAsync(UnnaturalDeaths UnnaturalDeaths)
        {
            var UnnaturalDeathsObj = await _context.Unnaturaldeaths.FindAsync(UnnaturalDeaths.Id);
            _context.Unnaturaldeaths.Remove(UnnaturalDeathsObj);
            return await _context.SaveChangesAsync();
        }
    }
}
