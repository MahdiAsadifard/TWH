using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Core.ILogs;
using Core.NLogs;
using Microsoft.Extensions.Options;
using NRedisStack;
using StackExchange.Redis;

namespace Database.Redis
{
    public class RedisProvider : IRedisProvider
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IOptions<RedisOptions> _options;
        private readonly ILoggerHelpers<RedisProtocol> _logger;

        public RedisProvider(
            IOptions<RedisOptions> options,
            ILoggerHelpers<RedisProtocol> logger
            )
        {
            this._options = options;
            this._logger = logger;
            _connectionMultiplexer = this.Connect();
        }

        #region Private

        private ConnectionMultiplexer Connect()
        {
            try
            {
                var st =  ConnectionMultiplexer.Connect($"{this._options.Value.Host}:{this._options.Value.Port}");
                return st;
            }
            catch (Exception ex)
            {
                this._logger.Log(ex, "Error on connecting to Redis - \nMessage: {Message}, \nStackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                NLogHelpers<RedisProvider>.Logger.Error(ex, "Error on connecting to Redis {Message}", ex.Message);
                throw;
            }
        }

        #endregion

        public StackExchange.Redis.IDatabase Build()
        {
            return _connectionMultiplexer.GetDatabase();
        }
    }
}
