using Common;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace datasyncservice.Services
{
    public class PatientSyncService : BackgroundService
    {
        private readonly ILogger<PatientSyncService> _logger;

        public PatientSyncService(ILogger<PatientSyncService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"PatientSyncService is starting.");

            stoppingToken.Register(() =>
                _logger.LogDebug($" PatientSyncService background task is stopping."));

            // Keep these in appsettings.json or similar. This is just a POC. 
            var sourceConnection = @"Server=127.0.0.1;Port=5432;Database=identitywithpgsql;User Id=postgres;Password=Tetya1:2;";
            var destinationConnection = @"Server=127.0.0.1;Port=5433;Database=postgres;User Id=postgres;Password=Tetya1:2;";

            var _sourceContext = new identitywithpgsqlContext();
            _sourceContext.Database.GetDbConnection().ConnectionString = sourceConnection;

            var _destinationContext = new identitywithpgsqlContext();
            _destinationContext.Database.GetDbConnection().ConnectionString = destinationConnection;

            var listFromEF = _sourceContext.Patient.ToList();
            var list = new List<PatientDto>();

            foreach (var pat in listFromEF)
            {
                // See if this patient is in the destination db.
                // If yes, update it if its last modified date is earlier than this 
                // record (todo). If not, update this record (todo). 
                // If this record is not in the destination db, insert it (see below).
                // You can use a UHID or Name or similar to check whether the given
                // patient is there in the dest db.
                // Names can be repeacted. So please use UHID.
                var isThere = _destinationContext.Patient.ToList().FirstOrDefault(p => p.Uhid.ToString().ToLower() == pat.Uhid.ToString().ToLower()) != null;

                if (!isThere)
                {
                    // Insert it.
                    // Reset Unique IDs because these will be inserted by the system.
                    // Use UHIDs as stated above to check for uniqueness.
                    pat.Id = 0;
                    await _destinationContext.Patient.AddAsync(pat);
                }
                else
                {
                    // The record is there. Check last modified time.
                    var destRecord = _destinationContext.Patient.ToList().FirstOrDefault(p => p.Uhid.ToString().ToLower() == pat.Uhid.ToString().ToLower());

                    if (destRecord?.LastModified < pat.LastModified)
                    {
                        // Update destination record.
                        destRecord.Name = pat.Name;
                        destRecord.Address = pat.Address;
                        destRecord.LastModified = pat.LastModified;
                        
                        destRecord.Uhid = pat.Uhid;
                    }
                    else if (destRecord?.LastModified > pat.LastModified)
                    {
                        // Modify source record.
                        pat.Name = destRecord.Name;
                        pat.Address = destRecord.Address;
                        pat.LastModified = destRecord.LastModified;
                        
                        pat.Uhid = destRecord.Uhid;
                    }
                }
            }

            try
            {
                await _destinationContext.SaveChangesAsync();
                await _sourceContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }

            _logger.LogDebug($"PatientSyncService task doing background work.");

            _logger.LogDebug($"PatientSyncService background task is stopping.");
        }
    }
}
