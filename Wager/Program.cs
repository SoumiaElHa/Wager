using Wager;
using Wager.Services;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);

var app = builder.Build();

ConfigureApplication(app);

app.Run();

static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddScoped<IExecutionContext, HttpRequestContext>();
    builder.Services.AddScoped<IExecutionContextInitializer, ExecutionContextInitializer>();
    builder.Services.AddTransient<IWagerService, WagerService>();
    builder.Services.AddMemoryCache();
}

static void ConfigureApplication(WebApplication webApplication)
{
    webApplication.UseSwagger();
    webApplication.UseSwaggerUI();
    webApplication.UseHttpsRedirection();
    webApplication.UseAuthorization();
    webApplication.MapControllers();
    webApplication.UseMiddleware<ExecutionContextMiddleware>();
}