namespace CCSV.Diaries.Dtos.Diaries;

public class DiaryQueryDto
{
    public Guid Id { get; init; }
    public string? EntityCreationDate { get; init; }
    public string? EntityEditionDate { get; init; }
    public string? EntityDisabledDate { get; init; }
    public string? ExpirationDate { get; init; }
    public bool IsDisabled { get; init; }
}
