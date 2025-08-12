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

    //public static CultureDeleteCommand MapToCommand(this CultureDeleteView.FormModel form)
    //{
    //    return new CultureDeleteCommand(
    //        CultureCode: form.CultureCode,
    //    );
    //}

    public static CultureListQuery MapToQuery(this CultureListView.FormModel form)
    {
        return new CultureListQuery(TextSearch: form.TextSearch);
    }
}
