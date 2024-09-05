using CCSV.Rest.Validators;

namespace CCSV.Diaries.Services.Validators;

public static class ValidatorFactory
{
    public static IMasterValidator Create()
    {
        return MasterValidator.CreateByAssembly(typeof(ValidatorFactory).Assembly);
    }
}