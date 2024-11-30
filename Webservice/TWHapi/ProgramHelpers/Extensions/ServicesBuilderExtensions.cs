using Database.Model;
using Microsoft.Extensions.Configuration;
using Models.Common;
using Services.Authentication;
using Services.Collections;
using Services.Interfaces;

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
            services.AddSingleton(typeof(Database.IDatabase<>), typeof(Database.Database<>));
            services.AddScoped<IUserOperations, UserOperations>();
            services.AddScoped<IAuthOperations, AuthOperations>();
            services.AddScoped<IJWTHelper, JWTHelper>();

            return services;
        }
    }
}
