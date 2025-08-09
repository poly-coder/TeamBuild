//using Marten;
//using TeamBuild.Core.Domain;

//namespace TeamBuild.Core.MartenExt;

//public abstract class AggregateMartenService<
//    TDomainAggregate,
//    TDomainEvent,
//    TAggregateDao,
//    TEventDao
//>(IDocumentSession session)
//    where TDomainAggregate : class
//    where TDomainEvent : class
//    where TAggregateDao : class
//    where TEventDao : class
//{
//    protected abstract string EntityName { get; }
//    protected IDocumentSession Session { get; } =
//        session ?? throw new ArgumentNullException(nameof(session));

//    protected abstract Option<TDomainAggregate> MapToDomainAggregate(TAggregateDao? dao);

//    protected abstract TEventDao MapToEventDao(TDomainEvent domainEvent);

//    protected async Task<Result<TSuccess, DomainError>> GetByIdAsync<TSuccess>(
//        string entityId,
//        Func<TDomainAggregate, TSuccess> mapToSuccess,
//        CancellationToken cancel = default
//    )
//    {
//        return await session.GetByIdAsync<TDomainAggregate, TAggregateDao, TSuccess>(
//            EntityName,
//            entityId,
//            MapToDomainAggregate,
//            mapToSuccess,
//            cancel: cancel
//        );
//    }

//    protected async Task<Result<TSuccess, DomainError>> GetByIdsAsync<TSuccess>(
//        string[] entityIds,
//        Func<Option<TDomainAggregate>[], TSuccess> mapToSuccess,
//        CancellationToken cancel = default
//    )
//    {
//        return await session.GetByIdsAsync<TDomainAggregate, TAggregateDao, TSuccess>(
//            entityIds,
//            MapToDomainAggregate,
//            mapToSuccess,
//            cancel: cancel
//        );
//    }

//    protected async Task<Result<AggregateCommandSuccess, DomainError>> ExecuteCommandAsync(
//        string entityId,
//        Func<
//            Option<TDomainAggregate>,
//            Result<IReadOnlyCollection<TDomainEvent>, DomainError>
//        > createEvents,
//        CancellationToken cancel = default
//    )
//    {
//        return await session.ExecuteCommandAsync<
//            TDomainAggregate,
//            TDomainEvent,
//            TAggregateDao,
//            TEventDao
//        >(entityId, MapToDomainAggregate, createEvents, MapToEventDao, cancel);
//    }
//}
