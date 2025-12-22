using Core.Token.Models;

namespace Core.Token
{
    public interface IJWTHelper
    {
        public string GenerateJWTToken(JWTClaimItems claimItems);

        public string GenerateRefreshToken();
    }
}
