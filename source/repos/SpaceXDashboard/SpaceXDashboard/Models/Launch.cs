using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceXDashboard.Models
{
    public class Launch
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateUtc { get; set; }
        public bool? Success { get; set; }
        public string RocketName { get; set; }
        public string Details { get; set; }
    }
}