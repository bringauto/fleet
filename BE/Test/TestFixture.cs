using Artin.BringAuto.DAL;
using Artin.BringAuto.DAL.Models;

using GenFu;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Microsoft.AspNetCore.TestHost;
using Helpers.Extensions.Tests.Helpers.Extensions;

namespace Artin.BringAuto.Test
{
    public class TestFixture : IDisposable
    {
        private readonly IServiceProvider ServiceProvider;

        public HttpClient HttpClient { get; set; }
        public IMock<BringAutoDbContext> DbContext { get; private set; }
        
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
                        //services.SwapService<IMySimpleGithubClient, MockSimpleGithubClient>(ServiceLifetime.Scoped);
                        services.SwapSqLiteDbContext<BringAutoDbContext>();
                        SetServices(services);
                    });
                });

            var host = hostBuilder.Start();
            ServiceProvider = host.Services;
            HttpClient = host.GetTestClient();
            DbContext = CreateDbContext();
        }
        
        public static IEnumerable<Car> GetFakeDataCars()
        {
            var i = 1;
            var persons = A.ListOf<Car>(26);
            persons.ForEach(x => x.Id = i++);
            return persons.Select(_ => _);
        }

        private static Mock<BringAutoDbContext> CreateDbContext()
        {
            var cars = GetFakeDataCars().AsQueryable();

            var dbSet = new Mock<DbSet<Car>>();
            dbSet.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(cars.Provider);
            dbSet.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(cars.Expression);
            dbSet.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(cars.ElementType);
            dbSet.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(cars.GetEnumerator());

            var context = new Mock<BringAutoDbContext>();
            context.Setup(c => c.Cars).Returns(dbSet.Object);
            return context;
        }
        public void ReloadContext()
            => DbContext = new Mock<BringAutoDbContext>(GetService<DbContextOptions<BringAutoDbContext>>());

        public T GetService<T>()
            => (T)ServiceProvider.GetService(typeof(T));

        protected virtual void SetServices(IServiceCollection serviceCollection) { }

        public void Dispose()
        {
            HttpClient.Dispose();
        }
    }

}
