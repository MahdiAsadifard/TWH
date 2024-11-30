using Models.DTOs.JWT;
using Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Login
{
    public class LoginRsponseDTO
    {
        public JwtCarrierDTO Token { get; set; }
        public UserResponseDTO User { get; set; }
    }
}
