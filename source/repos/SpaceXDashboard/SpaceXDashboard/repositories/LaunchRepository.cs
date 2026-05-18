using Microsoft.Data.Sqlite;
using SpaceXDashboard.Data;
using SpaceXDashboard.Models;
using SpaceXDashboard.Services;

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
                    VALUES ($id, $name, $success, $details)";
                cmd.Parameters.AddWithValue("$id", l.Id ?? "");
                cmd.Parameters.AddWithValue("$name", l.Name ?? "");
                cmd.Parameters.AddWithValue("$success", l.Success == true ? 1 : 0);
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
                    Success = reader.GetInt32(2) == 1,
                    Details = reader.GetString(3)
                });
            }

            return launches;
        }
    }
}