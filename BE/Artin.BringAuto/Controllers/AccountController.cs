using Artin.BringAuto.DAL.Models;
using Artin.BringAuto.Shared.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
    //public class AccountController : ControllerBase
    //{
    //    private readonly UserManager<ApplicationUser> userManager;
    //    private readonly SignInManager<ApplicationUser> signInManager;
    //    private readonly IMapper mapper;

    //    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
    //    {
    //        this.userManager = userManager;
    //        this.signInManager = signInManager;
    //        this.mapper = mapper;
    //    }

    //    [HttpPost("login")]
    //    public async Task<IActionResult> LoginAsync([FromBody] Login login)
    //    {
    //        var user = await userManager.FindByNameAsync(login.UserName);
    //        if (user is ApplicationUser && await userManager.CheckPasswordAsync(user, login.Password))
    //        {
    //            await signInManager.SignInAsync(user, true);
    //            return Ok();
    //        }
    //        return BadRequest();
    //    }

    //    [HttpPost("logout")]
    //    public async Task<IActionResult> LogoutAsync()
    //    {
    //        await signInManager.SignOutAsync();
    //        return Ok();
    //    }

        
    //}
}
