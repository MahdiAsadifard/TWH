namespace TWHapi.ProgramHelpers.Extensions
{
    public static class HostBuilderExtensions
    {
        public static WebApplication CreateHostBuilder(this WebApplication app, IWebHostEnvironment env, IConfiguration configuration)
        {
            bool IsDevelopment = env.IsDevelopment();
            var port = IsDevelopment ?
                configuration.GetSection("ServerInfo:DevelopmentPort").Get<string>()
                : configuration.GetSection("ServerInfo:ProductionPort").Get<string>();

            Console.WriteLine($"Development: {IsDevelopment}, PORT: {port}");

            app.Urls.Add($"http://*:{port}");
            return app;
        }
    }
}
