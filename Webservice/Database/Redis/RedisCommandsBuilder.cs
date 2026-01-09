using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Redis
{
    public class RedisCommandsBuilder : IRedisCommandsBuilder
    {
        private readonly StackExchange.Redis.IDatabase _redis;

        public RedisCommandsBuilder(IRedisProvider redisFactory)
        {
            this._redis = redisFactory.Build();
        }

        public async Task<bool> SetAddAsync(string key, string value)
        {
            var redisKey = new RedisKey(key);
            if (await _redis.KeyExistsAsync(redisKey)) return true;

            var redisValue = new RedisValue(value);
            return await _redis.SetAddAsync(redisKey, redisValue);
        }

        public async Task<string> GetValue(string key)
        {
            var redisKey = new RedisKey(key);

            var value = await _redis.StringGetAsync(redisKey);
            return value.ToString();
        }

        public async Task<BloomCommands> GetBloomCommands()
        {
            return _redis.BF();
        }
        public async Task<CuckooCommands> GetCuckooCommands()
        {
            return _redis.CF();
        }
        public async Task<CmsCommands> GetCmsCommands()
        {
            return _redis.CMS();
        }
        public async Task<TopKCommands> GetTopKCommands()
        {
            return _redis.TOPK();
        }
        public async Task<TdigestCommands> GetTdigestCommands()
        {
            return _redis.TDIGEST();
        }
        public async Task<SearchCommands> GetSearchCommands()
        {
            return _redis.FT();
        }
        public async Task<JsonCommands> GetJsonCommands()
        {
            return _redis.JSON();
        }
        public async Task<TimeSeriesCommands> GetTimeSeriesCommands()
        {
            return _redis.TS();
        }
    }
}
