
namespace Database.Redis
{
    public interface IRedisServices
    {
        Task<T> GetJson<T>(string key, string path = "$");
        Task<bool> SetJson(string key, object value, string path = "$");

        Task<bool> SetBloom(string key, string value);
    }
}