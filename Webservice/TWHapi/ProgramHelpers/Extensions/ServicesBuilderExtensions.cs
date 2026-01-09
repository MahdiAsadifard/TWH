using Core.ILogs;
using Core.Queue;
using Core.Token;
using Database.Mongodb;
using Database.Redis;
using Microsoft.OpenApi;
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
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
            });

            // -------------- singleton services ----------------
            // Logger
            services.AddSingleton(typeof(ILoggerHelpers<>), typeof(LoggerHelpers<>));

            // Redis
            services.AddSingleton<IRedisProvider, RedisProvider>();
            services.AddSingleton<IRedisCommandsBuilder, RedisCommandsBuilder>();
            services.AddSingleton<IRedisServices, RedisServices>();

            // Queue
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            // Processing Services
            services.AddSingleton<IServiceProcessing, ServiceProcessing>();

            // Service Processing
            services.AddSingleton<IServiceProcessing, ServiceProcessing>();

            // Mapper: All DTOs listed in Models assebly
            services.AddAutoMapper(cfg => { }, Utility.GetModelsAssemblies());

            // -------------- scoped services ----------------

            // JWT
            services.AddScoped<IJWTHelper, JWTHelper>();

            // Database
            services.AddScoped(typeof(IDatabase<>), typeof(Database<>));

            // Operations services
            services.AddScoped<IAuthOperations, AuthOperations>();
            services.AddScoped<IUserOperations, UserOperations>();

            return services;
        }
    }
}
