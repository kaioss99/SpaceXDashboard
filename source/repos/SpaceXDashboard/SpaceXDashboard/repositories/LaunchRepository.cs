using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceXDashboard.Repositories
{
    public class LaunchRepository : ILaunchRepository
    {
        private readonly DatabaseContext _context = new DatabaseContext();
        private readonly ApiService _api = new ApiService();

        public async Task<List<Launch>> GetAllAsync()
        {
            var launches = await _api.GetLaunchesAsync();

            if (launches != null && launches.Count > 0)
            {
                await SaveAllAsync(launches);
                return launches;
            }

            return GetFromDatabase();
        }

        public Task SaveAllAsync(List<Launch> launches)
        {
            using var connection = _context.GetConnection();
            connection.Open();

            foreach (var l in launches)
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    INSERT OR REPLACE INTO Launches
                    VALUES ($id, $name, $date, $success, $rocket, $details)";
                cmd.Parameters.AddWithValue("$id", l.Id ?? "");
                cmd.Parameters.AddWithValue("$name", l.Name ?? "");
                cmd.Parameters.AddWithValue("$date", l.DateUtc?.ToString() ?? "");
                cmd.Parameters.AddWithValue("$success", l.Success == true ? 1 : 0);
                cmd.Parameters.AddWithValue("$rocket", l.RocketName ?? "");
                cmd.Parameters.AddWithValue("$details", l.Details ?? "");
                cmd.ExecuteNonQuery();
            }

            return Task.CompletedTask;
        }

        private List<Launch> GetFromDatabase()
        {
            var launches = new List<Launch>();
            using var connection = _context.GetConnection();
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Launches";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                launches.Add(new Launch
                {
                    Id = reader.GetString(0),
                    Name = reader.GetString(1),
                    DateUtc = DateTime.TryParse(reader.GetString(2), out var d) ? d : null,
                    Success = reader.GetInt32(3) == 1,
                    RocketName = reader.GetString(4),
                    Details = reader.GetString(5)
                });
            }

            return launches;
        }
    }
}