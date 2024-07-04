using CCSV.Data.EFCore;
using CCSV.Diaries.Models;
using Microsoft.EntityFrameworkCore;

namespace CCSV.Diaries.Repositories;

public class DiaryRepository : Repository<Diary>, IDiaryRepository {
    private readonly ApplicationContext _context;

    public DiaryRepository(ApplicationContext context) : base(context) {
        _context = context;
    }

    public override Task<Diary?> GetByIdOrDefault(Guid id) {
        return _context.Set<Diary>()
            .Include(diary => diary.Entries)
            .FirstOrDefaultAsync(diary => diary.Id == id);
    }
}
