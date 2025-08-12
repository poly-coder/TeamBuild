using Marten;
using TeamBuild.Core;
using TeamBuild.Projects.Application.CultureFeature;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Infrastructure.CultureFeature;

internal class CultureMartenCommandService(IDocumentSession session) : ICultureCommandService
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
