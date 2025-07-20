using JasperFx.Events;
using Marten.Schema;
using TeamBuild.Core;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Infrastructure.CultureFeature;

internal sealed record CultureAggregateDao(
    [property: Identity] string CultureCode,
    string EnglishName,
    string NativeName,
    bool IsDeleted = false
)
{
    public static CultureAggregateDao Create(IEvent<CultureCreatedEventDao> @event) =>
        CultureAggregate.Apply(Option.None<CultureAggregate>(), @event.MapToDomain()).MapToDao();

    public static CultureAggregateDao Apply(
        CultureAggregateDao aggregate,
        IEvent<CultureUpdatedEventDao> @event
    ) => CultureAggregate.Apply(aggregate.MapToDomain(), @event.MapToDomain()).MapToDao();

    public static CultureAggregateDao Apply(
        CultureAggregateDao aggregate,
        IEvent<CultureDeletedEventDao> @event
    ) => CultureAggregate.Apply(aggregate.MapToDomain(), @event.MapToDomain()).MapToDao();
}

internal static class CultureAggregateDaoMapper
{
    public static Option<CultureAggregate> MapToDomain(this CultureAggregateDao? dao) =>
        dao is { IsDeleted: false }
            ? new CultureAggregate(dao.CultureCode, dao.EnglishName, dao.NativeName)
            : Option.None<CultureAggregate>();

    public static CultureAggregateDao MapToDao(this Option<CultureAggregate> source) =>
        source.Match(
            item => new CultureAggregateDao(item.CultureCode, item.EnglishName, item.NativeName),
            () => new CultureAggregateDao("", "", "", true)
        );
}
