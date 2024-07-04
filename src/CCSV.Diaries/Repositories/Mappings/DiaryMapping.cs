using CCSV.Data.EFCore.Mappings;
using CCSV.Diaries.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CCSV.Diaries.Repositories.Mappings;

public class DiaryMapping : EntityMapping<Diary> {
    protected override string TableName => "CCSV_Diaries_Diaries";

    protected override void ConfigureMapping(EntityTypeBuilder<Diary> builder) {
        builder.Navigation(diary => diary.Entries).HasField("_entries").UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
