using Core.Queue;
using Models.Options;

namespace TWHapi.ProgramHelpers.Extensions
{
    public static class OptionsBuilderExtensions
    {
        public static IServiceCollection InitializeOptions(this IServiceCollection services, IConfiguration configuration)
        {
            /// This part will change the format to Pascal-Case
            /// We are going to use Camel-Case as default behavior
            //services.AddControllers().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.PropertyNamingPolicy = null;
            //});

            services.Configure<BackgroundTaskQueueOptions>(configuration.GetSection(BackgroundTaskQueueOptions.OptionName));

            services.Configure<ServiceInfoOptions>(configuration.GetSection(ServiceInfoOptions.OptionName));
            services.Configure<DatabseOptions>(configuration.GetSection(DatabseOptions.OptionName));

            return services;
        }
    }
}
