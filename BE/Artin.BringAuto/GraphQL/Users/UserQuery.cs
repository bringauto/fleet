using Artin.BringAuto.DAL;
using Artin.BringAuto.DAL.Models;
using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Tenants;
using Artin.BringAuto.Shared.Users;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.GraphQL.Users
{
    public class UserQuery
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly BringAutoDbContext dbContext;
        private readonly ITenantRepository tenantRepository;
        private readonly IMapper mapper;

        public UserQuery(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            BringAutoDbContext dbContext,
            ITenantRepository tenantRepository,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
            this.tenantRepository = tenantRepository;
            this.mapper = mapper;
        }

        public async Task<User> GetLogin(Login login)
        {
            var user = await userManager.FindByNameAsync(login.UserName);
            if (user is ApplicationUser && await userManager.CheckPasswordAsync(user, login.Password))
            {
                await signInManager.SignInAsync(user, true);
                var result = mapper.Map<User>(user);
                result.Roles = await userManager.GetRolesAsync(user);
                return result;
            }

            return null;
        }

        [UsePaging]
        [UseFiltering]
        [UseSorting]
        [UseSelection]
        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<List<User>> GetAll()
        {
            return await dbContext.UserTenancy.Select(x=>x.User).ProjectTo<User>(mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<bool> GetLogout()
        {
            await signInManager.SignOutAsync();
            return true;
        }

        [Authorize(Roles = new[] { RoleNames.Admin, RoleNames.Driver, RoleNames.Privileged, RoleNames.User })]
        public async Task<User> GetMe([Service] IHttpContextAccessor httpContextAccessor)
        {
            var userDal = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);

            var user = mapper.Map<User>(userDal);
            user.Roles = await userManager.GetRolesAsync(userDal);
            return user;
        }
    }
}
