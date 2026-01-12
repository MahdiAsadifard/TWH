using Core.Response;
using Models.DTOs.User;
using Models.Models;
using MongoDB.Driver;

namespace Services.Interfaces
{
    public interface IUserOperations
    {
        Task<ServiceResponse<IEnumerable<UserRecord>>> GetUsersAsync(
            IEnumerable<FilterDefinition<UserRecord>> additionalFilters = null,
            string? quickFind = null,
            int limit = 25,
            int skip = 0);

        Task<ServiceResponse<UserRecord>> GetUserByUriAsync(string uri);
        Task<ServiceResponse<UserRecord>> InsertOneAsync(UserRequestDTO user);
        Task<ServiceResponse<UserRecord>> UpdateOneAsync(UserRecord userRecord);

        /// <summary>
        /// Always regenrate new token
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<ServiceResponse<UserRecord>> RegenrateRefreshToken(string uri);

        /// <summary>
        /// Compare requested token with current then regenerate new one
        /// </summary>
        /// <param name="userRecord"></param>
        /// <param name="requestedToken"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        Task<ServiceResponse<UserRecord>> RegenrateRefreshToken(UserRecord userRecord, string requestedToken);
        UserRefreshToken GetNewRefreshToken();
    }
}
