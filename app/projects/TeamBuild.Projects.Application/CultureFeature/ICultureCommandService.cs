using System.Diagnostics;
using System.Diagnostics.Metrics;
using TeamBuild.Core;
using TeamBuild.Core.Application.Decorators;
using TeamBuild.Core.Domain;
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

public sealed class CultureCommandServiceDecorator(
    ICultureCommandService service,
    CultureCommandServiceTracingAspect tracingAspect,
    LoggingAspect loggingAspect,
    FluentValidationAspect validationAspect,
    CultureCommandServiceMetricsAspect metricsAspect
)
    : StandardServiceDecorator(tracingAspect, loggingAspect, validationAspect, metricsAspect),
        ICultureCommandService,
        IDisposable
{
    public Task<CultureDefineCommandSuccess> Define(
        CultureDefineCommand command,
        CancellationToken cancel = default
    ) =>
        ExecuteAsync(
            targetType: service.GetType(),
            parameters: [command],
            action: () => service.Define(command, cancel),
            cancel: cancel
        );

    public Task<CultureDeleteCommandSuccess> Delete(
        CultureDeleteCommand command,
        CancellationToken cancel = default
    ) =>
        ExecuteAsync(
            targetType: service.GetType(),
            parameters: [command],
            action: () => service.Delete(command, cancel),
            cancel: cancel
        );

    public void Dispose()
    {
        service.DisposeIfNeeded();
    }
}

public class CultureCommandServiceTracingAspect(ActivitySource activitySource)
    : TracingAspect(activitySource)
{
    protected override IEnumerable<KeyValuePair<string, object?>>? CreateTags(
        Type targetType,
        string methodName
    )
    {
        return methodName switch
        {
            nameof(ICultureCommandService.Define) => TeamBuildProjectsApplication.OperationTags(
                CultureEntity.Caption,
                TeamBuildCoreDomain.OperationCreateName
            ),

            nameof(ICultureCommandService.Delete) => TeamBuildProjectsApplication.OperationTags(
                CultureEntity.Caption,
                TeamBuildCoreDomain.OperationDeleteName
            ),

            _ => null,
        };
    }
}

public class CultureCommandServiceMetricsAspect : MetricsAspect
{
    private static readonly Counter<long> DefineCounter =
        TeamBuildProjectsApplication.Meter.CreateCounter<long>(
            name: "culture-define",
            description: "Culture define commands"
        );

    private static readonly Counter<long> DeleteCounter =
        TeamBuildProjectsApplication.Meter.CreateCounter<long>(
            name: "culture-delete",
            description: "Culture delete commands"
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
