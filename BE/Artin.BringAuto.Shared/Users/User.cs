using Artin.BringAuto.Shared.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Users
{
    public class User : IId<String>
    {
        public virtual String Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }

        public IEnumerable<String> Roles { get; set; }

        public IEnumerable<Tenant> Tenants{ get; set; }
    }
}
