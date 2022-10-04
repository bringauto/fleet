using Artin.BringAuto.DAL;
using Artin.BringAuto.Shared.Ifaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context,
                                 BringAutoDbContext dbContext,
                                 ICurrentTenant currentTenant,
                                 ICurrentUserId currentUserId)
        {
            var tenantId = currentTenant.GetTenantId();

            if (!tenantId.HasValue || await UserIsPartOfTenant(tenantId, currentUserId.CurrentUserId, dbContext))
                await _next(context);
            else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("{\"error\":\"You are not part of required tenant\"}");
            }
        }

        private Task<bool> UserIsPartOfTenant(int? tenantId, string currentUserId, BringAutoDbContext dbContext)
        {
            return dbContext.UserTenancy.AnyAsync(x => x.UserId == currentUserId && x.TenantId == tenantId.Value);
        }
    }
}
