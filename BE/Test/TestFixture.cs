using Artin.BringAuto.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Test
{
    public class TestFixture : IDisposable
    {
        public const string USERNAME = "j.hodic@fingood.cz";
        public const string PASSWORD = "Abc123456789";
        private readonly IServiceProvider _services;

        public HttpClient HttpClient { get; set; }
        public BringAutoDbContext DbContext { get; private set; }
        /*
        public TestFixture()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    // Add TestServer
                    webHost.UseTestServer()
                        .UseStartup<Startup>()
                        .UseEnvironment(Environments.Development)
                        .UseConfiguration(new ConfigurationBuilder()
                            .AddJsonFile("appsettings.Development.json")
                            .Build()
                    );

                    // configure the services after the startup has been called.
                    webHost.ConfigureTestServices(services =>
                    {
                        // register the test one specifically

                        services.SwapSqLiteDbContext<BringAutoDbContext>();
                        SetServices(services);
                    });
                });

            var host = hostBuilder.Start();
            _services = host.Services;
            HttpClient = host.GetTestClient();
            DbContext = GetService<IDbContextResolver>().GetContext() as BringAutoDbContext;

            // Init DB
            CreateDatabase();


        }

        public void ReloadContext()
        {
            DbContext = new BringAutoDbContext(GetService<DbContextOptions<BringAutoDbContext>>());
        }

        */
        public virtual void CreateDatabase()
            => DbContext.Database.EnsureCreated();

        public IServiceProvider GetServices() => _services;
        public T GetService<T>() => (T)_services.GetService(typeof(T));

        protected virtual void SetServices(IServiceCollection serviceCollection) { }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (HttpClient != null)
                {
                    HttpClient.Dispose();
                    HttpClient = null;
                }
            }
        }
    }

}
