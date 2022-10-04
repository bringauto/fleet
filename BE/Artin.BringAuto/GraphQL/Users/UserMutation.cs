using Artin.BringAuto.DAL.Models;
using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Users;
using AutoMapper;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.GraphQL.Users
{
    public class UserMutation
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public UserMutation(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<IdentityResult> Add(NewUser user)
        {
            var appUser = mapper.Map<ApplicationUser>(user);
            var result = await userManager.CreateAsync(appUser, user.Password);
            if (result.Succeeded)
            {
                var identityResult = await userManager.AddToRolesAsync(appUser, user.Roles);
                if (!identityResult.Succeeded)
                {
                    await userManager.DeleteAsync(appUser);
                    return identityResult;
                }
            }
            return result;
        }

        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<IdentityResult> Update(NewUser user)
        {
            var appUser = await userManager.FindByNameAsync(user.UserName);
            appUser = mapper.Map(user, appUser);
            var result = await userManager.UpdateAsync(appUser);
            if (result.Succeeded)
            {
                var roles = await userManager.GetRolesAsync(appUser);
                await userManager.RemoveFromRolesAsync(appUser, roles.Where(x => !user.Roles.Contains(x)));
                return await userManager.AddToRolesAsync(appUser, user.Roles);
            }
            return result;
        }

        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<IdentityResult> Delete(NewUser user)
        {
            var appUser = await userManager.FindByNameAsync(user.UserName);
            var roles = await userManager.GetRolesAsync(appUser);
            await userManager.RemoveFromRolesAsync(appUser, roles.Where(x => !user.Roles.Contains(x)));
            var result = await userManager.DeleteAsync(appUser);
            return result;
        }

        [Authorize(Roles = new[] { RoleNames.Admin, RoleNames.Driver, RoleNames.Privileged, RoleNames.User })]
        public async Task<IdentityResult> ChangePassword(ChangePass login)
        {
            var user = await userManager.FindByNameAsync(login.UserName);
            return await userManager.ChangePasswordAsync(user, login.Password, login.NewPassword);
        }
    }
}
