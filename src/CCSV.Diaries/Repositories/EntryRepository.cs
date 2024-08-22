using CCSV.Data.EFCore;
using CCSV.Diaries.Models;
using CCSV.Diaries.Services;
using Microsoft.EntityFrameworkCore;

namespace CCSV.Diaries.Repositories;

public class EntryRepository : Repository<Entry>, IEntryRepository
{
    private readonly ApplicationContext _context;

    public EntryRepository(ApplicationContext context) : base(context)
    {
        _context = context;
    }

    public override Task<Entry?> GetByIdOrDefault(Guid id)
    {
        return _context.Set<Entry>().FirstOrDefaultAsync(entry => entry.Id == id);
    }
}
