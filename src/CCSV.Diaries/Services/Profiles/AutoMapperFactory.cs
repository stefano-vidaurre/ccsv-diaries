using AutoMapper;

namespace CCSV.Diaries.Services.Profiles;

public static class AutoMapperFactory
{
    public static IMapper Create()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(AutoMapperFactory).Assembly);
            cfg.CreateMap<DateTime, string>().ConvertUsing(dt => dt.ToString("O"));
        });
        return configuration.CreateMapper();
    }
}
