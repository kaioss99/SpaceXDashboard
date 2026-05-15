using System.Net.Http;
using Newtonsoft.Json;
using SpaceXDashboard.Models;

namespace SpaceXDashboard.Services
{
    public class ApiService
    {
        private readonly HttpClient _client = new HttpClient();
        private const string BASE_URL = "https://url-da-api-do-aluno2.com";

        public async Task<List<Launch>> GetLaunchesAsync()
        {
            try
            {
                var response = await _client.GetStringAsync($"{BASE_URL}/api/launches");
                return JsonConvert.DeserializeObject<List<Launch>>(response);
            }
            catch { return null; }
        }

        public async Task<List<Rocket>> GetRocketsAsync()
        {
            try
            {
                var response = await _client.GetStringAsync($"{BASE_URL}/api/rockets");
                return JsonConvert.DeserializeObject<List<Rocket>>(response);
            }
            catch { return null; }
        }

        public async Task<Stats> GetStatsAsync()
        {
            try
            {
                var response = await _client.GetStringAsync($"{BASE_URL}/api/stats");
                return JsonConvert.DeserializeObject<Stats>(response);
            }
            catch { return null; }
        }
    }
}