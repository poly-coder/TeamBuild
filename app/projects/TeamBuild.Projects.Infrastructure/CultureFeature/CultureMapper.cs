using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Infrastructure.CultureFeature;

internal static class CultureMapper
{
    internal static CultureDetails MapToDetails(this CultureDocument doc) =>
        new(doc.CultureCode, doc.EnglishName, doc.NativeName);
}
