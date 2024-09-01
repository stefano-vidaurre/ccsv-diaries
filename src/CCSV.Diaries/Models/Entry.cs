using CCSV.Domain.Entities;
using CCSV.Domain.Exceptions;

namespace CCSV.Diaries.Models;

public class Entry : Entity
{
    public static Entry Default => new Entry();

    public Guid DiaryId { get; private set; }
    public State State { get; private set; }
    public string Description { get; private set; }

    private Entry() : base(Guid.Empty)
    {
        DiaryId = Guid.Empty;
        State = State.None;
        Description = string.Empty;
    }

    public Entry(Guid id, Diary diary, State state) : base(id)
    {
        if (id == Guid.Empty)
        {
            throw new InvalidValueException("The entry id can't be empty.");
        }

        if (diary is null)
        {
            throw new BusinessException("The diary can't be null.");
        }

        if (state == State.None)
        {
            throw new InvalidValueException("The state can't be 'None'.");
        }

        DiaryId = diary.Id;
        State = state;
        Description = string.Empty;
    }

    public void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidValueException("The description can't be empty or null.");
        }
        Description = description;
    }

    public void ClearDescription()
    {
        Description = string.Empty;
    }
}
