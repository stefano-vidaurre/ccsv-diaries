using CCSV.Diaries.Dtos.Diaries;
using CCSV.Diaries.Dtos.Entries;

namespace CCSV.Diaries.Services;

public interface IDiaryAppService
{
    Task<IEnumerable<DiaryQueryDto>> GetAll();
    Task<DiaryReadDto> GetById(Guid id);
    Task Create(DiaryCreateDto data);
    Task AddEntry(Guid diaryId, EntryCreateDto data);
    Task RemoveEntry(Guid diaryId, Guid entryId);
    Task Delete(Guid id);
}
