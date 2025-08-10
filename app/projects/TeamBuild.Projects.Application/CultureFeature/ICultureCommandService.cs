using System.Diagnostics.Metrics;
using TeamBuild.Core;
using TeamBuild.Core.Application.Decorators;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Application.CultureFeature;

public interface ICultureCommandService
{
    Task<CultureDefineCommandSuccess> Define(
        CultureDefineCommand command,
        CancellationToken cancel = default
    );

    Task<CultureDeleteCommandSuccess> Delete(
        CultureDeleteCommand command,
        CancellationToken cancel = default
    );
}

public class CultureCommandServiceDecorator(
    ICultureCommandService service,
    TracingAspect tracingAspect,
    LoggingAspect loggingAspect,
    FluentValidationAspect validationAspect,
    CultureCommandServiceMetricsAspect metricsAspect
) : ICultureCommandService, IDisposable
{
    public Task<CultureDefineCommandSuccess> Define(
        CultureDefineCommand command,
        CancellationToken cancel = default
    )
    {
        var targetType = service.GetType();
        IReadOnlyList<object?> parameters = [command];

        return tracingAspect.ExecuteAsync(
            targetType,
            () =>
                loggingAspect.ExecuteAsync(
                    targetType,
                    2,
                    () =>
                        validationAspect.ExecuteAsync(
                            parameters,
                            () =>
                                metricsAspect.ExecuteAsync(
                                    parameters,
                                    () => service.Define(command, cancel)
                                ),
                            cancel
                        )
                )
        );
    }

    public Task<CultureDeleteCommandSuccess> Delete(
        CultureDeleteCommand command,
        CancellationToken cancel = default
    ) =>
        tracingAspect.ExecuteAsync(
            service.GetType(),
            () =>
                loggingAspect.ExecuteAsync(
                    service.GetType(),
                    2,
                    () =>
                        validationAspect.ExecuteAsync(
                            [command],
                            () =>
                                metricsAspect.ExecuteAsync(
                                    [command],
                                    () => service.Delete(command, cancel)
                                ),
                            cancel
                        )
                )
        );

    public void Dispose()
    {
        service.DisposeIfNeeded();
    }
}

public class CultureCommandServiceMetricsAspect : MetricsAspect
{
    private static readonly Counter<long> DefineCounter =
        TeamBuildProjectsApplication.Meter.CreateCounter<long>(
            name: "culture-define",
            description: "Culture define operations"
        );

    private static readonly Counter<long> DeleteCounter =
        TeamBuildProjectsApplication.Meter.CreateCounter<long>(
            name: "culture-delete",
            description: "Culture delete operations"
        );

    protected override void Count(
        string methodName,
        IReadOnlyList<object?> parameters,
        object? result
    )
    {
        switch (methodName)
        {
            case nameof(ICultureCommandService.Define):
                DefineCounter.Add(1);
                break;

            case nameof(ICultureCommandService.Delete):
                DeleteCounter.Add(1);
                break;
        }
    }
}
