using System.Diagnostics.Metrics;
using Marten;
using Marten.Schema;
using TeamBuild.Projects.Application.CultureFeature;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Infrastructure.CultureFeature;

internal class CultureCommandMartenService(IDocumentSession session) : ICultureCommandService
{
    private static readonly Counter<int> DefineCounter = new Meter(
        "TeamBuild.Projects.Infrastructure.CultureCommandMartenService"
    ).CreateCounter<int>("DefineCultureCommand");

    public async Task<CultureDefineCommandSuccess> DefineAsync(
        CultureDefineCommand command,
        CancellationToken cancel = default
    )
    {
        var doc = new CultureDocument(command.CultureCode, command.EnglishName, command.NativeName);

        session.Store(doc);

        await session.SaveChangesAsync(cancel);

        return new CultureDefineCommandSuccess(doc.MapToDetails());
    }

    public async Task<CultureDeleteCommandSuccess> DeleteAsync(
        CultureDeleteCommand command,
        CancellationToken cancel = default
    )
    {
        session.Delete<CultureDocument>(command.CultureCode);

        await session.SaveChangesAsync(cancel);

        return new CultureDeleteCommandSuccess();
    }
}

[DocumentAlias("cultures")]
public record CultureDocument(
    [property: Identity] string CultureCode,
    string EnglishName,
    string NativeName
);

internal static class CultureCommandServiceMapper
{
    internal static CultureDetails MapToDetails(this CultureDocument doc) =>
        new(doc.CultureCode, doc.EnglishName, doc.NativeName);
}
