using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NesivosHakodesh.Providers.Identity
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public int UserId { get; set; }
        public string Token { get; set; }
        public string OldPassword { get; set; }
    }
}
