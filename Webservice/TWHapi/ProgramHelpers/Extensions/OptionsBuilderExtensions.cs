namespace TWHapi.ProgramHelpers.Extensions
{
    public static class OptionsBuilderExtensions
    {
        public static IServiceCollection InitializeOptions(this IServiceCollection services)
        {
            /// This part will change the format to Pascal-Case
            /// We are going to use Camel-Case as default behavior
            //services.AddControllers().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.PropertyNamingPolicy = null;
            //});
            return services;
        }
    }
}
