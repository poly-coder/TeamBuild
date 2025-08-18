using TeamBuild.Projects.Blazor.CultureFeature.CultureEdit;
using TeamBuild.Projects.Blazor.CultureFeature.CultureList;
using TeamBuild.Projects.Blazor.CultureFeature.CultureNew;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Blazor.CultureFeature;

internal static class CultureMapper
{
    public static CultureDefineCommand MapToCommand(this CultureNewFormView.FormModel form)
    {
        return new CultureDefineCommand(
            CultureCode: form.CultureCode,
            EnglishName: form.EnglishName,
            NativeName: form.NativeName
        );
    }

    public static CultureDefineCommand MapToCommand(this CultureEditView.FormModel form)
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

    public static CultureEditView.FormModel MapToEditForm(
        this Domain.CultureFeature.CultureDetails form
    )
    {
        return new CultureEditView.FormModel
        {
            CultureCode = form.CultureCode,
            EnglishName = form.EnglishName,
            NativeName = form.NativeName,
        };
    }
}
