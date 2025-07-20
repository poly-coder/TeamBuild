using Marten;
using TeamBuild.Core;
using TeamBuild.Core.Domain;
using TeamBuild.Core.MartenExt;
using TeamBuild.Projects.Application.CultureFeature;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Infrastructure.CultureFeature;

internal class CultureAggregateMartenService(IDocumentSession session)
    : AggregateMartenService<
        CultureAggregate,
        CultureDomainEvent,
        CultureAggregateDao,
        CultureEventDao
    >(session),
        ICultureAggregateService
{
    protected override string EntityName => CultureAggregate.EntityName;

    protected override CultureEventDao MapToEventDao(CultureDomainEvent domainEvent) =>
        domainEvent.MapToDao();

    protected override Option<CultureAggregate> MapToDomainAggregate(CultureAggregateDao? dao) =>
        dao.MapToDomain();

    public async Task<Result<CultureGetByIdQuerySuccess, DomainError>> GetByIdAsync(
        CultureGetByIdQuery query,
        CancellationToken cancel = default
    ) =>
        await GetByIdAsync(
            query.CultureCode,
            entity => new CultureGetByIdQuerySuccess(entity),
            cancel: cancel
        );

    public async Task<Result<CultureGetByIdsQuerySuccess, DomainError>> GetByIdsAsync(
        CultureGetByIdsQuery query,
        CancellationToken cancel = default
    ) =>
        await GetByIdsAsync(
            query.CultureCodes,
            list => new CultureGetByIdsQuerySuccess(list),
            cancel: cancel
        );

    public async Task<Result<AggregateCommandSuccess, DomainError>> CreateAsync(
        CultureCreateCommand command,
        CancellationToken cancel = default
    ) =>
        await ExecuteCommandAsync(
            command.CultureCode,
            agg => CultureAggregate.Create(agg, command),
            cancel
        );

    public async Task<Result<AggregateCommandSuccess, DomainError>> UpdateAsync(
        CultureUpdateCommand command,
        CancellationToken cancel = default
    ) =>
        await ExecuteCommandAsync(
            command.CultureCode,
            agg => CultureAggregate.Update(agg, command),
            cancel
        );

    public async Task<Result<AggregateCommandSuccess, DomainError>> DeleteAsync(
        CultureDeleteCommand command,
        CancellationToken cancel = default
    ) =>
        await ExecuteCommandAsync(
            command.CultureCode,
            agg => CultureAggregate.Delete(agg, command),
            cancel
        );
}
