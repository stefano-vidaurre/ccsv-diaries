using CCSV.Diaries.Models;
using CCSV.Domain.Exceptions;
using FluentAssertions;

namespace CCSV.Diaries.Tests.Models;

public class DiaryShould
{
    [Fact]
    public void FailWhenTryToCreateWithEmptyId()
    {
        Guid id = Guid.Empty;

        Func<Diary> result = () => new Diary(id);

        result.Should().Throw<InvalidValueException>();
    }

    [Fact]
    public void AddingANewEntry()
    {
        Diary diary = new Diary(Guid.NewGuid());
        Guid entryId = Guid.NewGuid();

        diary.AddEntry(entryId, State.Normal);
    
        Entry result = diary.GetEntry(entryId);
        result.Id.Should().Be(entryId);
        result.State.Should().Be(State.Normal);
    }

    [Fact]
    public void FailWhenTryToAddADuplicatedEntry()
    {
        Diary diary = new Diary(Guid.NewGuid());
        Guid entryId = Guid.NewGuid();
        diary.AddEntry(entryId, State.Normal);

        Action result = () => diary.AddEntry(entryId, State.Normal);
    
        result.Should().Throw<DuplicatedValueException>();
    }

    [Fact]
    public void RemoveAEntry()
    {
        Diary diary = new Diary(Guid.NewGuid());
        Guid entryId = Guid.NewGuid();
        diary.AddEntry(entryId, State.Normal);
    
        Entry entry = diary.GetEntry(entryId);
        diary.RemoveEntry(entry);
    
        Func<Entry> result = () => diary.GetEntry(entryId);
        result.Should().Throw<ValueNotFoundException>();
    }

    [Fact]
    public void FailWhenTryToRemoveANonexistentEntry()
    {
        Diary diary = new Diary(Guid.NewGuid());
        Guid entryId = Guid.NewGuid();
        diary.AddEntry(entryId, State.Normal);
        Entry entry = diary.GetEntry(entryId);
        diary.RemoveEntry(entry);

        Action result = () => diary.RemoveEntry(entry);
        result.Should().Throw<WrongOperationException>();
    }
}
