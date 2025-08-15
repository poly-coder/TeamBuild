using TeamBuild.Projects.Blazor.CultureFeature.CultureList;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Blazor.CultureFeature;

internal static class CultureMapper
{
    public static CultureDefineCommand MapToCommand(this CultureNewView.FormModel form)
    {
        return new CultureDefineCommand(
            CultureCode: form.CultureCode,
            EnglishName: form.EnglishName,
            NativeName: form.NativeName
        );
    }

    public static CultureListQuery MapToQuery(this CultureListSearchView.FormModel form)
    {
        return new CultureListQuery(TextSearch: form.TextSearch);
    }
}
