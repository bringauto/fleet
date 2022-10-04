using Artin.BringAuto.Shared;
using Microsoft.AspNetCore.Identity;

namespace Artin.BringAuto.DAL.Models
{
    public class ApplicationUser : IdentityUser, IId<string>
    {
    }
}
