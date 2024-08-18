using AutoMapper;
using CCSV.Diaries.Dtos.Diaries;
using CCSV.Diaries.Dtos.Entries;
using CCSV.Diaries.Models;
using CCSV.Diaries.Repositories;
using CCSV.Domain.Parsers;

namespace CCSV.Diaries.Services;

public class DiaryAppService : IDiaryAppService
{
    private readonly IDiaryRepository _diaryRepository;
    private readonly IMapper _mapper;

    public DiaryAppService(IDiaryRepository diaryRepository, IMapper mapper)
    {
        _diaryRepository = diaryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DiaryQueryDto>> GetAll()
    {
        IEnumerable<Diary> diaries = await _diaryRepository.GetAll();
        return _mapper.Map<IEnumerable<Diary>, IEnumerable<DiaryQueryDto>>(diaries);
    }

    public async Task<DiaryReadDto> GetById(Guid id)
    {
        Diary diary = await _diaryRepository.GetById(id);
        return _mapper.Map<Diary, DiaryReadDto>(diary);
    }

    public async Task Create(DiaryCreateDto data)
    {
        Diary diary = new Diary(data.Id);
        await _diaryRepository.Create(diary);
    }

    public async Task Update(Guid id, DiaryUpdateDto updateDto)
    {
        Diary diary = await _diaryRepository.GetById(id);
        DateTime expirationDate = DateTimeParser.ParseUTC(updateDto.ExpirationDate);
        diary.SetExpirationDate(expirationDate);
    }

    public async Task AddEntry(Guid diaryId, EntryCreateDto data)
    {
        Diary diary = await _diaryRepository.GetById(diaryId);
        State state = EnumParser.Parse<State>(data.State);
        diary.AddEntry(data.Id, state);
    }

    public async Task RemoveEntry(Guid diaryId, Guid entryId)
    {
        Diary diary = await _diaryRepository.GetById(diaryId);
        Entry entry = diary.GetEntry(entryId);
        diary.RemoveEntry(entry);
    }

    public async Task Delete(Guid id)
    {
        Diary diary = await _diaryRepository.GetById(id);
        await _diaryRepository.Delete(diary);
    }

    public async Task Enable(Guid id)
    {
        Diary diary = await _diaryRepository.GetById(id);
        diary.SetAsEnabled();
    }

    public async Task Disable(Guid id)
    {
        Diary diary = await _diaryRepository.GetById(id);
        diary.SetAsDisabled();
    }
}
