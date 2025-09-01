using System.Runtime.CompilerServices;
using FluentValidation;
using TeamBuild.Core.Testing;

namespace TeamBuild.Core.Domain.Testing;

public static class ModelValidationTestExtensions
{
    public static async Task ShouldBeTestedFor<TModel>(
        this IValidator<TModel> validator,
        ModelValidationTestData<TModel> testData,
        Func<TModel, TModel>? coerce = null,
        [CallerFilePath] string sourceFile = "",
        [CallerMemberName] string? methodName = null
    )
    {
        var instance = testData.Instance;

        if (coerce is not null)
        {
            instance = coerce(instance);
        }

        var validation = await validator.ValidateAsync(instance);

        await Verify(new { instance, validation }, sourceFile: sourceFile)
            .UseSnapshotsDirectory()
            .UseRuleName(testData.Rule, methodName: methodName);
    }
}
