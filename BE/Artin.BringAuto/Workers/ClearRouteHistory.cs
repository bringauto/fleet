using Artin.BringAuto.Configuration;
using Artin.BringAuto.DAL;
using Artin.BringAuto.Shared.Cars;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Artin.BringAuto.Workers
{
    public class ClearRouteHistory : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<UpdateButtonState> logger;

        public ClearRouteHistory(IServiceProvider serviceProvider, ILogger<UpdateButtonState> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<BringAutoDbContext>();
                    await db.Database.ExecuteSqlRawAsync($"DELETE from {nameof(DAL.Models.LocationHistory)} where {nameof(DAL.Models.LocationHistory.Time)} < DATEADD(hh,-5,sysdatetime()) ");
                }
                catch (Exception exc)
                {
                    logger.LogWarning(exc, exc.Message);
                }
                await Task.Delay(TimeSpan.FromHours(12));
            }
        }

    }
}
