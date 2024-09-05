using CCSV.Diaries.Dtos.Diaries;
using CCSV.Rest.Validators;

namespace CCSV.Diaries.Services.Validators;

public class DiaryCreateDtoValidator : Validator<DiaryCreateDto>
{
    public DiaryCreateDtoValidator() {
        RuleFor(x => x.Id);
    }
}