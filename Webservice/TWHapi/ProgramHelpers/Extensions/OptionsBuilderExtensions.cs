using Core.Queue;

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

            return services;
        }
    }
}
