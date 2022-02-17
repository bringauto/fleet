using Artin.BringAuto.DAL.Models;
using Artin.BringAuto.MQTTClient;
using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Ifaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Artin.BringAuto.DAL
{
    public class BringAutoDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly int? tenantId;

        public BringAutoDbContext(
            DbContextOptions options,
            ICanAccessOrderProvider canAccessOrderProvider,
            ICurrentTenant tenancy) : base(options)
        {
            CanAccessOrderProvider = canAccessOrderProvider;
            this.tenantId = tenancy.GetTenantId();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>().HasQueryFilter(x => tenantId.HasValue && tenantId == x.TenantId && (CanAccessOrderProvider.CanAccessAllOrders || x.UserId == CanAccessOrderProvider.CurrentUserId));

            builder.Entity<LocationHistory>().HasIndex(x => x.Time);
            builder.Entity<LocationHistory>().HasIndex(x => new { x.Latitude, x.Longitude, x.Time });
            builder.Entity<Tenant>().HasIndex(x => x.Name).IsUnique();

            TenantFilter<Car>(builder);
            TenantFilter<Map>(builder);
            TenantFilter<Route>(builder);
            TenantFilter<RouteStop>(builder);
            TenantFilter<Station>(builder);
            builder.Entity<UserTenancy>().HasQueryFilter(x => tenantId.HasValue && tenantId == x.TenantId);

            base.OnModelCreating(builder);
        }

        private void TenantFilter<Tenancy>(ModelBuilder modelBuilder)
            where Tenancy : class, ITenancy
            => modelBuilder.Entity<Tenancy>().HasQueryFilter(x => tenantId.HasValue && tenantId == x.TenantId);

        public DbSet<Car> Cars { get; set; }
        public DbSet<Button> ButtonStates { get; set; }
        public DbSet<LocationHistory> LocationHistory { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Map> Maps { get; set; }

        public DbSet<Station> Stations { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<RouteStop> RouteStops { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<UserTenancy> UserTenancy { get; set; }
        public ICanAccessOrderProvider CanAccessOrderProvider { get; }


        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {


            foreach (var carEntity in this.ChangeTracker.Entries().Where(x => x.Entity is Car && x.State == EntityState.Modified))
            {
                if (carEntity.Entity is Car car)
                {
                    if (String.IsNullOrEmpty(car.CompanyName))
                    {
                        car.CompanyName = (carEntity.CurrentValues[nameof(Car.CompanyName)] = carEntity.OriginalValues[nameof(Car.CompanyName)])?.ToString();
                    }

                    if (String.IsNullOrEmpty(car.CallTwiml))
                    {
                        car.CallTwiml = (carEntity.CurrentValues[nameof(Car.CallTwiml)] = carEntity.OriginalValues[nameof(Car.CallTwiml)])?.ToString();
                    }

                    if (String.IsNullOrEmpty(car.CarAdminPhone))
                    {
                        car.CarAdminPhone = (carEntity.CurrentValues[nameof(Car.CarAdminPhone)] = carEntity.OriginalValues[nameof(Car.CarAdminPhone)])?.ToString();
                    }
                }
            }



            foreach (var entity in this.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified || x.State == EntityState.Added))
            {
                if (entity.Entity is ITenancy tenancy)
                {
                    if (!tenantId.HasValue)
                        throw new ArgumentException("Tenant is not specified");
                    tenancy.TenantId = tenantId;
                }
            }
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        }
    }
}
