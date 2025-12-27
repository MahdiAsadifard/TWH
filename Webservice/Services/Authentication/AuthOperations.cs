using AutoMapper;
using Core.Common;
using Core.Exceptions;
using Core.NLogs;
using Core.Response;
using Core.Token;
using Core.Token.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.DTOs.Login;
using Models.DTOs.User;
using Models.Models;
using MongoDB.Driver;
using Services.Collections;
using Services.Interfaces;
using System;
using System.Net;

namespace Services.Authentication
{
    public class AuthOperations(
            IConfiguration configuration,
            IUserOperations userOperations,
            IJWTHelper jwtHelper,
            IMapper mapper
        ) : IAuthOperations
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserOperations _userOperations = userOperations;
        private readonly IJWTHelper _jWTHelper = jwtHelper;
        private readonly IMapper _mapper = mapper;

        public async Task<ServiceResponse<UserRecord>> GetUsersByEmailAsync(string email)
        {
            _mapper.Map<string>(email);
            try
            {
                var filters = new List<FilterDefinition<UserRecord>>()
                {
                    Builders<UserRecord>.Filter.Eq(x => x.Email, email.ToLower())
                };

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

            // Save refresh token
            userRecord = _userOperations.RegenrateRefreshToken(userRecord.Uri).Result.Data;
            
            var uderDto = _mapper.Map<UserResponseDTO>(userRecord);

            var claimItems = new JWTClaimItems
            {
                FirstName = userRecord.FirstName,
                LastName = userRecord.LastName,
                Uri = userRecord.Uri,
                Email = userRecord.Email
            };

            var reponse = new LoginRsponseDTO
            {
                Token = new TokensDTO
                {
                    AccessToken = _jWTHelper.GenerateJWTToken(claimItems),
                    AccessTokenExpityUTC = _jWTHelper.GetTokenExpiryDateTimeUtc(),
                    RefreshToken = userRecord.RefreshToken.Token,
                    RefreshTokenExpityUTC = _jWTHelper.GetRefreshTokenExpiryDateTimeUtc(),
                },
                User = uderDto
            };
            return new ServiceResponse<LoginRsponseDTO>(reponse);
        }

        public bool IsRefreshTokenValid(HttpRequest request, UserRecord userRecord)
        {
            ArgumentsValidator.ThrowIfNull(nameof(request), request);

            var savedToken = userRecord.RefreshToken;

            var requestedRefreshToken = request.Headers[TokenConstants.RefreshTokenFlag].FirstOrDefault() ?? string.Empty;

            if (!savedToken.Token.Equals(requestedRefreshToken))
            {
                throw new SecurityTokenException("Requested refresh token is not belong to the user");
            }

            var delta = Convert.ToDateTime(userRecord.RefreshToken.ExpiryUtc) - DateTime.UtcNow;
            return delta > TimeSpan.Zero;
        }
    }
}
