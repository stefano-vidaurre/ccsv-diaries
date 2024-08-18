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

        expected.SetAsDisabled();
        await _diaryRepository.Update(expected);
        await _applicationContext.SaveChangesAsync();

        IEnumerable<Diary> result = await _diaryRepository.GetAll();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetLogicDeletedDiary()
    {
        Diary expected = new Diary(Guid.NewGuid());
        await _diaryRepository.Create(expected);
        await _applicationContext.SaveChangesAsync();
        expected.SetAsDisabled();
        await _diaryRepository.Update(expected);
        await _applicationContext.SaveChangesAsync();

        IEnumerable<Diary> result = await _diaryRepository.GetAll(disabledIncluded: true);

        result.Should().ContainEquivalentOf(expected);
    }

    [Fact]
    public async Task ConfirmWhenItHasContent()
    {
        Diary expected = new Diary(Guid.NewGuid());
        await _diaryRepository.Create(expected);
        await _applicationContext.SaveChangesAsync();

        bool result = await _diaryRepository.Any();

        result.Should().BeTrue();
    }

    [Fact]
    public async Task RefuseWhenItIsEmpty()
    {
        bool result = await _diaryRepository.Any();

        result.Should().BeFalse();
    }

    [Fact]
    public async Task RefuseWhenItHasOnlyLogicDeletes()
    {
        Diary expected = new Diary(Guid.NewGuid());
        await _diaryRepository.Create(expected);
        await _applicationContext.SaveChangesAsync();
        expected.SetAsDisabled();
        await _diaryRepository.Update(expected);
        await _applicationContext.SaveChangesAsync();

        bool result = await _diaryRepository.Any();
        
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ConfirmWhenConsiderLogicDeletes()
    {
        Diary expected = new Diary(Guid.NewGuid());
        await _diaryRepository.Create(expected);
        await _applicationContext.SaveChangesAsync();
        expected.SetAsDisabled();
        await _diaryRepository.Update(expected);
        await _applicationContext.SaveChangesAsync();

        bool result = await _diaryRepository.Any(disabledIncluded: true);
        
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ReturnOneWhenItHasOneRecord()
    {
        Diary expected = new Diary(Guid.NewGuid());
        await _diaryRepository.Create(expected);
        await _applicationContext.SaveChangesAsync();

        int result = await _diaryRepository.Count();

        result.Should().Be(1);
    }

    [Fact]
    public async Task ReturnZeroWhenItIsEmpty()
    {
        int result = await _diaryRepository.Count();

        result.Should().Be(0);
    }

    [Fact]
    public async Task ReturnZeroWhenItHasOnlyOneLogicDelete()
    {
        Diary expected = new Diary(Guid.NewGuid());
        await _diaryRepository.Create(expected);
        await _applicationContext.SaveChangesAsync();
        expected.SetAsDisabled();
        await _diaryRepository.Update(expected);
        await _applicationContext.SaveChangesAsync();

        int result = await _diaryRepository.Count();
        
        result.Should().Be(0);
    }

    [Fact]
    public async Task ReturnOneWhenConsiderLogicDeletes()
    {
        Diary expected = new Diary(Guid.NewGuid());
        await _diaryRepository.Create(expected);
        await _applicationContext.SaveChangesAsync();
        expected.SetAsDisabled();
        await _diaryRepository.Update(expected);
        await _applicationContext.SaveChangesAsync();

        int result = await _diaryRepository.Count(disabledIncluded: true);
        
        result.Should().Be(1);
    }

    [Fact]
    public async Task FilterExpiredDiaries()
    {
        Diary expired = new Diary(Guid.NewGuid());
        expired.SetExpirationDate(DateTime.UtcNow);
        Diary notExpired = new Diary(Guid.NewGuid());
        await _diaryRepository.Create(expired);
        await _diaryRepository.Create(notExpired);
        await _applicationContext.SaveChangesAsync();

        IEnumerable<Diary> result = await _diaryRepository.GetAll(query => query.Where(diary => diary.ExpirationDate > DateTime.UtcNow));
        
        result.Should().ContainEquivalentOf(notExpired);
        result.Should().NotContainEquivalentOf(expired);
    }
}
