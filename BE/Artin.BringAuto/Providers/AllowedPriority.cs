using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Enums;
using Artin.BringAuto.Shared.Ifaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Providers
{
    public class AllowedPriority : IAllowedPriority
    {
        Dictionary<String, OrderPriority> MaxPriorities = new Dictionary<string, OrderPriority>()
        {
            [RoleNames.User] = OrderPriority.Normal,
            [RoleNames.Privileged] = OrderPriority.High,
            [RoleNames.Admin] = OrderPriority.High,
            [RoleNames.Driver] = OrderPriority.High
        };

        public AllowedPriority(ICurrentRoles currentRoles)
        {
            CurrentRoles = currentRoles;
        }

        public ICurrentRoles CurrentRoles { get; }

        public OrderPriority CheckOrderPriority(OrderPriority priority)
        {
            var maxAllowedPriority = OrderPriority.Low;
            foreach (var role in CurrentRoles.Roles)
            {
                if (MaxPriorities.TryGetValue(role, out var rolePriority))
                {
                    if (priority <= rolePriority)
                        return priority;
                    else if (maxAllowedPriority < rolePriority)
                        maxAllowedPriority = rolePriority;
                }
            }
            return maxAllowedPriority;
        }
    }
}
