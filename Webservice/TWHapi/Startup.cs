using Database.Model;
using Models.Common;
using TWHapi.Middlewares;

namespace TWHapi
{
    public class Startup
    {
        private readonly WebApplicationBuilder _builder;
        public Startup(WebApplicationBuilder builder)
        {
            _builder = builder;
        }

        public void Configure(WebApplication app)
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

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        // Add services to the container.
        //public void ConfigureServices(IServiceCollection services)
        public void ConfigureServices()
        {
            _builder.Services.AddControllers();
            _builder.Services.AddEndpointsApiExplorer();
            _builder.Services.AddSwaggerGen();

            _builder.Services.Configure<DatabaseSettings>(_builder.Configuration.GetSection("ServerInfo"));

            // Mapper: All DTOs listed in Models assebly
            _builder.Services.AddAutoMapper(Utility.GetModelsAssemblies());

            this.InitializeCORS();
            this.InitializeOptions();
            this.InitializeServices();
            this.CreateHostBuilder();
        }


        private void InitializeCORS()
        {
            _builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                        .WithOrigins(_builder.Configuration.GetSection("CORS:AllowedOrigins").Get<string[]>())
                        .WithHeaders(_builder.Configuration.GetSection("CORS:AllowedHeaders").Get<string[]>())
                        .WithMethods(_builder.Configuration.GetSection("CORS:AllowedMethods").Get<string[]>());
                    });
            });
        }
        private void InitializeOptions()
        {
            _builder.Logging.ClearProviders();
            _builder.Logging.AddConsole();

            //services.AddHttpsRedirection(options => options.HttpsPort = 5001);
            _builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
        }
        //private void InitializeServices(IServiceCollection services)
        private void InitializeServices()
        {
            _builder.Services.AddSingleton(typeof(Database.IDatabase<>), typeof(Database.Database<>));
            _builder.Services.AddScoped<Services.Interfaces.IUserOperations, Services.Collections.UserOperations>();
        }

        private void CreateHostBuilder(/*IWebHostBuilder webHost*/)
        {
            var envs = Environment.GetEnvironmentVariables();
            foreach (var item in envs)
            {
                Console.WriteLine("==== env:  " + item);
            }
            var x = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Console.WriteLine("==========================x ASPNETCORE_ENVIRONMENT : " + x);
            // TODO : change url based on dev / prod
            _builder.WebHost.UseUrls("http://*:5001");
        }
    }
}
