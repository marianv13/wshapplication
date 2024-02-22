using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using WSHAppAPI.Controllers;
using WSHAppDB.Model;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddDbContext<WSHDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddControllers();

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "AngularDebug",
            builder => builder.AllowAnyHeader().AllowAnyMethod()
                .WithOrigins("https://localhost:4200")
                .WithOrigins("http://localhost:4200"));
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    await WSHDBContext.MigrAsync(app.Services);

    if (args.Length > 0 && args[0] == "loadData")
    {
        await Migr.LoadDataAsync(app.Services);
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    //app.UseHttpsRedirection();
    //app.UseDefaultFiles();
    app.UseStaticFiles();
    //app.UseForwardedHeaders();
    app.UseCors("AngularDebug");

    app.UseRouting();

    //app.MapFallbackToController("Index", "Fallback");

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}