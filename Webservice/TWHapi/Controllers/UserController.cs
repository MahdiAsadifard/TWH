using AutoMapper;
using Core.Exceptions;
using Core.ILogs;
using Core.NLogs;
using Core.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.User;
using Services.Interfaces;
using Core.Token;
using Models.DTOs.FileSystem;

namespace TWHapi.Controllers
{
    [Authorize]
    [Route("api/{customerUri?}/user")]
    public class UserController(
            IUserOperations user,
            IMapper mapper,
            IJWTHelper jwhHelper,
            ILoggerHelpers<UserController> logger,
            IUserUploads userUpload
        ) : BaseController
    {
        private readonly IJWTHelper _jwtHelper = jwhHelper;
        private readonly IUserOperations _user = user;
        private readonly IMapper _mapper = mapper;
        private readonly ILoggerHelpers<UserController> _logger = logger;
        private readonly IUserUploads _userUpload = userUpload;

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

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ServiceResponse<UserResponseDTO>> InsertOneAsync([FromBody] UserRequestDTO submission)
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
                var msg = $"UserController/InsertOneAsync: Error on adding new user: {ex.Message} - \n trace:\n {ex.StackTrace}";
                NLogHelpers<UserController>.Logger.Info(msg);
                throw new Exception("Error: " + ex.Message);
            }
        }

        [HttpPost("upload")]
        public async Task<ServiceResponse<IFormFile>> UploadProfileImage(IFormFile file, [FromRoute] string customerUri)
        {
            var uploadedFile = this._userUpload
                .SetFile(file)
                .SetUserUri(customerUri)
                .ProfileImage();
            return new ServiceResponse<IFormFile>(uploadedFile);
        }
        
        [HttpGet("profileimage")]
        public async Task<ServiceResponse<ImageDTO>> GetProfileImage([FromRoute] string customerUri)
        {
            var file = this._userUpload.GetUserProfileImage(customerUri);
            return new ServiceResponse<ImageDTO>(file, System.Net.HttpStatusCode.OK);
        }
    }
}
