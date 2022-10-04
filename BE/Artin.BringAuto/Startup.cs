using Artin.BringAuto.Configuration;
using Artin.BringAuto.DAL;
using Artin.BringAuto.DAL.Models;
using Artin.BringAuto.GraphQL.Cars;
using Artin.BringAuto.GraphQL.Maps;
using Artin.BringAuto.GraphQL.Orders;
using Artin.BringAuto.GraphQL.Routes;
using Artin.BringAuto.GraphQL.Stations;
using Artin.BringAuto.GraphQL.Users;
using Artin.BringAuto.Helpers;
using Artin.BringAuto.Middlewares;
using Artin.BringAuto.MQTTClient;
using Artin.BringAuto.Providers;
using Artin.BringAuto.Services;
using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Ifaces;
using Artin.BringAuto.Shared.Twillio;
using Artin.BringAuto.StartupParts;
using AutoMapper;
using BringAuto.Server.StartupParts;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Subscriptions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace Artin.BringAuto
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
            .CreateLogger();
            var section = Configuration.GetSection("tokenManagement");
            var tokenSettings = section.Get<TokenManagement>();
            services.Configure<TokenManagement>(section);

            section = Configuration.GetSection("ButtonSettings");
            services.Configure<ButtonSettings>(section);

            services.AddDbContext<DAL.BringAutoDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetValue<string>("ConnectionStrings:BringAuto"));
            }, ServiceLifetime.Transient);
            services.AddAutoMapper(typeof(Artin.BringAuto.Mappings.CarMap).Assembly);


            var CookieSecurity = Environment.GetEnvironmentVariable("CookieSecurity");
            if (Enum.TryParse<CookieSecurePolicy>(CookieSecurity, out var securePolicy))
                services.ConfigureApplicationCookie(options =>
                {
                    options.Cookie.SecurePolicy = securePolicy;
                });

            services.AddDefaultIdentity<ApplicationUser>(
                opt =>
                {
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequiredLength = 5;
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireLowercase = false;
                    opt.Password.RequireDigit = false;

                    opt.SignIn.RequireConfirmedAccount = false;
                    opt.SignIn.RequireConfirmedEmail = false;
                    opt.SignIn.RequireConfirmedPhoneNumber = false;
                }
                )
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<BringAutoDbContext>();

            services.AddRepositories();
            services.AddHostedServices();
            services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddGraphQL(SchemaBuilder.New()
                 .AddAuthorizeDirectiveType()
                 .AddQueryType(descriptor =>
                 {
                     descriptor.Name("Queries");
                     descriptor.AddField<CarQuery>();
                     descriptor.AddField<OrderQuery>();
                     descriptor.AddField<StopQuery>();
                     descriptor.AddField<RouteQuery>();
                     descriptor.AddField<UserQuery>();
                     descriptor.AddField<MapQuery>();
                 })
                 .AddMutationType(descriptor =>
                 {
                     descriptor.Name("Mutations");
                     descriptor.AddField<CarMutation>();
                     descriptor.AddField<OrderMutation>();
                     descriptor.AddField<StopMutation>();
                     descriptor.AddField<RouteMutation>();
                     descriptor.AddField<UserMutation>();
                     descriptor.AddField<MapMutation>();
                 })
                 .AddType<CarExtension>()
                 .AddType<UserExtension>()
                 );


            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(PolicyNames.IsCar, x =>
                {
                    x.RequireClaim(ClaimNames.CarName);
                    x.RequireClaim(ClaimNames.CarId);
                });
            });
            services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;

                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSettings.Secret)),
                        ValidIssuer = tokenSettings.Issuer,
                        ValidAudience = tokenSettings.Audience,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = false,

                    };
                })
                .AddCookie();

            services.AddScoped<ICanAccessOrderProvider, CanAccessOrderProvider>();
            services.AddScoped<ICurrentUserId, CanAccessOrderProvider>();
            services.AddScoped<ICurrentRoles, CurrentRoles>();
            services.AddScoped<IAllowedPriority, AllowedPriority>();
            services.AddScoped<IUpdateCarByMQTTService, UpdateCarByMQTTService>();
            services.AddScoped<ITwillioCaller, TwillioCaller>();
            services.AddScoped<IIsInStationProcess, IsInStationProcess>();
            services.AddScoped<ICurrentTenant, CurrentTenant>();
            services.Configure<TwillioConfig>(Configuration.GetSection(nameof(TwillioConfig)));

            services.AddCors(opt => opt.AddPolicy("AllowAll",
                   builder => builder
                   .AllowAnyMethod()
                   .AllowAnyOrigin()
                   .AllowAnyHeader()));

            services.AddSwaggerGen();

            services.AddSpaStaticFiles(opt => { opt.RootPath = "App"; });

            services.AddMqtt(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<BringAutoDbContext>();

                ctx.Database.Migrate(); //Auto migrate database. 

                scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>().SeedRolesAsync().GetAwaiter().GetResult();
                
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                userManager.SeedUsersAsync(scope.ServiceProvider.GetRequiredService<BringAutoDbContext>()).GetAwaiter().GetResult();

                try
                { ctx.SaveChanges(); }
                catch (DbUpdateException ex)
                {
                    Log.Error($"Unable to seed the database: {ex.Message}");
                }
            }


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BringAuto");
            });




            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<TenantMiddleware>();

            app.UseGraphQL("/graphql");
            app.UseSpaStaticFiles();
            app.UseSpa(opt =>
            {
                opt.Options.SourcePath = "App";
                opt.Options.DefaultPageStaticFileOptions = new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "App"))
                };
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
