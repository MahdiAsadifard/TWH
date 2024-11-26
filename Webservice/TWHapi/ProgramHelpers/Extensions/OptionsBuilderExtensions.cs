namespace TWHapi.ProgramHelpers.Extensions
{
    public static class OptionsBuilderExtensions
    {
        public static IServiceCollection InitializeOptions(this IServiceCollection services)
        {

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            return services;
        }
    }
}
