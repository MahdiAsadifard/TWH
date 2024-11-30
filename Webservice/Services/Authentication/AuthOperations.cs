using Core.Exceptions;
using Core.NLogs;
using Core.Response;
using Models.Models;
using MongoDB.Driver;
using Services.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Core.Common;
using Models.DTOs.Login;
using Microsoft.Extensions.Configuration;
using Models.DTOs.JWT;

namespace Services.Authentication
{
    public class AuthOperations : IAuthOperations
    {
        private readonly IConfiguration _configuration;
        private readonly IUserOperations _userOperations;
        private readonly IJWTHelper _jWTHelper;

        public AuthOperations(
            IConfiguration configuration,
            IUserOperations userOperations,
            IJWTHelper jwtHelper)
        {
            this._userOperations = userOperations;
            this._jWTHelper = jwtHelper;
            this._configuration = configuration;
        }
        public async Task<ServiceResponse<UserRecord>> GetUsersByEmailAsync(string email)
        {
            try
            {
                var filters = new List<FilterDefinition<UserRecord>>();
                filters.Add(Builders<UserRecord>.Filter.Eq(x => x.Email, email));

                var users = await _userOperations.GetUsersAsync(filters);

                if (users.Data.Count() > 1)
                {
                    var msg = $"AuthOperations/GetUsersByEmailAsync: Found multiple user with same email: {email}";
                    NLogHelpers<UserOperations>.Logger.Error(msg);
                    throw new ApiException(ApiExceptionCode.Conflict, msg, HttpStatusCode.Conflict);
                }

                return new ServiceResponse<UserRecord>(users.Data.FirstOrDefault());
            }
            catch (ApiException ex)
            {
                var msg = $"AuthOperations/GetUsersByEmailAsync:  Error on fetching user email:${email} - message {ex.Message}";
                NLogHelpers<UserOperations>.Logger.Error(msg);
                throw new ApiException(ApiExceptionCode.Conflict, msg, HttpStatusCode.Conflict);
            }
            catch (Exception ex)
            {
                var msg = $"AuthOperations/GetUsersByEmailAsync: : Error on fetching user email:${email} - message {ex.Message}";
                NLogHelpers<UserOperations>.Logger.Error(msg);
                throw new ApiException(ApiExceptionCode.InternalServerError, msg, HttpStatusCode.InternalServerError, ex);
            }
        }

        public async Task<ServiceResponse<LoginRsponseDTO>> ValidateUserLogin(UserRecord userRecord, LoginRequestDTO loginRequest)
        {
            ArgumentsValidator.ThrowIfNull(nameof(userRecord), userRecord);
            ArgumentsValidator.ThrowIfNull(nameof(loginRequest), loginRequest);

            var hashedPassword = CoreUtility.GenerateSHA256HashByte(loginRequest.Password + userRecord.Salt);
            if (!userRecord.HashPassword.SequenceEqual(hashedPassword))
            {
                throw new ApiException(ApiExceptionCode.Unauthorized, "Provided wrong password.", HttpStatusCode.Unauthorized);
            }

            var token = _jWTHelper.GenerateJWTToken(userRecord);
            
            var reponse = new LoginRsponseDTO
            {
                FirstName = userRecord.FirstName,
                Lastname = userRecord.LastName,
                Token = new JwtCarrierDTO
                {
                    AccessToken = token,
                    expiry = _configuration.GetSection("JWT:expiry").Get<string>(),
                }
            };
            return new ServiceResponse<LoginRsponseDTO>(reponse);
        }
    }
}
