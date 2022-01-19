using Artin.BringAuto.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Cars
{
    public class CarStatusInfo
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public CarStatus? Status { get; set; }
    }
}
