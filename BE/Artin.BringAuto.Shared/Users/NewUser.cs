using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Users
{
    public class UpdateUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<String> Roles { get; set; }
    }

    public class NewUser : UpdateUser
    {

        public string NewTenantName { get; set; }
    }

    public class DeleteUser
    {
        public string UserName { get; set; }

    }
}
