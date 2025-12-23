using Core.Token.Models;
using Microsoft.Extensions.Options;

namespace Core.Token
{
    public interface IJWTHelper
    {
        public string GenerateJWTToken(JWTClaimItems claimItems);
        public string GenerateRefreshToken();
        public IOptions<JWTOptions> GetJWTOptions();
        public DateTime GetTokenExpiryDateTimeUtc();
        public DateTime GetRefreshTokenExpiryDateTimeUtc();
    }
}
