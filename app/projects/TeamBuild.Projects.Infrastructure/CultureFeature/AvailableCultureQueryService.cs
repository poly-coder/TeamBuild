using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using TeamBuild.Core;
using TeamBuild.Core.Application;
using TeamBuild.Projects.Application.CultureFeature;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Infrastructure.CultureFeature;

public class AvailableCultureQueryService : IAvailableCultureQueryService
{
    public async Task<AvailableCultureListQuerySuccess> List(
        AvailableCultureListQuery query,
        CancellationToken cancel = default
    )
    {
        await Task.Yield();

        var list = CultureInfo
            .GetCultures(CultureTypes.AllCultures)
            .Where(c => !string.IsNullOrEmpty(c.Name))
            .ApplyFiltering(query.Filter)
            .ApplySorting()
            .ApplyPaging()
            .ApplyProjection()
            .ToList();

        return new AvailableCultureListQuerySuccess(list);
    }
}

internal static class AvailableCultureQueryExtensions
{
    public static IServiceCollection AddAvailableCultureQueryService(
        this IServiceCollection services
    ) =>
        services.AddDecoratedInfrastructureService<
            IAvailableCultureQueryService,
            AvailableCultureQueryService,
            AvailableCultureQueryServiceDecorator,
            AvailableCultureQueryServiceAspect
        >();

    public static IEnumerable<CultureInfo> ApplyFiltering(
        this IEnumerable<CultureInfo> query,
        AvailableCultureListQueryFilter? filter
    )
    {
        if (filter is null)
            return query;

        query = query.ApplyTextSearchData(
            filter.Search,
            e => e.Name.ToTextSearchData(e.EnglishName, e.NativeName)
        );

        return query;
    }

    public static IEnumerable<CultureInfo> ApplySorting(this IEnumerable<CultureInfo> query)
    {
        return query.OrderBy(doc => doc.EnglishName);
    }

    public static IEnumerable<CultureInfo> ApplyPaging(this IEnumerable<CultureInfo> query)
    {
        return query.Take(10);
    }

    public static IEnumerable<CultureDetails> ApplyProjection(this IEnumerable<CultureInfo> query)
    {
        return query.Select(doc => new CultureDetails(
            CultureCode: doc.Name,
            EnglishName: doc.EnglishName,
            NativeName: doc.NativeName
        ));
    }
}
