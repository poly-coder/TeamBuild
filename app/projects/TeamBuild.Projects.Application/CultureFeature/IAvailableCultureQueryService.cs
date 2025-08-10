using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Application.CultureFeature;

public interface IAvailableCultureQueryService
{
    Task<AvailableCultureListQuerySuccess> ListAsync(
        AvailableCultureListQuery query,
        CancellationToken cancel = default
    );
}
