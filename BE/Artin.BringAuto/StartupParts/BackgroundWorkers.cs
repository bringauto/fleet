using Microsoft.Extensions.DependencyInjection;

namespace Artin.BringAuto.StartupParts
{
    public static class BackgroundWorkers
    {
        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {

            services.AddHostedService<Workers.UpdateButtonState>();
            services.AddHostedService<Workers.ClearRouteHistory>();

            return services;
        }
    }
}
