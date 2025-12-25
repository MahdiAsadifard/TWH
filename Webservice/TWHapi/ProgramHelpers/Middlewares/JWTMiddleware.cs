using Core.Exceptions;
using Core.NLogs;
using Core.Response;
using Core.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Models.Models;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace TWHapi.ProgramHelpers.Middlewares
{
    public class JWTMiddleware
    {
        private readonly IConfiguration _configuration;
        private readonly RequestDelegate _nextDelegate;

        private const string RefreshTokenFlag = "X-Refresh-Token";
        private const string CustomerUriFlag = "customerUri";
        private const string AuthorizationFlag = "Authorization";
        private const string CookieAccessTokenFlag = "newAccessToken";
        private const string CookieRefreshTokenFlag = "newRefreshToken";

        private IUserOperations _userOperations = null;
        private IJWTHelper _jwtHelper = null;
        private ServiceResponse<UserRecord> _userResponse = null;

        public JWTMiddleware(
            IConfiguration configuration,
            RequestDelegate nextDelegate)
        {
            this._configuration = configuration;
            this._nextDelegate = nextDelegate;
        }

        public async Task InvokeAsync(HttpContext context, IUserOperations userOperations, IJWTHelper jwtHelper)
        {
            ArgumentsValidator.ThrowIfNull(nameof(context), context);
            try
            {
                this._userOperations = userOperations;
                this._jwtHelper = jwtHelper;

                // No need to check token if endpoint marked with [AllowAnonymous] attribute
                if (this.IsAllowAnonymous(ref context))
                {
                    await _nextDelegate(context);
                    return;
                }

                // Check if request has refresh token, otherwise throw error 401
                this.IncludesRefreshToken(ref context);

                var token = this.GetAccessTokenFromHeader(ref context);

                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var validateParameters = GetValidationParameters();

                // validate access token
                jwtTokenHandler.ValidateToken(token, validateParameters, out var validatedAccessToken);

                // Check if access token is expired
                if (validatedAccessToken.ValidTo < DateTime.UtcNow)
                {
                    // Find customer by uri from db
                    await this.GetCustomerByUri(context);

                    this.RegenerateAccessToken(ref context);

                    var isValidRefrehToken = this.IsRefresfhTokenValid(ref context);
                    // Regenerate refresh token and check if provided refresh token is valid and belongs to the user
                    if (!isValidRefrehToken)
                        await this.RegenerateRefreshToken(context);

                    // Always return exception
                    throw new SecurityTokenNotYetValidException($"Access token {(isValidRefrehToken ? "" : "and refresh token ")} has expired.");
                }
            }
            catch (Exception e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                var result = new
                {
                    message = "Unauthorized token",
                    status = context.Response.StatusCode,
                    innerException = e.Message,
                };
                await context.Response.WriteAsJsonAsync(result);

                NLogHelpers<JWTMiddleware>.Logger.Error("Token validation failed. Invalid token provided. \nMessage: {Message}, \nTrace: {Trace}", e.Message, e.StackTrace);

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

        private bool IsAllowAnonymous(ref HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            var isAllow = endpoint is not null && endpoint.Metadata.GetMetadata<IAllowAnonymous>() is not null;

            return isAllow;
        }

        private bool IncludesRefreshToken(ref HttpContext context)
        {
            var refreshToken = this.GetRefreshTokenFromHeader(context);

            if (string.IsNullOrWhiteSpace(refreshToken)
                || refreshToken?.Length < _jwtHelper.GetJWTOptions().Value.RefreshTokenMinLength) // minimum: 40
            {
                throw new SecurityTokenExpiredException("Refresh token not found or invalid.");
            }

            return true;
        }

        private bool IsRefresfhTokenValid(ref HttpContext context)
        {
            ArgumentsValidator.ThrowIfNull(nameof(this._userResponse), this._userResponse);

            var savedToken = this._userResponse.Data!.RefreshToken;

            var requestedRefreshToken = this.GetRefreshTokenFromHeader(context);

            if (!savedToken.Token.Equals(requestedRefreshToken))
            {
                throw new SecurityTokenException("Requested refresh token is not belong to the user");
            }

            var delta = Convert.ToDateTime(this._userResponse.Data!.RefreshToken.ExpiryUtc) - DateTime.UtcNow;
            return delta > TimeSpan.Zero;
        }

        private string GetRefreshTokenFromHeader(HttpContext context)
        {
            return context.Request.Headers[RefreshTokenFlag].FirstOrDefault() ?? string.Empty;
        }

        private string GetAccessTokenFromHeader(ref HttpContext context)
        {
            var token = context.Request.Headers[AuthorizationFlag].FirstOrDefault()?.Split(' ').Last(); // remove Bearer
            if (token is null)
            {
                throw new EntryPointNotFoundException("Access Token not found");
            }
            return token;
        }

        private async Task GetCustomerByUri(HttpContext context)
        {
            this._userResponse = await this._userOperations.GetUserByUriAsync(this.GetCustomerUriFromRoute(ref context));
        }

        private string GetCustomerUriFromRoute(ref HttpContext context)
        {
            if (context.Request.RouteValues.TryGetValue(CustomerUriFlag, out object? customerId))
            {
                return customerId?.ToString() ?? string.Empty;
            }
            throw new KeyNotFoundException("CustomerUri not found in route");
        }

        private async Task RegenerateRefreshToken(HttpContext context)
        {
            var oldRefreshToken = this.GetRefreshTokenFromHeader(context);
            var refTokenUser = await _userOperations.RegenrateRefreshToken(this._userResponse.Data, oldRefreshToken);

            if (!refTokenUser.IsSuccess)
            {
                throw new SecurityTokenException("Failed to generate refresh token");
            }

            // Now update cookie with new refresh token
            context.Response.Cookies.Append(
                CookieRefreshTokenFlag,
                refTokenUser.Data.RefreshToken.Token,
                new CookieOptions
                {
                    MaxAge = TimeSpan.FromMinutes(Convert.ToDouble(_jwtHelper.GetJWTOptions().Value.RefreshTokenExpiryInMinutes))
                });
        }

        private void RegenerateAccessToken(ref HttpContext context)
        {
            var claim = new Core.Token.Models.JWTClaimItems
            {
                FirstName = this._userResponse.Data!.FirstName,
                LastName = this._userResponse.Data!.LastName,
                Uri = this._userResponse.Data!.Uri,
                Email = this._userResponse.Data!.Email,
            };

            var newAccessToken = _jwtHelper.GenerateJWTToken(claim);
            context.Response.Cookies.Append(
                CookieAccessTokenFlag,
                newAccessToken,
                new CookieOptions
                {
                    MaxAge = TimeSpan.FromMinutes(Convert.ToDouble(_jwtHelper.GetJWTOptions().Value.ExpiryInMinutes))
                });
        }
    }
}
