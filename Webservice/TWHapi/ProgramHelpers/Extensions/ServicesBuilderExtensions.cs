using Core.Queue;
using Database.Model;
using Microsoft.Extensions.Configuration;
using Models.Common;
using Services.Authentication;
using Services.Collections;
using Services.Interfaces;
using System.ComponentModel;

namespace TWHapi.ProgramHelpers.Extensions
{
    public static class ServicesBuilderExtensions
    {
        public static IServiceCollection InitializeServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options => {
                options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });
            });

            services.Configure<DatabaseSettings>(configuration.GetSection("ServerInfo"));

            services.InitializeJWT(configuration);

            // Mapper: All DTOs listed in Models assebly
            services.AddAutoMapper(Utility.GetModelsAssemblies());
            // Database
            services.AddSingleton(typeof(Database.IDatabase<>), typeof(Database.Database<>));
            services.AddScoped<IUserOperations, UserOperations>();
            services.AddScoped<IAuthOperations, AuthOperations>();
            // JWT
            services.AddScoped<IJWTHelper, JWTHelper>();
            // Queue
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            // Background Services
            services.AddSingleton<BackgroundWorker>();

            return services;
        }
    }
}
