using Core.Exceptions;
using Core.NLogs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Authentication
{
    public class JWTHelper(IConfiguration configuration) : IJWTHelper
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerateJWTToken(UserRecord user)
        {
            ArgumentsValidator.ThrowIfNull(nameof(user), user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Uri, user.Uri),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var jwtToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(_configuration.GetSection("JWT:expiry").Get<double>()),
                issuer: _configuration.GetSection("JWT:Issuer").Get<string>(),
                audience: _configuration.GetSection("JWT:Audience").Get<string>(),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Get<string>())
                    ),
                    SecurityAlgorithms.HmacSha256));
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            NLogHelpers<JwtHeader>.Logger.Info($"Token: {user.Uri}, {user.FirstName}:  {token}");
            return token;
        }
    }
}
