using Artin.BringAuto.Shared.Stops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Routes
{
    public class RouteStop 
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Order { get; set; }
        public StopInfo Station { get; set; }
    }
}
