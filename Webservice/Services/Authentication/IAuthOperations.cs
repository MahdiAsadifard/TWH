using Core.Response;
using Microsoft.AspNetCore.Http;
using Models.DTOs.Login;
using Models.Models;

namespace Services.Authentication
{
    public interface IAuthOperations
    {
        Task<ServiceResponse<UserRecord>> GetUsersByEmailAsync(string email);
        Task<ServiceResponse<LoginRsponseDTO>> ValidateUserLogin(UserRecord userRecord, LoginRequestDTO loginRequest);
        bool IsRefreshTokenValid(HttpRequest request, UserRecord userRecord);
    }
}
