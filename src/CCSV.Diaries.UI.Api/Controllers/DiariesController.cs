using CCSV.Diaries.Dtos.Diaries;
using CCSV.Diaries.Dtos.Entries;
using CCSV.Diaries.Services;
using Microsoft.AspNetCore.Mvc;

namespace CCSV.Diaries.UI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiariesController : ControllerBase
{
    private readonly IDiaryAppService _diaryAppService;

    public DiariesController(IDiaryAppService diaryAppService)
    {
        _diaryAppService = diaryAppService;
    }

    [HttpGet]
    public Task<IEnumerable<DiaryQueryDto>> GetAll() {
        return _diaryAppService.GetAll();
    }

    [HttpGet("{id}")]
    public Task<DiaryReadDto> GetById(Guid id) {
        return _diaryAppService.GetById(id);
    }

    [HttpPost]
    public Task Create(DiaryCreateDto data) {
        return _diaryAppService.Create(data);
    }

    [HttpDelete("{id}")]
    public Task Delete(Guid id) {
        return _diaryAppService.Delete(id);
    }

    [HttpPost("{diaryId}/entries")]
    public Task AddEntry(Guid diaryId, EntryCreateDto data){
        return _diaryAppService.AddEntry(diaryId, data);
    }

    [HttpDelete("{diaryId}/entries/{entryId}")]
    public Task RemoveEntry(Guid diaryId, Guid entryId) {
        return _diaryAppService.RemoveEntry(diaryId, entryId);
    }
}