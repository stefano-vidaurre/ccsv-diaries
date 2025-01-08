namespace CCSV.Diaries.Dtos.Entries;

public class EntryReadDto
{
    public Guid Id { get; init; }
    public Guid DiaryId { get; init; }
    public string? EntityCreationDate { get; init; }
    public string? EntityEditionDate { get; init; }
    public string? State { get; init; }
}
