using Microsoft.Extensions.Logging;

namespace Core.ILogs
{
    public interface ILoggerHelpers<T>
    {
        ILogger<T> Logger { get; }
        
        void Log(LogLevel logLevel, string message, params object?[] args);

        void Log(Exception excption, string message, params object?[] args);
    }
}