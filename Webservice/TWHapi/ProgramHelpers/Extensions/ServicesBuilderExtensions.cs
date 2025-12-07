using Core.Queue;
using Models.Common;
using Models.Options;
using Services.Authentication;
using Services.Collections;
using Services.Interfaces;
using Services.ServiceProcessing;

namespace TWHapi.ProgramHelpers.Extensions
{
    public static class ServicesBuilderExtensions
    {
        public static IServiceCollection InitializeServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });
            });

            services.Configure<DatabseOptions>(configuration.GetSection("ServerInfo"));

            services.InitializeJWT(configuration);
            // JWT
            services.AddScoped<IJWTHelper, JWTHelper>();

            // Queue
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();


            // Processing Services
            services.AddSingleton<IServiceProcessing, ServiceProcessing>();

            // Service Processing
            services.AddSingleton<IServiceProcessing, ServiceProcessing>();

            // Mapper: All DTOs listed in Models assebly
            services.AddAutoMapper(Utility.GetModelsAssemblies());

            // Database
            services.AddSingleton(typeof(Database.IDatabase<>), typeof(Database.Database<>));
            services.AddScoped<IAuthOperations, AuthOperations>();
            services.AddScoped<IUserOperations, UserOperations>();

            return services;
        }
    }
}
