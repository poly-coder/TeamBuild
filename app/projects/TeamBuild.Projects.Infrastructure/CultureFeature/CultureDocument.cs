using Marten.Schema;

namespace TeamBuild.Projects.Infrastructure.CultureFeature;

[DocumentAlias(TableName)]
public record CultureDocument(
    [property: Identity]
    [property: FullTextIndex(IndexName = CultureDocument.FullTextIndexName)]
    string CultureCode,
    [property: FullTextIndex(IndexName = CultureDocument.FullTextIndexName)] string EnglishName,
    [property: FullTextIndex(IndexName = CultureDocument.FullTextIndexName)] string NativeName,
    string TextSearchData
)
{
    public const string TableName = "cultures";
    public const string FullTextIndexName = $"idx_{TableName}_ft";
}
