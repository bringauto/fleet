using Artin.BringAuto.Shared;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Artin.BringAuto.DAL.Models
{
    public class ApplicationUser : IdentityUser, IId<string>
    {
    }
}
