using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Routes
{
    public class NewRoute
    {
        public String Name { get; set; }
        public String Color { get; set; }
        public IEnumerable<RouteStop> Stops { get; set; } 
    }
}
