using AutoMapper;
using Core.Exceptions;
using Core.Response;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Login;
using Models.DTOs.User;
using Models.Models;
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

        [HttpPost]
        [Route("login")]
        public async Task<ServiceResponse<LoginRsponseDTO>> Login([FromBody] LoginRequestDTO submission)
        {
            try
            {
                var userResponse = await _authOperations.GetUsersByEmailAsync(submission.Email);
                if (!userResponse.IsSuccess)
                {
                    return new ServiceResponse<LoginRsponseDTO>(userResponse.Message, userResponse.StatusCode);
                }

                var validatePassword = await _authOperations.ValidateUserLogin(userResponse.Data, submission);
                if (!validatePassword.IsSuccess)
                {
                    return new ServiceResponse<LoginRsponseDTO>(validatePassword.Message, validatePassword.StatusCode);
                }

                return validatePassword;
            }
            catch (ApiException ex)
            {
                throw new ApiException(ex.ErrorCode, ex.Message, ex.StatusCode, ex);
            }
            catch (Exception e)
            {
                throw new Exception("Error on Login user.", e);
            }
        }
    }
}
