namespace TWHapi.ProgramHelpers.Extensions
{
    public static class CORSBuilderExtensions
    {
        public static IServiceCollection InitializeCORS(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                        .WithOrigins(configuration.GetSection("CORS:AllowedOrigins").Get<string[]>() ?? [])
                        .WithHeaders(configuration.GetSection("CORS:AllowedHeaders").Get<string[]>() ?? [])
                        .WithMethods(configuration.GetSection("CORS:AllowedMethods").Get<string[]>() ?? []);
                    });
            });
            return services;
        }
    }
}
