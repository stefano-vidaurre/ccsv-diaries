using AutoMapper;
using CCSV.Diaries.Dtos.Entries;
using CCSV.Diaries.Models;

namespace CCSV.Diaries.Services.Profiles;

public class EntryMappingProfile : Profile
{
    public EntryMappingProfile()
    {
        CreateMap<Entry, EntryReadDto>();
    }
}
