var builder = WebApplication.CreateBuilder(args);

var startUp = new TWHapi.Startup(builder);
startUp.ConfigureServices();

var app = builder.Build();
startUp.Configure(app);
