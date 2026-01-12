using AutoMapper;
using Core.Common;
using Core.Exceptions;
using Core.NLogs;
using Core.Response;
using Core.Token;
using Database.Mongodb;
using Database.Redis;
using Models.Common;
using Models.DTOs.User;
using Models.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Services.Interfaces;
using System.Net;
using System.Threading.Tasks;

namespace Services.Collections
{
    public class UserOperations : IUserOperations
    {
        private readonly IMongoCollection<UserRecord> _user;
        private readonly IMapper _mapper;
        private readonly IJWTHelper _jwtHelper;
        private readonly IRedisServices _redisServices;

        public byte[] GenerateHashPassword(string password, string salt)
        {
            var hashedPassword = CoreUtility.GenerateSHA256HashByte(password + salt);
            return hashedPassword;
        }

        public UserOperations(
            IDatabase<UserRecord> database,
            IMapper mapper,
            IJWTHelper jwtHelper,
            IRedisServices redisServices)
        {
            this._user = database.GetCollection(ModelConstants.CollectionNames.User.ToString());
            this._mapper = mapper;
            this._jwtHelper = jwtHelper;
            this._redisServices = redisServices;
        }

        public async Task<ServiceResponse<IEnumerable<UserRecord>>> GetUsersAsync(
            IEnumerable<FilterDefinition<UserRecord>> additionalFilters = null,
            string? quickFind = null,
            int limit = 25,
            int skip = 0
        )
        {
            try
            {
                FilterDefinitionBuilder<UserRecord> predictBuilder = Builders<UserRecord>.Filter;
                List<FilterDefinition<UserRecord>> predicts = new List<FilterDefinition<UserRecord>>()
                {
                    predictBuilder.Or(
                        predictBuilder.Eq(z => z.Disabled, false),
                        predictBuilder.Exists(z => z.Disabled, true)
                        )
                };

                List<FilterDefinition<UserRecord>> quickFindSet = new();
                if (!string.IsNullOrWhiteSpace(quickFind))
                {
                    foreach (char q in quickFind)
                    {
                        quickFindSet.Add(
                            predictBuilder.Or(
                                    predictBuilder.Regex(z => z.FirstName, $"/^{q}.*/i"),
                                    predictBuilder.Regex(z => z.LastName, $"/^{q}.*/i"),
                                    predictBuilder.Regex(z => z.Email, $"/^{q}.*/i")
                                )
                            );
                    }
                }


                if (additionalFilters is not null)
                {
                    predicts.AddRange(additionalFilters);
                }

                NLogHelpers<UserOperations>.Logger.Info("UserOperations: User Search...");

                var result = _user.Find<UserRecord>(
                        predictBuilder?.And(predicts?.ToArray())
                    )
                    .Sort(Builders<UserRecord>.Sort.Ascending(x => x.Email))
                    .Limit(limit)
                    .Skip(skip)
                    .ToList();
                return new ServiceResponse<IEnumerable<UserRecord>>(result);
            }
            catch (Exception ex)
            {
                var msg = $"UserOperations: Error on fetching users Limit:${limit}, Skip: {skip} - message {ex.Message}";
                NLogHelpers<UserOperations>.Logger.Error(msg);
                return new ServiceResponse<IEnumerable<UserRecord>>(msg, HttpStatusCode.BadRequest);
            }
        }

        public async Task<ServiceResponse<UserRecord>> GetUserByUriAsync(string uri)
        {
            try
            {
                var filters = new List<FilterDefinition<UserRecord>>();
                filters.Add(Builders<UserRecord>.Filter.Eq(x => x.Uri, uri));
                var users = await this.GetUsersAsync(filters);

                if (users.Data.Count() == 0)
                {
                    throw new ApiException(ApiExceptionCode.NotFound, "User not found", HttpStatusCode.NotFound);
                }

                if (users.Data.Count() > 1)
                {
                    var msg = $"Found multiple user with same uri: {uri}";
                    NLogHelpers<UserOperations>.Logger.Error(msg);
                    throw new ApiException(ApiExceptionCode.Conflict, msg, HttpStatusCode.Conflict);
                }

                return new ServiceResponse<UserRecord>(users.Data.FirstOrDefault());
            }
            catch (ApiException ex)
            {
                var msg = $"UserOperations: Error on fetching user uri:{uri} - message {ex.Message}";
                NLogHelpers<UserOperations>.Logger.Error(msg);
                throw new ApiException(ApiExceptionCode.Conflict, msg, HttpStatusCode.Conflict);
            }
            catch (Exception ex)
            {
                var msg = $"UserOperations: Error on fetching user uri:${uri} - message {ex.Message}";
                NLogHelpers<UserOperations>.Logger.Error(msg);
                throw new ApiException(ApiExceptionCode.InternalServerError, msg, HttpStatusCode.InternalServerError, ex);
            }
        }

        public async Task<ServiceResponse<UserRecord>> InsertOneAsync(UserRequestDTO submission)
        {
            try
            {
                ArgumentsValidator.ThrowIfNull(submission.Password, nameof(submission.Password));

                string salt = Guid.NewGuid().ToString();
                var userRecord = _mapper.Map<UserRecord>(submission);

                userRecord._Id = ObjectId.GenerateNewId();
                userRecord.Salt = salt;
                userRecord.HashPassword = GenerateHashPassword(submission.Password, salt);

                await _user.InsertOneAsync(userRecord);
                return new ServiceResponse<UserRecord>(userRecord, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                var msg = $"UserOperations: Error on adding new user: {ex.Message}";
                NLogHelpers<UserOperations>.Logger.Error(msg);
                return new ServiceResponse<UserRecord>(msg, HttpStatusCode.BadRequest);
            }
        }

        public async Task<ServiceResponse<UserRecord>> UpdateOneAsync(UserRecord userRecord)
        {
            try
            {
                ArgumentsValidator.ThrowIfNull(nameof(userRecord), userRecord);
                var filter = Builders<UserRecord>.Filter.Eq(x => x._Id, userRecord._Id);
                var result = await _user.ReplaceOneAsync(filter, userRecord);
                if (result.IsAcknowledged && result.ModifiedCount > 0)
                {
                    return new ServiceResponse<UserRecord>(userRecord, HttpStatusCode.OK);
                }
                else
                {
                    return new ServiceResponse<UserRecord>("No document updated.", HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                var msg = $"UserOperations: Error on updating user: {ex.Message}";
                NLogHelpers<UserOperations>.Logger.Error(msg);
                throw new ApiException(ApiExceptionCode.BadRequest, msg, HttpStatusCode.BadRequest);
            }
        }

        public async Task<ServiceResponse<UserRecord>> RegenrateRefreshToken(string uri)
        {
            ArgumentsValidator.ThrowIfNull(nameof(uri), uri);

            var response = await this.GetUserByUriAsync(uri);

            if (!response.IsSuccess)
            {
                return response;
            }

            response.Data.RefreshToken = this.GetNewRefreshToken();
            var updateResult = await this.UpdateOneAsync(response.Data);

            if (!updateResult.IsSuccess)
            {
                return new ServiceResponse<UserRecord>(updateResult.Message, updateResult.StatusCode);
            }
            return new ServiceResponse<UserRecord>(response.Data, HttpStatusCode.OK);
        }

        /// <summary>
        /// Compare requested token with current then regenerate new one
        /// </summary>
        /// <param name="userRecord"></param>
        /// <param name="requestedToken"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public async Task<ServiceResponse<UserRecord>> RegenrateRefreshToken(UserRecord userRecord, string requestedToken)
        {
            ArgumentsValidator.ThrowIfNull(nameof(UserRecord), userRecord);
            ArgumentsValidator.ThrowIfNull(nameof(requestedToken), requestedToken);

            if (!userRecord.RefreshToken.Token.Equals(requestedToken))
            {
                throw new ApiException(ApiExceptionCode.Unauthorized, "Invalid old refresh token. Try to signing again", HttpStatusCode.Unauthorized);
            }
            return await this.RegenrateRefreshToken(userRecord.Uri);
        }

        public UserRefreshToken GetNewRefreshToken()
        {
            return new UserRefreshToken()
            {
                Token = this._jwtHelper.GenerateRefreshToken(),
                ExpiryUtc = this._jwtHelper.GetRefreshTokenExpiryDateTimeUtc(),
            };
        }

    }
}
