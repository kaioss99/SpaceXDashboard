using SpaceXDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceXDashboard.Repositories
{
    public interface ILaunchRepository
    {
        Task<List<Launch>> GetAllAsync();
        Task SaveAllAsync(List<Launch> launches);
    }
}