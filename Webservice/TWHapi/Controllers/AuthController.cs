using AutoMapper;
using Core.Exceptions;
using Core.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Login;
using Services.Authentication;
using Services.Interfaces;

namespace TWHapi.Controllers
{
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUserOperations _userOperations;
        private readonly IAuthOperations _authOperations;

        public AuthController(
            IConfiguration configuration,
            IMapper mapper,
            IUserOperations userOperations,
            IAuthOperations authOperations)
        {
            this._configuration = configuration;
            this._mapper = mapper;
            this._userOperations = userOperations;
            this._authOperations = authOperations;
        }

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
    }
}
