using Models.Options;

namespace TWHapi.ProgramHelpers.Extensions
{
    public static class HostBuilderExtensions
    {
        public static WebApplication CreateHostBuilder(
            this WebApplication app,
            IWebHostEnvironment env,
            IConfiguration configuration)
        {
            bool IsDevelopment = env.IsDevelopment();
            var port = configuration.GetSection($"{ServiceInfoOptions.OptionName}:{nameof(ServiceInfoOptions.Port)}").Get<string>();

            Console.WriteLine($"Development: {IsDevelopment}, PORT: {port}");

            app.Urls.Add($"http://*:{port}");
            return app;
        }
    }
}
