using AutoMapper;
using CCSV.Diaries.Dtos.Entries;
using CCSV.Diaries.Models;

namespace CCSV.Diaries.Services.Mappings;

public class EntryMappingProfile: Profile
{
    public EntryMappingProfile()
    {
        CreateMap<Entry, EntryReadDto>();
    }
}
