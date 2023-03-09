using Artin.BringAuto.Configuration;
using Artin.BringAuto.Shared.Cars;
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
    public class UpdateButtonState : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IOptions<ButtonSettings> buttonOptions;
        private readonly ILogger<UpdateButtonState> logger;

        public UpdateButtonState(IServiceProvider serviceProvider, IOptions<ButtonSettings> options, ILogger<UpdateButtonState> logger)
        {
            this.serviceProvider = serviceProvider;
            this.buttonOptions = options;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = serviceProvider.CreateScope();
                    var carRepository = scope.ServiceProvider.GetRequiredService<ICarRepository>();
                }
                catch (Exception exc)
                {
                    logger.LogWarning(exc, exc.Message);
                }
                await Task.Delay(TimeSpan.FromSeconds(buttonOptions.Value.TimeoutSec / 2.0));
            }
        }

    }
}
