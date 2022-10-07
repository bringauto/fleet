using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.DAL
{
    public class RequireTenantProvider
    {
        public bool RequireTenant { get; }

        public RequireTenantProvider(bool requireTenant)
        {
            RequireTenant = requireTenant;
        }
    }
}
