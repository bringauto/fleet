using Artin.BringAuto.DAL;
using Artin.BringAuto.DAL.Models;
using Artin.BringAuto.Shared;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.StartupParts
{
    public static class SeedUsers
    {
        public static async Task SeedUsersAsync(this UserManager<ApplicationUser> userManager, BringAutoDbContext context)
        {
            await CheckCreateUserAsync(context, userManager, RoleNames.Admin, RoleNames.Admin, RoleNames.Admin + "1", "BringAuto");
            await CheckCreateUserAsync(context, userManager, RoleNames.User, RoleNames.User, RoleNames.User + "1", "BringAuto");
            await CheckCreateUserAsync(context, userManager, RoleNames.Privileged, RoleNames.Privileged, RoleNames.Privileged + "1", "BringAuto");
            await CheckCreateUserAsync(context, userManager, RoleNames.Driver, RoleNames.Driver, RoleNames.Driver + "1", "BringAuto");

            await CheckCreateUserAsync(context, userManager, "BorsodChem", RoleNames.User, "bringauto", "BorsodChem");
            await CheckCreateUserAsync(context, userManager, "BorsodChemAdmin", RoleNames.Privileged, "bringautopilot", "BorsodChem");
            await CheckCreateUserAsync(context, userManager, "BringAuto", RoleNames.Driver, "bringautothebest", "BorsodChem");
        }



        private static async Task CheckCreateUserAsync(BringAutoDbContext context, UserManager<ApplicationUser> userManager, string user, string role, string pass, string tenant)
        {
            var exist = await userManager.FindByNameAsync(user);
            if (exist is null)
                await userManager.CreateUserAsync(user, role, pass);

            exist = await userManager.FindByNameAsync(user);

            var tenantEntry = context.Tenants.FirstOrDefault(x => x.Name == tenant);
            if (tenantEntry is null)
                tenantEntry = new Tenant() { Name = tenant };

            var tenantAssign = new UserTenancy()
            {
                UserId = exist.Id,
                Tenant = tenantEntry,
            };

            context.Add(tenantAssign);
            await context.SaveChangesAsync();

        }

        public static async Task CreateUserAsync(this UserManager<ApplicationUser> userManager, string username, string role, string pass)
        {
            ApplicationUser usr = new ApplicationUser
            {
                UserName = username,
                Email = $"NoRealMail{username}@penguin.cz",
                EmailConfirmed = true,
            };
            var result = await userManager.CreateAsync(usr, pass);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(usr, role);

        }

        public static async Task SeedRolesAsync(this RoleManager<IdentityRole> roleManager)
        {
            await CheckCreateRoleAsync(roleManager, RoleNames.Driver);
            await CheckCreateRoleAsync(roleManager, RoleNames.Admin);
            await CheckCreateRoleAsync(roleManager, RoleNames.User);
            await CheckCreateRoleAsync(roleManager, RoleNames.Privileged);
        }

        private static async Task CheckCreateRoleAsync(RoleManager<IdentityRole> roleManager, string role)
        {
            var exist = await roleManager.RoleExistsAsync(role);

            if (!exist)
                await roleManager.CreateRoleAsync(role);
        }

        public static async Task CreateRoleAsync(this RoleManager<IdentityRole> roleManager, string roleName)
        {
            var role = new IdentityRole
            {
                Name = roleName
            };
            await roleManager.CreateAsync(role);
        }
    }
}
