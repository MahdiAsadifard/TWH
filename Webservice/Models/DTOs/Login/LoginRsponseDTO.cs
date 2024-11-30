using Models.DTOs.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Login
{
    public class LoginRsponseDTO
    {
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public JwtCarrierDTO Token { get; set; }
    }
}
