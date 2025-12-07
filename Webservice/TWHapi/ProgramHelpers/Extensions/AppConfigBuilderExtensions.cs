namespace TWHapi.ProgramHelpers.Extensions
{
    public static class AppConfigBuilderExtensions
    {
        public static WebApplication Configure(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }
            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            return app;
        }
    }
}
