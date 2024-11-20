using CCSV.Diaries.Dtos.Diaries;
using CCSV.Rest.Validators;

namespace CCSV.Diaries.Services.Validators;

public class DiaryUpdateDtoValidator : Validator<DiaryUpdateDto>
{
    public DiaryUpdateDtoValidator() {
        RuleFor(x => x.ExpirationDate).NotNull();
    }
}