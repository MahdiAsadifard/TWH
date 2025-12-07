using AutoMapper;
using Core.Common;
using Core.Exceptions;
using Core.NLogs;
using Core.Response;
using Database;
using Models.Common;
using Models.DTOs.User;
using Models.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Services.Interfaces;
using System.Net;

namespace Services.Collections
{
    public class UserOperations : IUserOperations
    {
        private readonly IMongoCollection<UserRecord> _user;
        private readonly IMapper _mapper;

        public byte[] GenerateHashPassword(string password, string salt)
        {
            var hashedPassword = CoreUtility.GenerateSHA256HashByte(password + salt);
            return hashedPassword;
        }

        public UserOperations(IDatabase<UserRecord> database, IMapper mapper)
        {
            _user = database.GetCollection(ModelConstants.CollectionNames.User.ToString());
            _mapper = mapper;
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
                var msg = $"UserOperations: Error on fetching user uri:${uri} - message {ex.Message}";
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

    }
}
