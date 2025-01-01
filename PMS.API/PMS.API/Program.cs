using PMS.API.Infrastructure.Extensions;
using Serilog;

try
{
    Log.Information("Host started");
    var builder = WebApplication.CreateBuilder(args);

    const string AllowSpecificOrigins = "_allowSpecificOrigins";
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: AllowSpecificOrigins,
            policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
    });

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
    app.UseCors(AllowSpecificOrigins);
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