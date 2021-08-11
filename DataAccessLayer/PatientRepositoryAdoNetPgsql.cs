using Common;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class PatientRepositoryAdoNetPgsql : IPatientRepository
    {
        
        // Get it from web.config or similar.
        private string connString = "Host=localhost;Username=postgres;Password=Tetya1:2;Database=identitywithpgsql";

        public PatientRepositoryAdoNetPgsql()
        {           
        }

        public async Task<IList<PatientDto>> GetListAsync()
        {
            var list = new List<PatientDto>();

            
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            // Retrieve all rows
            await using (var cmd = new NpgsqlCommand("SELECT * FROM public.\"Patient\"", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new PatientDto
                    {
                        Address = reader.GetFieldValue<string[]>("Address").First(),
                        Name = reader.GetFieldValue<string[]>("Name").First(),
                        ID = reader.GetInt64("Id")
                    });
                }
            }

            return list;
        }

        public async Task<long> AddAsync(PatientDto item)
        {
            var list = new List<PatientDto>();

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            // Retrieve all rows
            await using (var cmd = new NpgsqlCommand("INSERT INTO public.\"Patient\"(\"Name\", \"Address\") VALUES(@name, @address); ", conn))
            {
                cmd.Parameters.AddWithValue("name", new string[1] { item.Name });
                cmd.Parameters.AddWithValue("address", new string[1] { item.Address });
                await cmd.ExecuteNonQueryAsync();
            }

            return 1;
        }


        public async Task<long> RemoveWithIDAsync(long ID)
        {
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            // Remove a patient.
            await using (var cmd = new NpgsqlCommand("DELETE FROM public.\"Patient\" where \"ID\"= @id", conn))
            {
                cmd.Parameters.AddWithValue("id", ID);
                await cmd.ExecuteNonQueryAsync();
            }

            return 1;
        }

        public async Task<long> UpdateAsync(PatientDto item)
        {
            var list = new List<PatientDto>();

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            // Retrieve all rows
            await using (var cmd = new NpgsqlCommand("UPDATE public.\"Patient\" SET \"Name\" =@name, \"Address\"=@address WHERE \"ID\"= @id", conn))
            {
                cmd.Parameters.AddWithValue("id", item.ID);
                cmd.Parameters.AddWithValue("name", new string[1] { item.Name });
                cmd.Parameters.AddWithValue("address", new string[1] { item.Address });
                await cmd.ExecuteNonQueryAsync();
            }

            return 1;
        }

        public async Task<bool> IsExists(long id)
        {
            var list = new List<PatientDto>();

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            // Retrieve one row matching an ID.
            await using (var cmd = new NpgsqlCommand("SELECT Count(*) FROM public.\"Patient\" where \"ID\"= @id", conn))
            {
                cmd.Parameters.AddWithValue("id", id);
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        return reader.GetInt64(0) > 0;
                    }
                }
            }

            return false;
        }

        public async Task<PatientDto> FindAsync(long id)
        {
            var list = new List<PatientDto>();

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            // Retrieve one row matching an ID.
            await using (var cmd = new NpgsqlCommand("SELECT * FROM public.\"Patient\" where \"ID\"= @id", conn))
            {
                cmd.Parameters.AddWithValue("id", id);
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new PatientDto
                        {
                            Address = reader.GetFieldValue<string[]>("Address").First(),
                            Name = reader.GetFieldValue<string[]>("Name").First(),
                            ID = reader.GetInt64("Id")
                        });
                    }
                }
            }

            return list.First();
        }

        public async Task<long> RemoveAsync(PatientDto patient)
        {
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            // Remove a patient.
            await using (var cmd = new NpgsqlCommand("DELETE FROM public.\"Patient\" where \"ID\"= @id", conn))
            {
                cmd.Parameters.AddWithValue("id", patient.ID);
                await cmd.ExecuteNonQueryAsync();
            }

            return 1;
        }
    }
}
