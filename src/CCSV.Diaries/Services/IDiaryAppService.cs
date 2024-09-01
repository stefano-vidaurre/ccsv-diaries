using CCSV.Diaries.Dtos.Diaries;
using CCSV.Diaries.Dtos.Entries;

namespace CCSV.Diaries.Services;

public interface IDiaryAppService
{
    Task<int> GetLength();
    Task<IEnumerable<DiaryQueryDto>> GetAll(DiaryFilterDto filter);
    Task<DiaryReadDto> GetById(Guid id);
    Task Create(DiaryCreateDto data);
    Task Update(Guid id, DiaryUpdateDto updateDto);
    Task Delete(Guid id);
    Task Enable(Guid id);
    Task Disable(Guid id);

    Task<IEnumerable<EntryReadDto>> GetAllEntries(Guid diaryId, EntryFilterDto filter);
    Task AddEntry(Guid diaryId, EntryCreateDto data);
    Task UpdateEntry(Guid diaryId, Guid entryId, EntryUpdateDto entryEditDto);
    Task RemoveEntry(Guid diaryId, Guid entryId);
}
