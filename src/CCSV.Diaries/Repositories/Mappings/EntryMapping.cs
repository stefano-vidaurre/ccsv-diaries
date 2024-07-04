using CCSV.Data.EFCore.Mappings;
using CCSV.Diaries.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CCSV.Diaries.Repositories.Mappings;

public class EntryMapping : EntityMapping<Entry> {
    protected override string TableName => "CCSV_Diaries_Entries";

    protected override void ConfigureMapping(EntityTypeBuilder<Entry> builder) {
        builder.Property(x => x.State).HasConversion(typeof(string));

        builder.HasOne<Diary>().WithMany(diary => diary.Entries).HasForeignKey(entry => entry.DiaryId).OnDelete(DeleteBehavior.ClientCascade);
    }
}
