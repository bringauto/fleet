using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Stops
{
    public class NewStop
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [Phone]
        public string ContactPhone { get; set; }
    }
}
