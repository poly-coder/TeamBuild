using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Application.CultureFeature;

public interface ICultureCommandService
{
    Task<CultureDefineCommandSuccess> DefineAsync(
        CultureDefineCommand command,
        CancellationToken cancel = default
    );

    Task<CultureDeleteCommandSuccess> DeleteAsync(
        CultureDeleteCommand command,
        CancellationToken cancel = default
    );
}
