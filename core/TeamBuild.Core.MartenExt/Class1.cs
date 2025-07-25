﻿using Marten;
using TeamBuild.Core.Domain;

namespace TeamBuild.Core.MartenExt;

public static class AggregateMartenExtensions
{
    public static async Task<Result<TSuccess, DomainError>> GetByIdAsync<
        TDomainAggregate,
        TAggregateDao,
        TSuccess
    >(
        this IDocumentSession session,
        string entityName,
        string entityId,
        Func<TAggregateDao?, Option<TDomainAggregate>> mapToDomainAggregate,
        Func<TDomainAggregate, TSuccess> mapToSuccess,
        CancellationToken cancel = default
    )
        where TDomainAggregate : class
        where TAggregateDao : class
    {
        ArgumentNullException.ThrowIfNull(session);
        ArgumentException.ThrowIfNullOrWhiteSpace(entityId);
        ArgumentNullException.ThrowIfNull(mapToDomainAggregate);
        ArgumentNullException.ThrowIfNull(mapToSuccess);

        var dao = await session.Events.AggregateStreamAsync<TAggregateDao>(entityId, token: cancel);

        return mapToDomainAggregate(dao)
            .Match<Result<TSuccess, DomainError>>(
                some: entity => mapToSuccess(entity),
                none: () => DomainError.NotFound(entityName, entityId)
            );
    }

    public static async Task<Result<TSuccess, DomainError>> GetByIdsAsync<
        TDomainAggregate,
        TAggregateDao,
        TSuccess
    >(
        this IDocumentSession session,
        IEnumerable<string> entityIds,
        Func<TAggregateDao?, Option<TDomainAggregate>> mapToDomainAggregate,
        Func<Option<TDomainAggregate>[], TSuccess> mapToSuccess,
        CancellationToken cancel = default
    )
        where TDomainAggregate : class
        where TAggregateDao : class
    {
        ArgumentNullException.ThrowIfNull(session);
        ArgumentNullException.ThrowIfNull(entityIds);
        ArgumentNullException.ThrowIfNull(mapToDomainAggregate);
        ArgumentNullException.ThrowIfNull(mapToSuccess);

        var options = await entityIds
            .Select(async id =>
            {
                var dao = await session.Events.AggregateStreamAsync<TAggregateDao>(
                    id,
                    token: cancel
                );

                return mapToDomainAggregate(dao);
            })
            .WhenAll();

        return mapToSuccess(options);
    }

    public static async Task<Result<AggregateCommandSuccess, DomainError>> ExecuteCommandAsync<
        TDomainAggregate,
        TDomainEvent,
        TAggregateDao,
        TEventDao
    >(
        this IDocumentSession session,
        string entityId,
        Func<TAggregateDao?, Option<TDomainAggregate>> mapToDomainAggregate,
        Func<
            Option<TDomainAggregate>,
            Result<IReadOnlyCollection<TDomainEvent>, DomainError>
        > createEvents,
        Func<TDomainEvent, TEventDao> mapToEventDao,
        CancellationToken cancel = default
    )
        where TDomainAggregate : class
        where TAggregateDao : class
        where TDomainEvent : class
        where TEventDao : class
    {
        ArgumentNullException.ThrowIfNull(session);
        ArgumentException.ThrowIfNullOrWhiteSpace(entityId);
        ArgumentNullException.ThrowIfNull(createEvents);
        ArgumentNullException.ThrowIfNull(mapToEventDao);

        var stream = await session.Events.FetchForWriting<TAggregateDao>(entityId, cancel);

        var aggregate = mapToDomainAggregate(stream.Aggregate);

        var result = createEvents(aggregate);

        switch (result)
        {
            case Result<IReadOnlyCollection<TDomainEvent>, DomainError>.Ok ok:
                if (ok.Value is not { Count: > 0 } events)
                    return new AggregateCommandSuccess(0);

                stream.AppendMany(events: events.Select(object (e) => mapToEventDao(e)));

                await session.SaveChangesAsync(cancel);

                return new AggregateCommandSuccess(events.Count);

            case Result<IReadOnlyCollection<TDomainEvent>, DomainError>.Error error:
                return error.Err;

            default:
                throw DomainException.UnexpectedCaseType("result", result);
        }
    }
}

public abstract class AggregateMartenService<
    TDomainAggregate,
    TDomainEvent,
    TAggregateDao,
    TEventDao
>(IDocumentSession session)
    where TDomainAggregate : class
    where TDomainEvent : class
    where TAggregateDao : class
    where TEventDao : class
{
    protected abstract string EntityName { get; }
    protected IDocumentSession Session { get; } =
        session ?? throw new ArgumentNullException(nameof(session));

    protected abstract Option<TDomainAggregate> MapToDomainAggregate(TAggregateDao? dao);

    protected abstract TEventDao MapToEventDao(TDomainEvent domainEvent);

    protected async Task<Result<TSuccess, DomainError>> GetByIdAsync<TSuccess>(
        string entityId,
        Func<TDomainAggregate, TSuccess> mapToSuccess,
        CancellationToken cancel = default
    )
    {
        return await session.GetByIdAsync<TDomainAggregate, TAggregateDao, TSuccess>(
            EntityName,
            entityId,
            MapToDomainAggregate,
            mapToSuccess,
            cancel: cancel
        );
    }

    protected async Task<Result<TSuccess, DomainError>> GetByIdsAsync<TSuccess>(
        string[] entityIds,
        Func<Option<TDomainAggregate>[], TSuccess> mapToSuccess,
        CancellationToken cancel = default
    )
    {
        return await session.GetByIdsAsync<TDomainAggregate, TAggregateDao, TSuccess>(
            entityIds,
            MapToDomainAggregate,
            mapToSuccess,
            cancel: cancel
        );
    }

    protected async Task<Result<AggregateCommandSuccess, DomainError>> ExecuteCommandAsync(
        string entityId,
        Func<
            Option<TDomainAggregate>,
            Result<IReadOnlyCollection<TDomainEvent>, DomainError>
        > createEvents,
        CancellationToken cancel = default
    )
    {
        return await session.ExecuteCommandAsync<
            TDomainAggregate,
            TDomainEvent,
            TAggregateDao,
            TEventDao
        >(entityId, MapToDomainAggregate, createEvents, MapToEventDao, cancel);
    }
}
