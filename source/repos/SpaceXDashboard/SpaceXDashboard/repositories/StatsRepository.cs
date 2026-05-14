using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceXDashboard.Repositories
{
    public class StatsRepository : IStatsRepository
    {
        private readonly DatabaseContext _context = new DatabaseContext();
        private readonly ApiService _api = new ApiService();

        public async Task<Stats> GetAsync()
        {
            var stats = await _api.GetStatsAsync();

            if (stats != null)
            {
                await SaveAsync(stats);
                return stats;
            }

            return GetFromDatabase();
        }

        public Task SaveAsync(Stats stats)
        {
            using var connection = _context.GetConnection();
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT OR REPLACE INTO Stats
                VALUES (1, $total, $success, $failed, $rate)";
            cmd.Parameters.AddWithValue("$total", stats.TotalLaunches);
            cmd.Parameters.AddWithValue("$success", stats.SuccessfulLaunches);
            cmd.Parameters.AddWithValue("$failed", stats.FailedLaunches);
            cmd.Parameters.AddWithValue("$rate", stats.SuccessRate);
            cmd.ExecuteNonQuery();

            return Task.CompletedTask;
        }

        private Stats GetFromDatabase()
        {
            using var connection = _context.GetConnection();
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Stats WHERE Id = 1";

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Stats
                {
                    TotalLaunches = reader.GetInt32(1),
                    SuccessfulLaunches = reader.GetInt32(2),
                    FailedLaunches = reader.GetInt32(3),
                    SuccessRate = reader.GetDouble(4)
                };
            }

            return new Stats();
        }
    }
}