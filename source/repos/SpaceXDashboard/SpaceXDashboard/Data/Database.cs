using Microsoft.Data.Sqlite;

namespace SpaceXDashboard.Data
{
    public class DatabaseContext
    {
        private const string DB_PATH = "spacex.db";

        public SqliteConnection GetConnection() =>
            new SqliteConnection($"Data Source={DB_PATH}");

        public void Initialize()
        {
            using var connection = GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Launches (
                    Id      TEXT PRIMARY KEY,
                    Name    TEXT,
                    Success INTEGER,
                    Details TEXT
                );
                CREATE TABLE IF NOT EXISTS Rockets (
                    Id             TEXT PRIMARY KEY,
                    Name           TEXT,
                    Description    TEXT,
                    Active         INTEGER,
                    SuccessRatePct INTEGER
                );
                CREATE TABLE IF NOT EXISTS Stats (
                    Id                 INTEGER PRIMARY KEY,
                    TotalLaunches      INTEGER,
                    SuccessfulLaunches INTEGER,
                    FailedLaunches     INTEGER,
                    SuccessRate        REAL
                );";

            command.ExecuteNonQuery();
        }
    }
}