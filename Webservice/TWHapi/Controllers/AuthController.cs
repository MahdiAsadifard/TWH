using AutoMapper;
using Core.Exceptions;
using Core.Response;
using Core.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Login;
using Services.Authentication;
using Services.Interfaces;
using System.Net;

namespace TWHapi.Controllers
{
    [Authorize]
    [Route("api/{customerUri?}/auth")]
    public class AuthController(
            IConfiguration configuration,
            IMapper mapper,
            IUserOperations userOperations,
            IAuthOperations authOperations,
            IJWTHelper jwtHelper
        ) : BaseController
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IMapper _mapper = mapper;
        private readonly IUserOperations _userOperations = userOperations;
        private readonly IAuthOperations _authOperations = authOperations;
        private readonly IJWTHelper _jwtHelper = jwtHelper;

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<ServiceResponse<LoginRsponseDTO>> Login([FromBody] LoginRequestDTO submission)
        {
            try
            {
                var userResponse = await _authOperations.GetUsersByEmailAsync(submission.Email);
                if (!userResponse.IsSuccess)
                {
                    throw new ApiException(ApiExceptionCode.BadRequest, userResponse.Message, userResponse.StatusCode);
                }
                if (userResponse.Data is null)
                {
                    throw new ApiException(ApiExceptionCode.NotFound, "user not found", System.Net.HttpStatusCode.NotFound);
                }

                var validatePassword = await _authOperations.ValidateUserLogin(userResponse.Data, submission);
                if (!validatePassword.IsSuccess)
                {
                    throw new ApiException(ApiExceptionCode.BadRequest, validatePassword.Message, validatePassword.StatusCode);
                }

                return validatePassword;
            }
            catch (ApiException ex)
            {
                throw new ApiException(ex.ErrorCode, ex.Message, ex.StatusCode, ex);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentNullException($"Error on Login user. {e.Message} \n {e.InnerException?.Message}", e);
            }
            catch (Exception e)
            {
                throw new Exception("Error on Login user." + e.Message + e.InnerException?.Message, e);
            }
        }

        [HttpPost]
        [Route("resettokens")]
        public async Task<ServiceResponse<TokensDTO>> ResetTokens([FromRoute] string customerUri)
        {
            try
            {
                // Regenerate refresh token
                var userRecord = await _userOperations.RegenrateRefreshToken(customerUri);
                var newRefreshToken = userRecord.Data.RefreshToken.Token;

                // Generate new access token
                var newAccessToken = _jwtHelper.GenerateJWTToken(ProgramHelpers.Common.GetJwtClaimItems(userRecord.Data));

                var reponse = new TokensDTO()
                {
                    AccessToken = newAccessToken,
                    AccessTokenExpityUTC = _jwtHelper.GetTokenExpiryDateTimeUtc(),// TimeSpan.FromMinutes(Convert.ToDouble(_jwtHelper.GetJWTOptions().Value.RefreshTokenExpiryInMinutes)),
                    RefreshToken = newRefreshToken,
                    RefreshTokenExpityUTC = _jwtHelper.GetRefreshTokenExpiryDateTimeUtc(),// TimeSpan.FromMinutes(Convert.ToDouble(_jwtHelper.GetJWTOptions().Value.ExpiryInMinutes)),
                };

                return new ServiceResponse<TokensDTO>(reponse, HttpStatusCode.OK);
            }
            catch (ApiException e)
            {
                throw new ApiException(e.ErrorCode, e.Message, e.StatusCode, e);
            }
            catch (Exception e)
            {
                throw new Exception($"Error on reset tokens user: {customerUri}, \nMessage: {e.Message}, \nInnerException: {e.InnerException?.Message}", e);
            }
        }
    }
}
