using Artin.BringAuto.Repositories;
using Artin.BringAuto.Shared.Butons;
using Artin.BringAuto.Shared.Cars;
using Artin.BringAuto.Shared.LocationHistory;
using Artin.BringAuto.Shared.Maps;
using Artin.BringAuto.Shared.Orders;
using Artin.BringAuto.Shared.Routes;
using Artin.BringAuto.Shared.Stops;
using Artin.BringAuto.Shared.Tenants;
using BringAuto.Server.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BringAuto.Server.StartupParts
{
    public static class StartupRepositories
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICarRepository, CarRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IRouteRepository, RouteRepository>();
            services.AddTransient<IStopRepository, StationRepository>();
            services.AddTransient<IMapRepository, MapRepository>();
            services.AddTransient<ILocationHistoryRepository, CarLocationHistoryRepository>();
            services.AddTransient<IButtonRepository, ButtonRepository>();
            services.AddTransient<ITenantRepository, TenantRepository>();

            return services;
        }
    }
}
