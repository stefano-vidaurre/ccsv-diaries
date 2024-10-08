using CCSV.Diaries.Dtos.Entries;

namespace CCSV.Diaries.Dtos.Diaries;

public class DiaryReadDto
{
    public Guid Id { get; set; }
    public string? EntityCreationDate { get; set; }
    public string? EntityEditionDate { get; set; }
    public string? EntityDisabledDate { get; set; }
    public string? ExpirationDate { get; set; }
    public bool IsDisabled { get; set; }
    public IEnumerable<EntryReadDto>? Entries { get; set; }
}
