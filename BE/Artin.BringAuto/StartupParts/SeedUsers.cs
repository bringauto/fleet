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
        public static async Task SeedUsersAsync(this UserManager<ApplicationUser> userManager)
        {
            await CheckCreateUserAsync(userManager, RoleNames.Admin, RoleNames.Admin);
            await CheckCreateUserAsync(userManager, RoleNames.User, RoleNames.User);
            await CheckCreateUserAsync(userManager, RoleNames.Privileged, RoleNames.Privileged);
            await CheckCreateUserAsync(userManager, RoleNames.Driver, RoleNames.Driver);

            await CheckCreateUserAsync(userManager, "BorsodChem", RoleNames.User, "bringauto");
            await CheckCreateUserAsync(userManager, "BorsodChemAdmin", RoleNames.Privileged, "bringautopilot");
            await CheckCreateUserAsync(userManager, "BringAuto", RoleNames.Driver, "bringautothebest");
        }



        private static async Task CheckCreateUserAsync(UserManager<ApplicationUser> userManager, string user, string role, string pass = null)
        {
            var exist = await userManager.FindByNameAsync(user);
            if (exist is null)
                await userManager.CreateUserAsync(user, role, pass ?? user + "1");
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
