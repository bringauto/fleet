using Artin.BringAuto.Shared.Stations;
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
        public StationInfo Station { get; set; }
    }
}
