using AutoMapper;
using CCSV.Diaries.Dtos.Diaries;
using CCSV.Diaries.Dtos.Entries;
using CCSV.Diaries.Models;
using CCSV.Diaries.Repositories;
using CCSV.Domain.Exceptions;
using CCSV.Domain.Parsers;

namespace CCSV.Diaries.Services;

public class DiaryAppService : IDiaryAppService
{
    private readonly IDiaryRepository _diaryRepository;
    private readonly IEntryRepository _entryRepository;
    private readonly IMapper _mapper;

    public DiaryAppService(IDiaryRepository diaryRepository,
        IEntryRepository entryRepository,
        IMapper mapper)
    {
        _diaryRepository = diaryRepository;
        _entryRepository = entryRepository;
        _mapper = mapper;
    }


    public async Task<IEnumerable<DiaryQueryDto>> GetAll(DiaryFilterDto filter)
    {
        // TODO: WIP in CCSV.Rest
        if (!string.IsNullOrWhiteSpace(filter.ExpirationDate))
        {
            string[] operatorValue = filter.ExpirationDate.Split(':', 2);
            if (operatorValue.Length != 2)
            {
                throw new WrongOperationException("The expiration date filter is not valid.");
            }

            string op = operatorValue[0];
            if (op != "gt" && op != "lt")
            {
                throw new WrongOperationException("The expiration date filter (operator) is not valid.");
            }

            if (!DateTimeParser.TryParseUTC(operatorValue[1], out DateTime expirationDate))
            {
                throw new WrongOperationException("The expiration date filter (value) is not valid.");
            }

            if (op == "gt")
            {
                IEnumerable<Diary> filterResult = await _diaryRepository.GetAll(query => query.Where(diary => diary.ExpirationDate > expirationDate));
                return _mapper.Map<IEnumerable<Diary>, IEnumerable<DiaryQueryDto>>(filterResult);
            }

            if (op == "lt")
            {
                IEnumerable<Diary> filterResult = await _diaryRepository.GetAll(query => query.Where(diary => diary.ExpirationDate < expirationDate));
                return _mapper.Map<IEnumerable<Diary>, IEnumerable<DiaryQueryDto>>(filterResult);
            }
        }

        IEnumerable<Diary> diaries = await _diaryRepository.GetAll();
        return _mapper.Map<IEnumerable<Diary>, IEnumerable<DiaryQueryDto>>(diaries);
    }

    public async Task<DiaryReadDto> GetById(Guid id)
    {
        Diary diary = await _diaryRepository.GetById(id);
        return _mapper.Map<Diary, DiaryReadDto>(diary);
    }

    public Task<int> GetLength()
    {
        return _diaryRepository.Count();
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

    public async Task<IEnumerable<EntryReadDto>> GetAllEntries(Guid diaryId, EntryFilterDto filter)
    {
        // TODO: WIP in CCSV.Rest
        if (!string.IsNullOrWhiteSpace(filter.State))
        {
            string[] operatorValue = filter.State.Split(':', 2);
            if (operatorValue.Length != 2)
            {
                throw new WrongOperationException("The state filter is not valid.");
            }

            string op = operatorValue[0];
            if (op != "eq" && op != "neq")
            {
                throw new WrongOperationException("The state filter (operator) is not valid.");
            }

            if (!EnumParser.TryParse(operatorValue[1], out State state))
            {
                throw new WrongOperationException("The state filter (value) is not valid.");
            }

            if (op == "eq")
            {
                IEnumerable<Entry> filterResult = await _entryRepository.GetAll(query => query.Where(entry => entry.DiaryId == diaryId && entry.State == state));
                return _mapper.Map<IEnumerable<Entry>, IEnumerable<EntryReadDto>>(filterResult);
            }

            if (op == "neq")
            {
                IEnumerable<Entry> filterResult = await _entryRepository.GetAll(query => query.Where(entry => entry.DiaryId == diaryId && entry.State != state));
                return _mapper.Map<IEnumerable<Entry>, IEnumerable<EntryReadDto>>(filterResult);
            }
        }

        IEnumerable<Entry> entries = await _entryRepository.GetAll(query => query.Where(entry => entry.DiaryId == diaryId));
        return _mapper.Map<IEnumerable<Entry>, IEnumerable<EntryReadDto>>(entries);
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
