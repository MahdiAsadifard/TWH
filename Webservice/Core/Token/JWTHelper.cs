using Core.Exceptions;
using Core.Token.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
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

        IOptions<JWTOptions> _options = options;

        private JWTClaimItems _claimItems { get; set; }

        #endregion

        #region public methods
        public string GenerateJWTToken(JWTClaimItems claimItems)
        {
            this._claimItems = claimItems;

            string token = string.Empty;
            if (Convert.ToBoolean(Convert.ToBoolean(_options.Value.EnableJWE)) == true)
            {
                var handler = new JsonWebTokenHandler();
                var descriptor = GetSecurityTokenDescriptor();
                token = handler.CreateToken(descriptor);
            }
            else
            {
                var securityToken = GetJwtSecurityToken();
                token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            }
            return token;
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
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
            return !string.IsNullOrWhiteSpace(_claimItems.FirstName) &&
                   !string.IsNullOrWhiteSpace(_claimItems.LastName) &&
                   !string.IsNullOrWhiteSpace(_claimItems.Uri) &&
                   !string.IsNullOrWhiteSpace(_claimItems.Email);
        }
       
        private List<Claim> GetClaims()
        {
            ArgumentsValidator.ThrowIfNull(nameof(_claimItems), _claimItems);
            if (!this.IsValidClaimItems())
            {
                throw new ArgumentNullException(nameof(_claimItems), "JWT Claim Items cannot have null or empty values.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _claimItems.FirstName),
                new Claim(ClaimTypes.Surname, _claimItems.LastName),
                new Claim(ClaimTypes.Uri, _claimItems.Uri),
                new Claim(ClaimTypes.Email, _claimItems.Email)
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
