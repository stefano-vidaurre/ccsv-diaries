using AutoMapper;
using CCSV.Data.EFCore;
using CCSV.Diaries.Contexts;
using CCSV.Diaries.Dtos.Diaries;
using CCSV.Diaries.Dtos.Entries;
using CCSV.Diaries.Repositories;
using CCSV.Diaries.Services;
using CCSV.Diaries.Services.Profiles;
using CCSV.Diaries.Services.Validators;
using CCSV.Domain.Exceptions;
using CCSV.Domain.Repositories;
using CCSV.Domain.Validators;
using FluentAssertions;

namespace CCSV.Diaries.Tests.Services;

public class DiaryAppServiceShould : IDisposable
{
    private readonly ApplicationContext _applicationContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDiaryRepository _diaryRepository;
    private readonly IEntryRepository _entryRepository;
    private readonly IMasterValidator _masterValidator;
    private readonly IMapper _mapper;

    private readonly DiaryAppService _diaryAppService;

    public DiaryAppServiceShould()
    {
        _applicationContext = InMemoryApplicationContext.Create();
        _unitOfWork = new UnitOfWork(_applicationContext);
        _diaryRepository = new DiaryRepository(_applicationContext);
        _entryRepository = new EntryRepository(_applicationContext);
        _masterValidator = ValidatorFactory.Create();
        _mapper = AutoMapperFactory.Create();
        _diaryAppService = new DiaryAppService(_diaryRepository, _entryRepository, _masterValidator, _mapper);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if(disposing) {
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
        await _unitOfWork.SaveAsync();

        DiaryReadDto result = await _diaryAppService.GetById(createDto.Id);
        result.Id.Should().Be(createDto.Id);
    }

    [Fact]
    public async Task UpdateADiary()
    {
        DiaryCreateDto createDto = new DiaryCreateDto() { Id = Guid.NewGuid() };
        await _diaryAppService.Create(createDto);
        await _unitOfWork.SaveAsync();
        DiaryUpdateDto updateDto = new DiaryUpdateDto() { ExpirationDate = DateTime.UtcNow.ToString("O") };

        await _diaryAppService.Update(createDto.Id, updateDto);
        await _unitOfWork.SaveAsync();

        DiaryReadDto result = await _diaryAppService.GetById(createDto.Id);
        result.ExpirationDate.Should().Be(updateDto.ExpirationDate);
    }

    [Fact]
    public async Task AddEntry()
    {
        DiaryCreateDto diaryCreateDto = new DiaryCreateDto() { Id = Guid.NewGuid() };
        await _diaryAppService.Create(diaryCreateDto);
        await _unitOfWork.SaveAsync();
        EntryCreateDto entryCreateDto = new EntryCreateDto() {
            Id = Guid.NewGuid(),
            State = "Normal"
        };

        await _diaryAppService.AddEntry(diaryCreateDto.Id, entryCreateDto);
        await _unitOfWork.SaveAsync();

        DiaryReadDto result = await _diaryAppService.GetById(diaryCreateDto.Id);
        result.Entries.Should().Contain(entry => entry.Id == entryCreateDto.Id);
    }

    [Fact]
    public async Task RemoveEntry()
    {
        DiaryCreateDto diaryCreateDto = new DiaryCreateDto() { Id = Guid.NewGuid() };
        await _diaryAppService.Create(diaryCreateDto);
        await _unitOfWork.SaveAsync();
        EntryCreateDto entryCreateDto = new EntryCreateDto() {
            Id = Guid.NewGuid(),
            State = "Normal"
        };
        await _diaryAppService.AddEntry(diaryCreateDto.Id, entryCreateDto);
        await _unitOfWork.SaveAsync();

        await _diaryAppService.RemoveEntry(diaryCreateDto.Id, entryCreateDto.Id);
        await _unitOfWork.SaveAsync();

        DiaryReadDto result = await _diaryAppService.GetById(diaryCreateDto.Id);
        result.Entries.Should().NotContain(entry => entry.Id == entryCreateDto.Id);
    }

    [Fact]
    public async Task DeleteADiary()
    {
        DiaryCreateDto diaryCreateDto = new DiaryCreateDto() { Id = Guid.NewGuid() };
        await _diaryAppService.Create(diaryCreateDto);
        await _unitOfWork.SaveAsync();

        await _diaryAppService.Delete(diaryCreateDto.Id);
        await _unitOfWork.SaveAsync();

        Func<Task<DiaryReadDto>> result = async () => await _diaryAppService.GetById(diaryCreateDto.Id);
        await result.Should().ThrowAsync<ValueNotFoundException>();
    }  

    [Fact]
    public async Task DisableADiary()
    {
        DiaryCreateDto diaryCreateDto = new DiaryCreateDto() { Id = Guid.NewGuid() };
        await _diaryAppService.Create(diaryCreateDto);
        await _unitOfWork.SaveAsync();

        await _diaryAppService.Disable(diaryCreateDto.Id);
        await _unitOfWork.SaveAsync();

        DiaryReadDto result = await _diaryAppService.GetById(diaryCreateDto.Id);
        result.IsDisabled.Should().BeTrue();
    }

    [Fact]
    public async Task EnableADiary()
    {
        DiaryCreateDto diaryCreateDto = new DiaryCreateDto() { Id = Guid.NewGuid() };
        await _diaryAppService.Create(diaryCreateDto);
        await _unitOfWork.SaveAsync();
        await _diaryAppService.Disable(diaryCreateDto.Id);
        await _unitOfWork.SaveAsync();

        await _diaryAppService.Enable(diaryCreateDto.Id);
        await _unitOfWork.SaveAsync();

        DiaryReadDto result = await _diaryAppService.GetById(diaryCreateDto.Id);
        result.IsDisabled.Should().BeFalse();
    }
}