using TWHapi.ProgramHelpers.Middlewares;

namespace TWHapi.ProgramHelpers.Extensions
{
    public static class MiddlewareBuilderExtensions
    {
        public static WebApplication Middlewares(this WebApplication app)
        {
            app.UseMiddleware<JWTMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            return app;
        }
    }
}
