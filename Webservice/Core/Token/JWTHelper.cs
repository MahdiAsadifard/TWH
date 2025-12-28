using Core.Exceptions;
using Core.Token.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Buffers.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Core.Token
{
    public class JWTHelper(
            IOptions<JWTOptions> options
        ) : IJWTHelper
    {
        #region properties and Fields

        private readonly IOptions<JWTOptions> _options = options;

        private JWTClaimItems ClaimItems { get; set; }

        #endregion

        #region public methods
        public string GenerateJWTToken(JWTClaimItems claimItems)
        {
            this.ClaimItems = claimItems;

            if (Convert.ToBoolean(Convert.ToBoolean(_options.Value.EnableJWE)) == true)
            {
                var handler = new JsonWebTokenHandler();
                var descriptor = GetSecurityTokenDescriptor();
                return handler.CreateToken(descriptor);
            }

            var securityToken = GetJwtSecurityToken();
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public string GenerateRefreshToken()
        {
            //return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            //return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
            return Base64Url.EncodeToString(RandomNumberGenerator.GetBytes(32));
        }

        public IOptions<JWTOptions> GetJWTOptions()
        {
            return _options;
        }

        public DateTime GetTokenExpiryDateTimeUtc()
        {
            return DateTime.UtcNow.AddMinutes(Convert.ToDouble(_options.Value.ExpiryInMinutes));
        }

        public DateTime GetRefreshTokenExpiryDateTimeUtc()
        {
            return DateTime.UtcNow.AddMinutes(Convert.ToDouble(_options.Value.RefreshTokenExpiryInMinutes));
        }

        #endregion

        #region private methods
        private bool IsValidClaimItems()
        {
            return !string.IsNullOrWhiteSpace(ClaimItems.FirstName) &&
                   !string.IsNullOrWhiteSpace(ClaimItems.LastName) &&
                   !string.IsNullOrWhiteSpace(ClaimItems.Uri) &&
                   !string.IsNullOrWhiteSpace(ClaimItems.Email);
        }

        private List<Claim> GetClaims()
        {
            ArgumentsValidator.ThrowIfNull(nameof(ClaimItems), ClaimItems);
            if (!this.IsValidClaimItems())
            {
                throw new ArgumentNullException(nameof(ClaimItems), "JWT Claim Items cannot have null or empty values.");
            }

            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, ClaimItems.FirstName),
                new (ClaimTypes.Surname, ClaimItems.LastName),
                new (ClaimTypes.Uri, ClaimItems.Uri),
                new (ClaimTypes.Email, ClaimItems.Email)
            };
            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var securityKey = Encoding.UTF8.GetBytes(_options.Value.Key);
            var credentials = new SymmetricSecurityKey(securityKey);
            return new SigningCredentials(credentials, SecurityAlgorithms.HmacSha256);
        }

        private JwtSecurityToken GetJwtSecurityToken()
        {
            return new JwtSecurityToken(
                claims: GetClaims(),
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_options.Value.ExpiryInMinutes)),
                issuer: _options.Value.Issuer,
                audience: _options.Value.Audience,
                signingCredentials: GetSigningCredentials());
        }

        private SecurityTokenDescriptor GetSecurityTokenDescriptor()
        {
            return new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(GetClaims()),
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_options.Value.ExpiryInMinutes)),
                Issuer = _options.Value.Issuer,
                Audience = _options.Value.Audience,
                SigningCredentials = GetSigningCredentials(),
            };
        }

        #endregion

    }
}
