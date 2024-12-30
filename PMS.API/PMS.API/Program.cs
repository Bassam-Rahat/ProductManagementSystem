using PMS.API.Infrastructure.Extensions;
using Serilog;

try
{
    Log.Information("Host started");
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .ConfigureOptions(builder.Configuration)
        .ConfigureServices(builder.Configuration)
        .ConfigureRepositories()
        .ConfigureMediatR()
        .ConfigureSwagger()
        .ConfigureAutoMapper()
        .ConfigureDbContexts(builder.Configuration);

    builder.Host.UseSerilog((context, services, configuration) => configuration
         .ReadFrom.Configuration(context.Configuration)
         .ReadFrom.Services(services));

    var sentryEnabled = builder.Configuration.GetValue<bool>("SentryEnabled");
    if (sentryEnabled)
    {
        builder.WebHost.UseSentry();
    }

    builder.Services.AddControllers();

    var app = builder.Build();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCustomErrorHandlingMiddleware();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapProductEndpoints();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}