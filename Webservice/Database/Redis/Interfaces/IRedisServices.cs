
using StackExchange.Redis;

namespace Database.Redis
{
    public interface IRedisServices
    {
        Task<bool> SetAddAsync(string key, string value, int ttlMinutes = default);
        
        Task<RedisValue[]> GetValueAsync(string key);
        
        Task<bool> DeleteKeyAsync(string key);
    }
}