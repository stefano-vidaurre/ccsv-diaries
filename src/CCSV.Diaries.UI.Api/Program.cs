using AutoMapper;
using CCSV.Rest;
using CCSV.Data.EFCore;
using CCSV.Diaries.Contexts;
using CCSV.Diaries.Repositories;
using CCSV.Diaries.Services;
using CCSV.Diaries.Services.Mappings;

namespace CCSV.Diaries.UI.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.ConfigureServices();

        var app = builder.Build();
        app.ConfigureApp();
        app.Run();
    }

    private static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();
        services.AddSingleton<IMapper>(_ => AutoMapperFactory.Create());
        services.AddDbContext<ApplicationContext, InFileApplicationContext>();
        services.AddEntityFramework();

        services.AddScoped<IDiaryRepository, DiaryRepository>();
        services.AddScoped<IDiaryAppService, DiaryAppService>();

        return services;
    }

    private static WebApplication ConfigureApp(this WebApplication app)
    {
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

        app.UseHttpStatusCodeExceptionHandler();

        app.UseUnitOfWork();

        app.UseRouting();

        app.MapControllers();

        return app;
    }
}
