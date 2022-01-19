using Artin.BringAuto.Shared.Ifaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Providers
{
    public class CurrentRoles : ICurrentRoles
    {
        public CurrentRoles(IHttpContextAccessor httpContextAccessor)
        {
            this.Context = httpContextAccessor?.HttpContext;
        }

        public HttpContext Context { get; }

        public IEnumerable<string> Roles
            =>  Context?.User?.FindAll(ClaimTypes.Role).Select(x => x.Value) ?? Enumerable.Empty<String>();
    }
}
