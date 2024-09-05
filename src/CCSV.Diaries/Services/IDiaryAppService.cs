using CCSV.Diaries.Dtos.Diaries;
using CCSV.Diaries.Dtos.Entries;

namespace CCSV.Diaries.Services;

public interface IDiaryAppService
{
    Task<IEnumerable<DiaryQueryDto>> GetAll(DiaryFilterDto filter);
    Task<DiaryReadDto> GetById(Guid id);
    Task<int> GetLength();
    Task Create(DiaryCreateDto data);
    Task Update(Guid id, DiaryUpdateDto data);
    Task<IEnumerable<EntryReadDto>> GetAllEntries(Guid diaryId, EntryFilterDto filter);
    Task AddEntry(Guid diaryId, EntryCreateDto data);
    Task RemoveEntry(Guid diaryId, Guid entryId);
    Task Delete(Guid id);
    Task Enable(Guid id);
    Task Disable(Guid id);

}
