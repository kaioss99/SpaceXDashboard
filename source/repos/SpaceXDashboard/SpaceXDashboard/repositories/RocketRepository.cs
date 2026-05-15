using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SpaceXDashboard.Data;
using SpaceXDashboard.Models;
using SpaceXDashboard.Services;

namespace SpaceXDashboard.Repositories
{
    public class RocketRepository : IRocketRepository
    {
        private readonly DatabaseContext _context = new DatabaseContext();
        private readonly ApiService _api = new ApiService();

        public async Task<List<Rocket>> GetAllAsync()
        {
            var rockets = await _api.GetRocketsAsync();

            if (rockets != null && rockets.Count > 0)
            {
                await SaveAllAsync(rockets);
                return rockets;
            }

            return GetFromDatabase();
        }

        public Task SaveAllAsync(List<Rocket> rockets)
        {
            using var connection = _context.GetConnection();
            connection.Open();

            foreach (var r in rockets)
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    INSERT OR REPLACE INTO Rockets
                    VALUES ($id, $name, $desc, $active, $rate)";
                cmd.Parameters.AddWithValue("$id", r.Id ?? "");
                cmd.Parameters.AddWithValue("$name", r.Name ?? "");
                cmd.Parameters.AddWithValue("$desc", r.Description ?? "");
                cmd.Parameters.AddWithValue("$active", r.Active ? 1 : 0);
                cmd.Parameters.AddWithValue("$rate", r.SuccessRatePct);
                cmd.ExecuteNonQuery();
            }

            return Task.CompletedTask;
        }

        private List<Rocket> GetFromDatabase()
        {
            var rockets = new List<Rocket>();
            using var connection = _context.GetConnection();
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Rockets";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                rockets.Add(new Rocket
                {
                    Id = reader.GetString(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    Active = reader.GetInt32(3) == 1,
                    SuccessRatePct = reader.GetInt32(4)
                });
            }

            return rockets;
        }
    }
}