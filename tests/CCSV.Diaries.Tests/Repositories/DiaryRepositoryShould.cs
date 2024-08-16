using CCSV.Data.EFCore;
using CCSV.Diaries.Contexts;
using CCSV.Diaries.Models;
using CCSV.Diaries.Repositories;
using CCSV.Domain.Exceptions;
using FluentAssertions;

namespace CCSV.Diaries.Tests.Repositories;

public class DiaryRepositoryShould : IDisposable
{
    private readonly IDiaryRepository _diaryRepository;
    private readonly ApplicationContext _applicationContext;

    public DiaryRepositoryShould()
    {
        _applicationContext = InMemoryApplicationContext.Create();
        _diaryRepository = new DiaryRepository(_applicationContext);
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
        IEnumerable<Diary> diaries = await _diaryRepository.GetAll();
        diaries.Should().BeEmpty();
    }

    [Fact]
    public async Task SaveADiary()
    {
        Diary expected = new Diary(Guid.NewGuid());

        await _diaryRepository.Create(expected);
        await _applicationContext.SaveChangesAsync();

        Diary result = await _diaryRepository.GetById(expected.Id);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task UpdateADiary()
    {
        Diary expected = new Diary(Guid.NewGuid());
        await _diaryRepository.Create(expected);
        await _applicationContext.SaveChangesAsync();

        expected.AddEntry(Guid.NewGuid(), State.Normal);
        await _diaryRepository.Update(expected);
        await _applicationContext.SaveChangesAsync();

        Diary result = await _diaryRepository.GetById(expected.Id);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task DeleteADiary()
    {
        Diary expected = new Diary(Guid.NewGuid());
        await _diaryRepository.Create(expected);
        await _applicationContext.SaveChangesAsync();

        await _diaryRepository.Delete(expected);
        await _applicationContext.SaveChangesAsync();

        Func<Task<Diary>> result = async () => await _diaryRepository.GetById(expected.Id);
        await result.Should().ThrowAsync<ValueNotFoundException>();
    }

    [Fact]
    public async Task LogicDeleteADiary()
    {
        Diary expected = new Diary(Guid.NewGuid());
        await _diaryRepository.Create(expected);
        await _applicationContext.SaveChangesAsync();

        expected.SetAsDeleted();
        await _diaryRepository.Update(expected);
        await _applicationContext.SaveChangesAsync();

        IEnumerable<Diary> result = await _diaryRepository.GetAll();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetLogicDeleteDiary()
    {
        Diary expected = new Diary(Guid.NewGuid());
        await _diaryRepository.Create(expected);
        await _applicationContext.SaveChangesAsync();
        expected.SetAsDeleted();
        await _diaryRepository.Update(expected);
        await _applicationContext.SaveChangesAsync();

        IEnumerable<Diary> result = await _diaryRepository.GetAll(true);

        result.Should().ContainEquivalentOf(expected);
    }
}
