using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Ifaces;
using Artin.BringAuto.Shared.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Providers
{
    public class CanAccessOrderProvider : ICanAccessOrderProvider
    {
        private readonly ICurrentRoles currentRoles;

        public CanAccessOrderProvider(IHttpContextAccessor httpContextAccessor, ICurrentRoles currentRoles)
        {
            this.Context = httpContextAccessor?.HttpContext;
            this.currentRoles = currentRoles;
        }

        public bool CanAccessAllOrders
        {
            get
            {
                var allowAccessAll = new[] { RoleNames.Driver, RoleNames.Privileged, RoleNames.Admin };
                return  currentRoles.Roles.Any(x => allowAccessAll.Contains(x));
            }
        }

        public string CurrentUserId => Context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        public HttpContext Context { get; }
    }
}
