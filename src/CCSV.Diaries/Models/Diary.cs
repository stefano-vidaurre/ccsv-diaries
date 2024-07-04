using CCSV.Domain.Entities;
using CCSV.Domain.Entities.Collections;
using CCSV.Domain.Exceptions;

namespace CCSV.Diaries.Models;

public class Diary : Entity {
    public static Diary Default => new Diary();

    private readonly IEntityList<Entry> _entries;

    public IEnumerable<Entry> Entries => _entries.AsEnumerable();

    private Diary() : base(Guid.Empty) {
        _entries = new EntityList<Entry>();
    }

    public Diary(Guid id) : base(id) {
        _entries = new EntityList<Entry>();
    }

    public void AddEntry(Guid id, State state) {
        Entry entry = new Entry(id, this, state);
        _entries.Add(entry);
    }

    public void RemoveEntry(Entry entry) {
        if(entry is null) {
            throw new BusinessException("The entry cant be null.");
        }

        _entries.Remove(entry);
    }
}
