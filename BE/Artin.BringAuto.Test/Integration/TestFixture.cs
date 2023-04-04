using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.TestHost;
//using JsonApiDotNetCore.Repositories;
using Movies.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApplication18;
using Helpers.Extensions.Tests.Helpers.Extensions;
using Movies.Data.Models;
using Moq;
using GenFu;

namespace Movies.Api.Test.Integration
{
    public class TestFixture : IDisposable
    {
        private readonly IServiceProvider ServiceProvider;

        public HttpClient HttpClient { get; set; }
        public IMock<MoviesDbContext> DbContext { get; private set; }

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
                        services.SwapSqLiteDbContext<MoviesDbContext>();
                        SetServices(services);
                    });
                });

            var host = hostBuilder.Start();
            ServiceProvider = host.Services;
            HttpClient = host.GetTestClient();
            DbContext = CreateDbContext();
        }

        public static IEnumerable<Person> GetFakeDataPersons()
        {
            var i = 1;
            var persons = A.ListOf<Person>(26);
            persons.ForEach(x => x.Id = i++);
            return persons.Select(_ => _);
        }

        private static IEnumerable<Genre> GetFakeDataGenres()
        {
            var i = 1;
            var genres = A.ListOf<Genre>(26);
            genres.ForEach(x => x.Id = i++);
            return genres.Select(_ => _);
        }
        private static Mock<MoviesDbContext> CreateDbContext()
        {
            var persons = GetFakeDataPersons().AsQueryable();

            var dbSet = new Mock<DbSet<Person>>();
            dbSet.As<IQueryable<Person>>().Setup(m => m.Provider).Returns(persons.Provider);
            dbSet.As<IQueryable<Person>>().Setup(m => m.Expression).Returns(persons.Expression);
            dbSet.As<IQueryable<Person>>().Setup(m => m.ElementType).Returns(persons.ElementType);
            dbSet.As<IQueryable<Person>>().Setup(m => m.GetEnumerator()).Returns(persons.GetEnumerator());

            var context = new Mock<MoviesDbContext>();
            context.Setup(c => c.Persons).Returns(dbSet.Object);
            return context;
        }
        public void ReloadContext()
            => DbContext = new Mock<MoviesDbContext>(GetService<DbContextOptions<MoviesDbContext>>());

        public T GetService<T>()
            => (T)ServiceProvider.GetService(typeof(T));

        protected virtual void SetServices(IServiceCollection serviceCollection) { }

        public void Dispose()
        {
            HttpClient.Dispose();
        }
    }
}
