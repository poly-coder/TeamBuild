using Marten;
using Marten.Schema;
using TeamBuild.Core;
using TeamBuild.Projects.Application.CultureFeature;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Infrastructure.CultureFeature;

internal class CultureCommandMartenService(IDocumentSession session) : ICultureCommandService
{
    public async Task<CultureDefineCommandSuccess> Define(
        CultureDefineCommand command,
        CancellationToken cancel = default
    )
    {
        var textSearchData = command.CultureCode.ToTextSearchData(
            command.EnglishName,
            command.NativeName
        );

        var doc = new CultureDocument(
            command.CultureCode,
            command.EnglishName,
            command.NativeName,
            textSearchData
        );

        session.Store(doc);

        await session.SaveChangesAsync(cancel);

        return new CultureDefineCommandSuccess(doc.MapToDetails());
    }

    public async Task<CultureDeleteCommandSuccess> Delete(
        CultureDeleteCommand command,
        CancellationToken cancel = default
    )
    {
        session.Delete<CultureDocument>(command.CultureCode);

        await session.SaveChangesAsync(cancel);

        return new CultureDeleteCommandSuccess();
    }
}

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

internal static class CultureCommandServiceMapper
{
    internal static CultureDetails MapToDetails(this CultureDocument doc) =>
        new(doc.CultureCode, doc.EnglishName, doc.NativeName);
}
