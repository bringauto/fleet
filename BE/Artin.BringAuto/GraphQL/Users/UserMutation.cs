using Artin.BringAuto.DAL;
using Artin.BringAuto.DAL.Models;
using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Ifaces;
using Artin.BringAuto.Shared.Users;
using AutoMapper;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Artin.BringAuto.GraphQL.Users
{
    public class UserMutation
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly ICurrentUserId currentUser;
        private readonly BringAutoDbContext dbContext;
        private readonly ICurrentTenant currentTenant;
        private readonly ICurrentRoles currentRoles;

        public UserMutation(RoleManager<IdentityRole> roleManager,
                            UserManager<ApplicationUser> userManager,
                            IMapper mapper,
                            ICurrentUserId currentUser,
                            BringAutoDbContext dbContext,
                            ICurrentTenant currentTenant,
                            ICurrentRoles currentRoles)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.mapper = mapper;
            this.currentUser = currentUser;
            this.dbContext = dbContext;
            this.currentTenant = currentTenant;
            this.currentRoles = currentRoles;
        }

        [Authorize(Roles = new[] { RoleNames.Admin, RoleNames.SuperAdmin })]
        public async Task<IdentityResult> Add(NewUser user)
        {

            if (user.Roles?.Any() != true && string.IsNullOrEmpty(user.NewTenantName))
                return IdentityResult.Failed(new IdentityError() { Code = "Unkown role", Description = "New tenant name od roles has to be defined" });

            //if (!String.IsNullOrWhiteSpace(user.NewTenantName) && !currentRoles.Roles.Contains(RoleNames.SuperAdmin))
            //    return IdentityResult.Failed(new IdentityError() { Code = "Forbidden", Description = "Only Super admin can add new tenant" });

            var appUser = mapper.Map<ApplicationUser>(user);

            var result = await userManager.CreateAsync(appUser, user.Password);
            if (result.Succeeded)
            {

                var tenantId = currentTenant.GetTenantId();
                if (!tenantId.HasValue)
                {
                    await userManager.DeleteAsync(appUser);
                    return IdentityResult.Failed(new IdentityError() { Code = "Unknown tenant", Description = "Specified tenant doesn't exist" });
                }

                if (!String.IsNullOrWhiteSpace(user.NewTenantName))
                {
                    try
                    {
                        tenantId = await CreateNewTenant(user.NewTenantName);
                    }
                    catch (Exception ex)
                    {
                        await userManager.DeleteAsync(appUser);
                        return IdentityResult.Failed(new IdentityError() { Code = "Tenant already exists", Description = ex.Message });
                    }
                    user.Roles ??= new List<string>();
                    user.Roles.Add(RoleNames.Admin);
                }

                dbContext.Add(new UserTenancy()
                {
                    UserId = appUser.Id,
                    TenantId = tenantId.Value
                });
                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    await userManager.DeleteAsync(appUser);
                    return IdentityResult.Failed(new IdentityError() { Code = "Unknown tenant", Description = "Specified tenant doesn't exist" });
                }

                var identityResult = await userManager.AddToRolesAsync(appUser, user.Roles);
                if (!identityResult.Succeeded)
                {
                    await dbContext.UserTenancy.Where(x => x.UserId == appUser.Id).ForEachAsync(x => dbContext.Remove(x));
                    await dbContext.SaveChangesAsync();
                    await userManager.DeleteAsync(appUser);
                    return identityResult;
                }


            }
            return result;
        }

        private async Task<int> CreateNewTenant(string name)
        {
            var tenant = await dbContext.Tenants.FirstOrDefaultAsync(x => x.Name == name);

            if (tenant is null)
            {
                tenant = new Tenant()
                {
                    Name = name
                };
                dbContext.Tenants.Add(tenant);
            }
            else
            {
                if (!await dbContext.UserTenancy.AnyAsync(x => x.UserId == currentUser.CurrentUserId && x.TenantId == tenant.Id))
                {
                    throw new ArgumentException("Current admin is not part of specified tenant");
                }
            }
            await dbContext.SaveChangesAsync();

            if (!await dbContext.UserTenancy.AnyAsync(x => x.UserId == currentUser.CurrentUserId && x.TenantId == tenant.Id))
            {
                var userTenancy = new UserTenancy()
                {
                    Tenant = tenant,
                    UserId = currentUser.CurrentUserId
                };
                dbContext.UserTenancy.Add(userTenancy);
                await dbContext.SaveChangesAsync();
            }
            return tenant.Id;
        }

        [Authorize(Roles = new[] { RoleNames.Admin, RoleNames.SuperAdmin })]
        public async Task<IdentityResult> Update(UpdateUser user)
        {
            var appUser = await userManager.FindByNameAsync(user.UserName);
            appUser = mapper.Map(user, appUser);
            var result = await userManager.UpdateAsync(appUser);
            if (result.Succeeded)
            {
                var roles = await userManager.GetRolesAsync(appUser);
                await userManager.RemoveFromRolesAsync(appUser, roles.Where(x => !user.Roles.Contains(x)));
                return await userManager.AddToRolesAsync(appUser, user.Roles.Where(r => !roles.Contains(r)));
            }
            return result;
        }

        [Authorize(Roles = new[] { RoleNames.Admin, RoleNames.SuperAdmin })]
        public async Task<IdentityResult> Delete(DeleteUser user, CancellationToken cancellationToken)
        {
            var appUser = await userManager.FindByNameAsync(user.UserName);
            IdentityResult result = null;
            var tenancy = dbContext.UserTenancy.FirstOrDefault(x => x.UserId == appUser.Id);
            if (tenancy is not null)
            {
                dbContext.UserTenancy.Remove(tenancy);
                await dbContext.SaveChangesAsync(cancellationToken);

                if (!dbContext.UserTenancy.IgnoreQueryFilters().Any(x => x.UserId == appUser.Id))
                {
                    var roles = await userManager.GetRolesAsync(appUser);
                    await userManager.RemoveFromRolesAsync(appUser, roles);
                    result = await userManager.DeleteAsync(appUser);
                }
            }
            else
                result = IdentityResult.Failed(new IdentityError() { Code = "NotTenant", Description = "User is not part of tenant" });
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
