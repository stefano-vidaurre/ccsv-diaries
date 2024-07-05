using CCSV.Diaries.Dtos.Diaries;
using CCSV.Diaries.Dtos.Entries;

namespace CCSV.Diaries.Services;

public interface IDiaryAppService {
    Task<IEnumerable<DiaryQueryDto>> GetAll();
    Task<DiaryReadDto> GetById(Guid id);
    Task Create(DiaryCreateDto data);
    Task AddEntry(Guid id, EntryCreateDto data);
    Task RemoveEntry(Guid id, Guid entryId);
    Task Delete(Guid id);
}
