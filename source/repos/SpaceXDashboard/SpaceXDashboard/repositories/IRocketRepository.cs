using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceXDashboard.Repositories
{
    public interface IRocketRepository
    {
        Task<List<Rocket>> GetAllAsync();
        Task SaveAllAsync(List<Rocket> rockets);
    }
}