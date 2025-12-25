using Core.Token;
using Models.DTOs.User;

namespace Models.DTOs.Login
{
    public record LoginRsponseDTO
    {
        public required JwtCarrierDTO Token { get; set; }
        public required UserResponseDTO User { get; set; }
    }
}
