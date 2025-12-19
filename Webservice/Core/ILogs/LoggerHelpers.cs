using Microsoft.Extensions.Logging;
using MSLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Core.ILogs
{
    public class LoggerHelpers<T> : ILoggerHelpers<T>
    {
        private readonly ILogger<T> _logger;

        public LoggerHelpers()
        {
            _logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            }).CreateLogger<T>();
        }

        #region properties
        
        public ILogger<T> Logger => _logger;

        #endregion

        #region private methods

        private MSLogLevel MapLogLevel(ILogs.LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => MSLogLevel.Trace,
                LogLevel.Debug => MSLogLevel.Debug,
                LogLevel.Information => MSLogLevel.Information,
                LogLevel.Warning => MSLogLevel.Warning,
                LogLevel.Error => MSLogLevel.Error,
                LogLevel.Critical => MSLogLevel.Critical,
                _ => MSLogLevel.None,
            };
        }

        #endregion

        #region public methods

        public void Log(LogLevel logLevel, string message, params object?[] args)
        {
            var msLogLevel = MapLogLevel(logLevel);
            if (_logger.IsEnabled(msLogLevel)) _logger.Log(logLevel: msLogLevel, $"{message}", args);
        }
        public void Log(Exception exception, string message, params object?[] args)
        {
            if (_logger.IsEnabled(MSLogLevel.Error)) _logger.Log(MSLogLevel.Error, exception, $"{message}", args);
        }

        #endregion
    }
}
