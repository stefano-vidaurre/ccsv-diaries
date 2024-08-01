using CCSV.Domain.Entities;
using CCSV.Domain.Entities.Collections;
using CCSV.Domain.Exceptions;

namespace CCSV.Diaries.Models;

public class Diary : Entity
{
    public static Diary Default => new Diary();

    private readonly IEntityList<Entry> _entries;

    public DateTime ExpirationDate { get; private set; }

    public IEnumerable<Entry> Entries => _entries.AsEnumerable();

    private Diary() : base(Guid.Empty)
    {
        ExpirationDate = DateTime.MaxValue;
        _entries = new EntityList<Entry>();
    }

    public Diary(Guid id) : base(id)
    {
        if (id == Guid.Empty)
        {
            throw new InvalidValueException("The diary id cant be empty.");
        }

        ExpirationDate = DateTime.MaxValue;
        _entries = new EntityList<Entry>();
    }

    public void SetExpirationDate(DateTime expirationDate)
    {
        if (expirationDate.Kind != DateTimeKind.Utc)
        {
            throw new BusinessException("The diary expiration date is not UTC.");
        }

        ExpirationDate = expirationDate;
    }

    public void AddEntry(Guid id, State state)
    {
        if (_entries.GetByIdOrDefault(id) is not null)
        {
            throw new DuplicatedValueException("The entry already belongs to the diary.");
        }

        if (ExpirationDate < DateTime.UtcNow)
        {
            throw new NotAllowedOperationException("The diary expiration date is out of date.");
        }

        Entry entry = new Entry(id, this, state);
        _entries.Add(entry);
    }

    public Entry GetEntry(Guid id)
    {
        Entry? entry = _entries.GetByIdOrDefault(id);

        if (entry is null)
        {
            throw new ValueNotFoundException("The entry doesn't belong to the diary.");
        }

        return entry;
    }

    public Entry? GetEntryOrDefault(Guid id)
    {
        return _entries.GetByIdOrDefault(id);
    }

    public void RemoveEntry(Entry entry)
    {
        if (entry is null)
        {
            throw new BusinessException("The entry cant be null.");
        }

        if (ExpirationDate < DateTime.UtcNow)
        {
            throw new NotAllowedOperationException("The expiration date of the diary is out of date.");
        }

        if (!_entries.Remove(entry))
        {
            throw new WrongOperationException("The entry doesn't belong to the diary.");
        }
    }
}
