using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Database.Redis
{
    public class RedisServices : IRedisServices
    {
        private readonly IRedisCommandsBuilder _redisCommandsBuilder;
        public RedisServices(
                IRedisCommandsBuilder redisCommandsBuilder
            )
        {
            this._redisCommandsBuilder = redisCommandsBuilder;
        }

        public async Task<bool> SetJson(string key, object value, string path = "$")
        {
            var json = await this._redisCommandsBuilder.GetJsonCommands();
            var result = await json.SetAsync(key, path, value);
            return result;
        }
        public async Task<T> GetJson<T>(string key, string path = "$")
        {
            var json = await this._redisCommandsBuilder.GetJsonCommands();
            var result = await json.GetAsync<T>(key, path);
            return result;
        }

        public async Task<bool> SetBloom(string key, string value)
        {
            var redisKey = new RedisKey(key);
            var rediValue = new RedisValue(value);
            var bloom = await this._redisCommandsBuilder.GetBloomCommands();
            bool result = bloom.Add(redisKey, rediValue);
            return result;
        }
    }
}
