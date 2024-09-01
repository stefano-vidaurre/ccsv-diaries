using CCSV.Diaries.Dtos.Diaries;
using CCSV.Diaries.Dtos.Entries;
using CCSV.Diaries.Services;
using Microsoft.AspNetCore.Mvc;

namespace CCSV.Diaries.UI.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Tags("Diaries")]
public class DiariesController : ControllerBase
{
    private readonly IDiaryAppService _diaryAppService;

    public DiariesController(IDiaryAppService diaryAppService)
    {
        _diaryAppService = diaryAppService;
    }

    [HttpGet]
    public Task<IEnumerable<DiaryQueryDto>> GetAll([FromQuery] DiaryFilterDto filter) {
        return _diaryAppService.GetAll(filter);
    }

    [HttpGet("{id}")]
    public Task<DiaryReadDto> GetById(Guid id) {
        return _diaryAppService.GetById(id);
    }

    [HttpGet("Length")]
    public Task<int> GetLength() {
        return _diaryAppService.GetLength();
    }

    [HttpPost]
    public Task Create([FromBody] DiaryCreateDto data) {
        return _diaryAppService.Create(data);
    }

    [HttpPut("{id}")]
    public Task Update(Guid id, [FromBody] DiaryUpdateDto data) {
        return _diaryAppService.Update(id, data);
    }

    [HttpDelete("{id}")]
    public Task Delete(Guid id) {
        return _diaryAppService.Delete(id);
    }

    [HttpGet("{diaryId}/Entries")]
    public Task<IEnumerable<EntryReadDto>> GetAllEntries(Guid diaryId, [FromQuery] EntryFilterDto filter){
        return _diaryAppService.GetAllEntries(diaryId, filter);
    }

    [HttpPost("{diaryId}/Entries")]
    public Task AddEntry(Guid diaryId, [FromBody] EntryCreateDto data){
        return _diaryAppService.AddEntry(diaryId, data);
    }

    [HttpDelete("{diaryId}/Entries/{entryId}")]
    public Task RemoveEntry(Guid diaryId, Guid entryId) {
        return _diaryAppService.RemoveEntry(diaryId, entryId);
    }

    [HttpPut("{id}/Enabled")]
    public Task Enable(Guid id) {
        return _diaryAppService.Enable(id);
    }

    [HttpPut("{id}/Disabled")]
    public Task Disable(Guid id) {
        return _diaryAppService.Disable(id);
    }
}
