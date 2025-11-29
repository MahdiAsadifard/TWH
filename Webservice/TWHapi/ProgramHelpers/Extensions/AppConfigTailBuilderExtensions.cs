using TWHapi.ProgramHelpers.Middlewares;

namespace TWHapi.ProgramHelpers.Extensions
{
    public static class AppConfigTailBuilderExtensions
    {
        public static WebApplication ConfigureTail(this WebApplication app)
        {
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
