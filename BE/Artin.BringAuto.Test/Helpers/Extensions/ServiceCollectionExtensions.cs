using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebApplication18;

namespace Helpers.Extensions.Tests.Helpers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Removes all registrations of <see cref="TService"/> and adds in <see cref="TImplementation"/>.
        /// </summary>
        /// <typeparam name="TService">The type of service interface which needs to be placed.</typeparam>
        /// <typeparam name="TImplementation">The test or mock implementation of <see cref="TService"/> to add into <see cref="services"/>.</typeparam>
        /// <param name="services"></param>
        public static void SwapService<TService, TImplementation>(this IServiceCollection services, ServiceLifetime serviceLifetime)
            where TImplementation : class, TService
        {
            if (services.Any(x => x.ServiceType == typeof(TService) && x.Lifetime == serviceLifetime))
            {
                var serviceDescriptors = services.Where(x => x.ServiceType == typeof(TService) && x.Lifetime == serviceLifetime).ToList();
                foreach (var serviceDescriptor in serviceDescriptors)
                {
                    services.Remove(serviceDescriptor);
                }
            }

            if (serviceLifetime == ServiceLifetime.Transient)
                services.AddTransient(typeof(TService), typeof(TImplementation));

            else if (serviceLifetime == ServiceLifetime.Scoped)
                services.AddScoped(typeof(TService), typeof(TImplementation));

            else if (serviceLifetime == ServiceLifetime.Singleton)
                services.AddSingleton(typeof(TService), typeof(TImplementation));
        }

        public static void SwapSqLiteDbContext<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            // Remove the app's ApplicationDbContext registration.
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            var connection = new SqliteConnection($"DataSource='file:memdb{Guid.NewGuid()}?mode=memory&cache=shared'");
            connection.Open();

            // Add ApplicationDbContext using an in-memory database for testing.

            var app = Assembly.GetAssembly(typeof(Startup));
            services.AddDbContext<TDbContext>(options => options.UseSqlite(connection).UseTriggers((
                triggerOptions => triggerOptions.AddAssemblyTriggers(app))));
        }
    }
}
