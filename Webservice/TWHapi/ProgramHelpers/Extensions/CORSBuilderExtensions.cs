using Models.Options;

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
                        .WithOrigins(configuration.GetSection($"{CorsOptions.OptionName}:{nameof(CorsOptions.AllowedOrigins)}").Get<string[]>() ?? [])
                        .WithHeaders(configuration.GetSection($"{CorsOptions.OptionName}:{nameof(CorsOptions.AllowedHeaders)}").Get<string[]>() ?? [])
                        .WithMethods(configuration.GetSection($"{CorsOptions.OptionName}:{nameof(CorsOptions.AllowedMethods)}").Get<string[]>() ?? []);
                    });
            });
            return services;
        }
    }
}
