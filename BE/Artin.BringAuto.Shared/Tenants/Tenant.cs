using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Tenants
{
    public class Tenant : IId<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
