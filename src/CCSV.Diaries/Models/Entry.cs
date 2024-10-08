using CCSV.Domain.Entities;
using CCSV.Domain.Exceptions;

namespace CCSV.Diaries.Models;

public class Entry : Entity
{
    public static Entry Default => new Entry();

    public Guid DiaryId { get; private set; }
    public State State { get; private set; }

    private Entry() : base(Guid.Empty)
    {
        DiaryId = Guid.Empty;
        State = State.None;
    }

    public Entry(Guid id, Diary diary, State state) : base(id)
    {
        if (id == Guid.Empty)
        {
            throw new InvalidValueException("The entry id cant be empty.");
        }

        if (diary is null)
        {
            throw new BusinessException("The diary cant be null.");
        }

        if (state == State.None)
        {
            throw new InvalidValueException("The state cant be 'None'.");
        }

        DiaryId = diary.Id;
        State = state;
    }
}
