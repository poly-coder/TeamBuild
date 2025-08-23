using System.Diagnostics.CodeAnalysis;
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
        return new CultureListQuery(Filter: new(Search: form.Search));
    }

    public static CultureNewFormView.FormModel MapToNewForm(
        this Domain.CultureFeature.CultureDetails culture
    )
    {
        return new CultureNewFormView.FormModel
        {
            CultureCode = culture.CultureCode,
            EnglishName = culture.EnglishName,
            NativeName = culture.NativeName,
        };
    }

    public static CultureEditView.FormModel MapToEditForm(
        this Domain.CultureFeature.CultureDetails culture
    )
    {
        return new CultureEditView.FormModel
        {
            CultureCode = culture.CultureCode,
            EnglishName = culture.EnglishName,
            NativeName = culture.NativeName,
        };
    }

    [return: NotNullIfNotNull(nameof(culture))]
    public static string? MapToString(this Domain.CultureFeature.CultureDetails? culture)
    {
        return culture != null ? $"{culture.EnglishName} ({culture.CultureCode})" : null;
    }
}
