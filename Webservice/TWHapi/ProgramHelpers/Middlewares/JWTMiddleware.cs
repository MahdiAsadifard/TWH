using Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using Core.NLogs;
using Core.Token;

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

                var expiryInMinutes = _configuration.GetSection($"{JWTOptions.OptionName}:{nameof(JWTOptions.ExpiryInMinutes)}").Get<string>();

                if (validatedToken.ValidTo < DateTime.UtcNow.AddMinutes(Convert.ToDouble(expiryInMinutes)))
                    throw new SecurityTokenNotYetValidException("Token has expired");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                
                var result = new
                {
                    message = "Unauthorized token",
                    status = context.Response.StatusCode,
                };
                await context.Response.WriteAsJsonAsync(result);

                NLogHelpers<JWTMiddleware>.Logger.Error( "Token validation failed. Invalid token provided. \nMessage: {Message}, \nTrace: {Trace}", e.Message, e.StackTrace);

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
                ValidAudience = _configuration.GetSection($"{JWTOptions.OptionName}:{nameof(JWTOptions.Audience)}").Get<string>(),
                ValidIssuer = _configuration.GetSection($"{JWTOptions.OptionName}:{nameof(JWTOptions.Issuer)}").Get<string>(),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection($"{JWTOptions.OptionName}:{nameof(JWTOptions.Key)}").Get<string>() ?? string.Empty))
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
