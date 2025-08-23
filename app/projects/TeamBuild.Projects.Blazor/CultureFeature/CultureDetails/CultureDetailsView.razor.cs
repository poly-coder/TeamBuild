using Microsoft.AspNetCore.Components;
using TeamBuild.Core.Blazor;

namespace TeamBuild.Projects.Blazor.CultureFeature.CultureDetails;

public sealed partial class CultureDetailsView
{
    [Parameter]
    public Domain.CultureFeature.CultureDetails? Culture { get; set; }

    private IEnumerable<FieldValue> FieldValues()
    {
        yield return new FieldValue("Code", Culture?.CultureCode);
        yield return new FieldValue("English", Culture?.EnglishName);
        yield return new FieldValue("Native", Culture?.NativeName);
    }
}
