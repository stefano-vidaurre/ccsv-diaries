namespace CCSV.Diaries.Dtos.Entries;

public class EntryReadDto
{
    public Guid Id { get; set; }
    public Guid DiaryId { get; set; }
    public string? EntityCreationDate { get; set; }
    public string? EntityEditionDate { get; set; }
    public string? State { get; set; }
}
