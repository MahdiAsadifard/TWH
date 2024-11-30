using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;
using Models.DTOs.User;
using Core.Response;
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
    }
}
