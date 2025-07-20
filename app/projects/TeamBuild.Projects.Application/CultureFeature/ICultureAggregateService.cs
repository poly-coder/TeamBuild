using TeamBuild.Core;
using TeamBuild.Core.Domain;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Application.CultureFeature;

public interface ICultureAggregateService
{
    // Detail Queries

    Task<Result<CultureGetByIdQuerySuccess, DomainError>> GetByIdAsync(
        CultureGetByIdQuery query,
        CancellationToken cancel = default
    );

    Task<Result<CultureGetByIdsQuerySuccess, DomainError>> GetByIdsAsync(
        CultureGetByIdsQuery query,
        CancellationToken cancel = default
    );

    // Commands

    Task<Result<AggregateCommandSuccess, DomainError>> CreateAsync(
        CultureCreateCommand command,
        CancellationToken cancel = default
    );

    Task<Result<AggregateCommandSuccess, DomainError>> UpdateAsync(
        CultureUpdateCommand command,
        CancellationToken cancel = default
    );

    Task<Result<AggregateCommandSuccess, DomainError>> DeleteAsync(
        CultureDeleteCommand command,
        CancellationToken cancel = default
    );
}
