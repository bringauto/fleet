using Artin.BringAuto.Shared.Ifaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Services
{
    public class CurrentTenant : ICurrentTenant
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public const string HeaderName = "tenant";

        public CurrentTenant(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public int? GetTenantId()
        {
            var headerValue = httpContextAccessor.HttpContext?.Request?.Headers?.FirstOrDefault(x => x.Key == HeaderName).Value.FirstOrDefault();
            if (!String.IsNullOrEmpty(headerValue) && Int32.TryParse(headerValue, out var tenant))
                return tenant;
            return null;
        }
    }
}
