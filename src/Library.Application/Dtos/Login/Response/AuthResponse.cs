using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Dtos.Login.Response
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string Username { get; set; } 
        public string Role { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
