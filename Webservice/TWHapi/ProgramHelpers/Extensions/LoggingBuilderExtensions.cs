namespace TWHapi.ProgramHelpers.Extensions
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder InitializeLogging(this ILoggingBuilder logging)
        {
            logging.ClearProviders();
            logging.AddConsole();

            return logging;
        }
    }
}
