using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.DTO
{
    public class LoginDTO
    {
        public string user_name { get; set; }
        public string password { get; set; }
        public bool remember_me { get; set; } 
    }
}
