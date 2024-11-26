using Database.Model;
using Microsoft.Extensions.Configuration;
using Models.Common;

namespace TWHapi.ProgramHelpers.Extensions
{
    public static class ServicesBuilderExtensions
    {
        public static IServiceCollection InitializeServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.Configure<DatabaseSettings>(configuration.GetSection("ServerInfo"));

            // Mapper: All DTOs listed in Models assebly
            services.AddAutoMapper(Utility.GetModelsAssemblies());
            services.AddSingleton(typeof(Database.IDatabase<>), typeof(Database.Database<>));
            services.AddScoped<Services.Interfaces.IUserOperations, Services.Collections.UserOperations>();

            return services;
        }
    }
}
