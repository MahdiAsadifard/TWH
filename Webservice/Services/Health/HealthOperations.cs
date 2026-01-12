using Database.Redis;
using Microsoft.Extensions.Options;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Health
{
    
    public class HealthOperations : IHealthOperations
    {
        private readonly IRedisServices _redisServices;
        private readonly IOptions<RedisOptions> _redisOptions;

        public HealthOperations(
            IRedisServices redisServices,
            IOptions<RedisOptions> redisOptions
            )
        {
            this._redisServices = redisServices;
            this._redisOptions = redisOptions;
        }

        public async Task<string> AddSampleRedisCache()
        {
            var now = DateTime.Now;
            var key = "Key_" + now;
            var value = $"value from api; {now}";
            var isKeyAdded = await this._redisServices.SetAddAsync(key, value, _redisOptions.Value.KeyTtlMinutes);

            var result = await this._redisServices.GetValueAsync(key);
            var isDeleted  = await this._redisServices.DeleteKeyAsync(key);
            return string.Join(", ", result)+ "\nKey deleted: " + isDeleted;
        }
    }
}
