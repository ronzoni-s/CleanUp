using CleanUp.Application.WebApi.Extensions;
using CleanUp.Infrastructure.Extensions;
using CleanUp.WebApi.Extensions;
using fbognini.i18n;
using fbognini.Infrastructure.Multitenancy;
using fbognini.WebFramework;
using fbognini.WebFramework.Middlewares;
using fbognini.WebFramework.OpenApi;
using Microsoft.AspNetCore.Rewrite;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.AddConfigurations();
    builder.Host.UseSerilog((ctx, configuration) =>
    {
        configuration
            .WriteTo.Console()
            .ReadFrom.Configuration(ctx.Configuration);
    });

    builder.Services.AddInfrastructureWebApi(builder.Configuration);
    builder.Services.AddApplicationWebApi();

    var app = builder.Build();

    await app.InitializeI18N();

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseCors("AllowAll");

    app.UseRequestLocalizationI18N();

    app.UseRewriter(new RewriteOptions()
        .AddRedirectToLowercasePermanent());

    app.UseCustomApiExceptionHandler();

    app.UseMultiTenancy();
    app.UseTenantMiddleware();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseOpenApiDocumentation(builder.Configuration);

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

    app.InitializeInfrastructureDatabase(builder.Configuration);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}


