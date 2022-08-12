using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Stops
{
    public class StopInfo
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        [Phone]
        public string ContactPhone { get; set; }
    }
}
