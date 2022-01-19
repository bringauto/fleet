using Artin.BringAuto.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Maps
{
    public class NewMap
    {
        public String Image { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Double MinLongitude { get; set; }
        public Double MaxLongitude { get; set; }
        public Double MinLatitude { get; set; }
        public Double MaxLatitude { get; set; }

    }
}
