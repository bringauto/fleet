using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Routes
{
    public class RouteUpdate : IId<int>
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Color { get; set; }
        public IEnumerable<RouteStopUpdate> Stops { get; set; }

    }
}
