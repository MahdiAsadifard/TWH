using NLog;

namespace Core.NLogs
{
    public class NLogHelpers<T>
    {
        private static Logger logger = LogManager.GetLogger(typeof(T).Name);

        public static Logger Logger { get => logger; }

    }
}
