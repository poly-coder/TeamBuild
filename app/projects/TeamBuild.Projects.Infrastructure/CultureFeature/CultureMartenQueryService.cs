using Marten;
using TeamBuild.Core.Domain;
using TeamBuild.Core.MartenExt;
using TeamBuild.Projects.Application.CultureFeature;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Infrastructure.CultureFeature;

public class CultureMartenQueryService(IQuerySession session) : ICultureQueryService
{
    public async Task<CultureListQuerySuccess> List(
        CultureListQuery query,
        CancellationToken cancel = default
    )
    {
        var dbQuery = session
            .Query<CultureDocument>()
            .ApplyFiltering(query.TextSearch)
            .ApplySorting();

        var docs = await dbQuery.ToListAsync(cancel);

        var list = docs.Select(doc => doc.MapToDetails()).ToList();

        return new CultureListQuerySuccess(list);
    }

    public async Task<CultureGetByIdQuerySuccess> GetById(
        CultureGetByIdQuery query,
        CancellationToken cancel = default
    )
    {
        var doc = await session.LoadAsync<CultureDocument>(query.CultureCode, cancel);

        if (doc is null)
        {
            throw new EntityNotFoundException(CultureEntity.Caption, query.CultureCode);
        }

        return new CultureGetByIdQuerySuccess(doc.MapToDetails());
    }

    public async Task<CultureGetByIdsQuerySuccess> GetByIds(
        CultureGetByIdsQuery query,
        CancellationToken cancel = default
    )
    {
        var docs = await session.LoadManyAsync<CultureDocument>(cancel, query.CultureCodes);

        var list = docs.Select(doc => doc.MapToDetails()).ToList();

        return new CultureGetByIdsQuerySuccess(list);
    }
}

internal static class CultureMartenQueryExtensions
{
    public static IQueryable<CultureDocument> ApplyFiltering(
        this IQueryable<CultureDocument> query,
        string? textSearch
    )
    {
        return query.ApplyTextSearchData(textSearch, doc => doc.TextSearchData);
    }

    public static IQueryable<CultureDocument> ApplySorting(this IQueryable<CultureDocument> query)
    {
        return query.OrderBy(doc => doc.EnglishName);
    }
}
