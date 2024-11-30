using Microsoft.AspNetCore.Mvc;
using System.Web;
using Services.Interfaces;
using AutoMapper;
using Models.DTOs.User;
using Core.NLogs;
using Core.Response;
using Core.Exceptions;
using Services.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace TWHapi.Controllers
{
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly IJWTHelper _jwtHelper;
        private readonly IUserOperations _user;
        private readonly IMapper _mapper;
        public UserController(IUserOperations user, IMapper mapper, IJWTHelper jwhHelper)
        {
            _user = user;
            _mapper = mapper;
            this._jwtHelper = jwhHelper;
        }

        [Authorize]
        [Route("")]
        [HttpGet]
        public async Task<ServiceResponse<IEnumerable<UserResponseDTO>>> GetUsersAsync()
        {
            var response = await _user.GetUsersAsync();

            if (!response.IsSuccess) return new ServiceResponse<IEnumerable<UserResponseDTO>>(response.Message, response.StatusCode);

            //NLogHelpers<UserOperations>.Logger.Info(CoreUtility.SerializeJson(user));

            var dto = _mapper.Map<IEnumerable<UserResponseDTO>>(response.Data);
            return new ServiceResponse<IEnumerable<UserResponseDTO>>(dto);
        }

        [Authorize]
        [Route("{uri}")]
        [HttpGet]
        public async Task<ServiceResponse<UserResponseDTO>> GetUsersByUriAsync([FromRoute] string uri)
        {
            try
            {
                var response = await _user.GetUserByUriAsync(uri);

               if (!response.IsSuccess) return new ServiceResponse<UserResponseDTO>(response.Message, response.StatusCode);

                var dto = _mapper.Map<UserResponseDTO>(response.Data);
               return new ServiceResponse<UserResponseDTO>(dto);
            }
            catch (ApiException e)
            {
                throw new ApiException(e.ErrorCode, e.Message, e.StatusCode, e);
            }
            catch (Exception e)
            {
                throw new Exception($"UserController/GetUsersByUriAsync: Error on getting user uri: {uri}, message: {e.Message}");
            }
        }

    
        [HttpPost("")]
        public async Task<ServiceResponse<UserResponseDTO>> InsertOneAsync([FromBody]UserRequestDTO submission)
        {
            try
            {
                var response = await _user.InsertOneAsync(submission);


                if (!response.IsSuccess) return new ServiceResponse<UserResponseDTO>(response.Message, response.StatusCode);

                var dto = _mapper.Map<UserResponseDTO>(response.Data);
                return new ServiceResponse<UserResponseDTO>(dto);
            }
            catch (Exception ex)
            {
                var msg = $"UserController/InsertOneAsync: Error on adding new user: {ex.Message} - \n trace:\n {ex}";
                NLogHelpers<UserController>.Logger.Info(msg);
                throw new Exception("Error: " + ex.Message);
            }
        }
    }
}
