using Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace TWHapi.ProgramHelpers.Middlewares
{
    public class JWTMiddleware
    {
        private readonly IConfiguration _configuration;
        private readonly RequestDelegate _nextDelegate;

        public JWTMiddleware(
            IConfiguration configuration,
            RequestDelegate nextDelegate)
        {
            this._configuration = configuration;
            this._nextDelegate = nextDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            ArgumentsValidator.ThrowIfNull(nameof(context), context);

            try
            {
                // No need to check token if endpoint marked with [AllowAnonymous] attribute
                if (IsAllowAnonymous(context))
                {
                    await _nextDelegate(context);
                    return;
                }

                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last(); // remove Bearer
                if (token is null)
                {
                    throw new Exception("Token not found");
                }

                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var validateParameters = GetValidationParameters();

                jwtTokenHandler.ValidateToken(token, validateParameters, out var validatedToken);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync($"Token validation failed. Invalid token provided. \nTrace: {e.Message}");
                return;
            }
            await _nextDelegate(context);
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false,
                RequireExpirationTime = true,
                ValidateIssuer = true,
                ValidAudience = _configuration.GetSection("JWT:Audience").Get<string>(),
                ValidIssuer = _configuration.GetSection("JWT:Issuer").Get<string>(),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Get<string>()))
            };
        }

        private bool IsAllowAnonymous(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            var isAllow = endpoint is not null && endpoint.Metadata.GetMetadata<IAllowAnonymous>() is not null;

            return isAllow;
        }
    }
}
