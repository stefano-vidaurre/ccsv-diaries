using AutoMapper;
using CCSV.Diaries.Dtos.Diaries;
using CCSV.Diaries.Models;

namespace CCSV.Diaries.Services.Profiles;

public class DiaryMappingProfile : Profile
{
    public DiaryMappingProfile()
    {
        CreateMap<Diary, DiaryReadDto>();
        CreateMap<Diary, DiaryQueryDto>();
    }
}
