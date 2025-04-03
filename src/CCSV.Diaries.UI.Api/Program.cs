using AutoMapper;
using CCSV.Rest;
using CCSV.Data.EFCore;
using CCSV.Diaries.Contexts;
using CCSV.Diaries.Repositories;
using CCSV.Diaries.Services;
using CCSV.Diaries.Services.Profiles;
using Serilog;
using CCSV.Diaries.Services.Validators;
using CCSV.Domain.Validators;

namespace CCSV.Diaries.UI.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();
        Log.Information("Starting server...");
        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Services.ConfigureServices(builder.Configuration);
            builder.Host.ConfigureLogger();

            var app = builder.Build();
            app.ConfigureApp();
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An unhandled exception occurred during bootstrapping.");
        }
        finally
        {
            Log.CloseAndFlush();
        }

    }

    private static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddSwaggerGen();
        services.AddSingleton<IMapper>(_ => AutoMapperFactory.Create());
        services.AddSingleton<IMasterValidator>(_ => ValidatorFactory.Create());
        services.AddDbContext<ApplicationContext, InFileApplicationContext>();
        services.AddEntityFramework();

        services.AddScoped<IDiaryRepository, DiaryRepository>();
        services.AddScoped<IEntryRepository, EntryRepository>();
        services.AddScoped<IDiaryAppService, DiaryAppService>();
        services.ConfigureCors(configuration);

        return services;
    }
    private static IHostBuilder ConfigureLogger(this IHostBuilder host)
    {
        host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

        return host;
    }

    private static WebApplication ConfigureApp(this WebApplication app)
    {
        app.UseCors();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseSerilogRequestLogging();

        app.UseHttpStatusCodeExceptionHandler();

        app.UseUnitOfWork();

        app.UseRouting();

        app.MapControllers();

        return app;
    }
}
