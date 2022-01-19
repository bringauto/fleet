using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Routes
{
    public class Route : NewRoute, IId<int>
    {
        public int Id { get; set; }
       
    }
}
