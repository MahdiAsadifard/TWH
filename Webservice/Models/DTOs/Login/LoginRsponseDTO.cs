using Models.DTOs.JWT;
using Models.DTOs.User;

namespace Models.DTOs.Login
{
    public class LoginRsponseDTO
    {
        public JwtCarrierDTO Token { get; set; }
        public UserResponseDTO User { get; set; }
    }
}
