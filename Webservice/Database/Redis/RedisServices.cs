using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Database.Redis
{
    public class RedisServices : IRedisServices
    {
        private readonly StackExchange.Redis.IDatabase _redis;
        private readonly IOptions<RedisOptions> _options;
        private readonly int TTLMinutes;

        public RedisServices(
                IRedisProvider redisFactory,
                IOptions<RedisOptions> options
            )
        {
            this._options = options;
            this.TTLMinutes = this._options.Value.KeyTtlMinutes;
            this._redis = redisFactory.Build();
        }

        public async Task<bool> SetAddAsync(string key, string value, int ttlMinutes)
        {
            var redisKey = new RedisKey(key);
            if (await _redis.KeyExistsAsync(redisKey)) return true;

            var redisValue = new RedisValue(value);
            await _redis.SetAddAsync(redisKey, redisValue);
            return await _redis.KeyExpireAsync(redisKey, TimeSpan.FromMinutes(ttlMinutes), when: ExpireWhen.Always, flags: CommandFlags.None);
        }

        public async Task<RedisValue[]> GetValueAsync(string key)
        {
            var redisKey = new RedisKey(key);

            return await _redis.SetMembersAsync(redisKey, CommandFlags.None);
        }

        public async Task<bool> DeleteKeyAsync(string key)
        {
            var redisKey = new RedisKey(key);
            return await _redis.KeyDeleteAsync(redisKey);
        }
    }
}
