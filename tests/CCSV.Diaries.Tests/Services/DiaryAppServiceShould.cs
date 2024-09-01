using AutoMapper;
using CCSV.Data.EFCore;
using CCSV.Diaries.Contexts;
using CCSV.Diaries.Dtos.Diaries;
using CCSV.Diaries.Dtos.Entries;
using CCSV.Diaries.Repositories;
using CCSV.Diaries.Services;
using CCSV.Diaries.Services.Mappings;
using CCSV.Domain.Exceptions;
using FluentAssertions;

namespace CCSV.Diaries.Tests.Services;

public class DiaryAppServiceShould : IDisposable
{
    private readonly ApplicationContext _applicationContext;
    private readonly IDiaryRepository _diaryRepository;
    private readonly IEntryRepository _entryRepository;
    private readonly IMapper _mapper;

    private readonly DiaryAppService _diaryAppService;

    public DiaryAppServiceShould()
    {
        _applicationContext = InMemoryApplicationContext.Create();
        _diaryRepository = new DiaryRepository(_applicationContext);
        _entryRepository = new EntryRepository(_applicationContext);
        _mapper = AutoMapperFactory.Create();
        _diaryAppService = new DiaryAppService(_diaryRepository, _entryRepository, _mapper);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _applicationContext.Dispose();
        }
    }


    [Fact]
    public async Task GetEmptyList()
    {
        IEnumerable<DiaryQueryDto> result = await _diaryAppService.GetAll(new DiaryFilterDto());

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateADiary()
    {
        DiaryCreateDto createDto = new DiaryCreateDto() { Id = Guid.NewGuid() };

        await _diaryAppService.Create(createDto);
        await _applicationContext.SaveChangesAsync();

        DiaryReadDto result = await _diaryAppService.GetById(createDto.Id);
        result.Id.Should().Be(createDto.Id);
    }

    [Fact]
    public async Task UpdateADiary()
    {
        DiaryCreateDto createDto = new DiaryCreateDto() { Id = Guid.NewGuid() };
        await _diaryAppService.Create(createDto);
        await _applicationContext.SaveChangesAsync();
        DiaryUpdateDto updateDto = new DiaryUpdateDto() { ExpirationDate = DateTime.UtcNow.ToString("O") };

        await _diaryAppService.Update(createDto.Id, updateDto);

        DiaryReadDto result = await _diaryAppService.GetById(createDto.Id);
        result.ExpirationDate.Should().Be(updateDto.ExpirationDate);
    }

    [Fact]
    public async Task AddEntry()
    {
        DiaryCreateDto diaryCreateDto = new DiaryCreateDto() { Id = Guid.NewGuid() };
        await _diaryAppService.Create(diaryCreateDto);
        await _applicationContext.SaveChangesAsync();
        EntryCreateDto entryCreateDto = new EntryCreateDto() {
            Id = Guid.NewGuid(),
            State = "Normal"
        };

        await _diaryAppService.AddEntry(diaryCreateDto.Id, entryCreateDto);
        await _applicationContext.SaveChangesAsync();

        DiaryReadDto result = await _diaryAppService.GetById(diaryCreateDto.Id);
        result.Entries.Should().Contain(entry => entry.Id == entryCreateDto.Id);
    }

    [Fact]
    public async Task UpdateEntryInDiary()
    {
        Guid diaryId = Guid.NewGuid();
        DiaryCreateDto diaryCreateDto = new DiaryCreateDto() { Id = diaryId };
        await _diaryAppService.Create(diaryCreateDto);
        await _applicationContext.SaveChangesAsync();

        Guid entryId = Guid.NewGuid();
        EntryCreateDto entryCreateDto = new EntryCreateDto() { Id = entryId, State = "Normal" };
        await _diaryAppService.AddEntry(diaryCreateDto.Id, entryCreateDto);
        await _applicationContext.SaveChangesAsync();

        EntryUpdateDto entryUpdateDto = new EntryUpdateDto() { Description = "Updated Description" };

        await _diaryAppService.UpdateEntry(diaryCreateDto.Id, entryCreateDto.Id, entryUpdateDto);
        await _applicationContext.SaveChangesAsync();

        DiaryReadDto result = await _diaryAppService.GetById(diaryCreateDto.Id);
        EntryReadDto? updatedEntry = result.Entries?.FirstOrDefault(e => e.Id == entryCreateDto.Id);
        updatedEntry.Should().NotBeNull();
        updatedEntry?.Description.Should().Be("Updated Description");
    }

    [Fact]
    public async Task FailWhenUpdateEntryWithNullDescription()
    {
        Guid diaryId = Guid.NewGuid();
        DiaryCreateDto diaryCreateDto = new DiaryCreateDto() { Id = diaryId };
        await _diaryAppService.Create(diaryCreateDto);
        await _applicationContext.SaveChangesAsync();

        Guid entryId = Guid.NewGuid();
        EntryCreateDto entryCreateDto = new EntryCreateDto() { Id = entryId, State = "Normal" };
        await _diaryAppService.AddEntry(diaryCreateDto.Id, entryCreateDto);
        await _applicationContext.SaveChangesAsync();

        EntryUpdateDto entryUpdateDto = new EntryUpdateDto() { Description = null };
        Func<Task> act = async () => await _diaryAppService.UpdateEntry(diaryCreateDto.Id, entryCreateDto.Id, entryUpdateDto);;

        await act.Should().ThrowAsync<InvalidValueException>();
    }

    [Fact]
    public async Task RemoveEntry()
    {
        DiaryCreateDto diaryCreateDto = new DiaryCreateDto() { Id = Guid.NewGuid() };
        await _diaryAppService.Create(diaryCreateDto);
        await _applicationContext.SaveChangesAsync();
        EntryCreateDto entryCreateDto = new EntryCreateDto() {
            Id = Guid.NewGuid(),
            State = "Normal"
        };
        await _diaryAppService.AddEntry(diaryCreateDto.Id, entryCreateDto);
        await _applicationContext.SaveChangesAsync();

        await _diaryAppService.RemoveEntry(diaryCreateDto.Id, entryCreateDto.Id);
        await _applicationContext.SaveChangesAsync();

        DiaryReadDto result = await _diaryAppService.GetById(diaryCreateDto.Id);
        result.Entries.Should().NotContain(entry => entry.Id == entryCreateDto.Id);
    }

    [Fact]
    public async Task DeleteADiary()
    {
        DiaryCreateDto diaryCreateDto = new DiaryCreateDto() { Id = Guid.NewGuid() };
        await _diaryAppService.Create(diaryCreateDto);
        await _applicationContext.SaveChangesAsync();

        await _diaryAppService.Delete(diaryCreateDto.Id);
        await _applicationContext.SaveChangesAsync();

        Func<Task<DiaryReadDto>> result = async () => await _diaryAppService.GetById(diaryCreateDto.Id);
        await result.Should().ThrowAsync<ValueNotFoundException>();
    }

    [Fact]
    public async Task DisableADiary()
    {
        DiaryCreateDto diaryCreateDto = new DiaryCreateDto() { Id = Guid.NewGuid() };
        await _diaryAppService.Create(diaryCreateDto);
        await _applicationContext.SaveChangesAsync();

        await _diaryAppService.Disable(diaryCreateDto.Id);
        await _applicationContext.SaveChangesAsync();

        DiaryReadDto result = await _diaryAppService.GetById(diaryCreateDto.Id);
        result.IsDisabled.Should().BeTrue();
    }

    [Fact]
    public async Task EnableADiary()
    {
        DiaryCreateDto diaryCreateDto = new DiaryCreateDto() { Id = Guid.NewGuid() };
        await _diaryAppService.Create(diaryCreateDto);
        await _applicationContext.SaveChangesAsync();
        await _diaryAppService.Disable(diaryCreateDto.Id);
        await _applicationContext.SaveChangesAsync();

        await _diaryAppService.Enable(diaryCreateDto.Id);
        await _applicationContext.SaveChangesAsync();

        DiaryReadDto result = await _diaryAppService.GetById(diaryCreateDto.Id);
        result.IsDisabled.Should().BeFalse();
    }
}