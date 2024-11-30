using Core.Response;
using Models.DTOs.Login;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Authentication
{
    public interface IAuthOperations
    {
        Task<ServiceResponse<UserRecord>> GetUsersByEmailAsync(string email);
        Task<ServiceResponse<LoginRsponseDTO>> ValidateUserLogin(UserRecord userRecord, LoginRequestDTO loginRequest);
    }
}
