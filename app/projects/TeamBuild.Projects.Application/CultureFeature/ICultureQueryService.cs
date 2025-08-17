using System.Diagnostics;
using System.Diagnostics.Metrics;
using TeamBuild.Core;
using TeamBuild.Core.Application.Decorators;
using TeamBuild.Core.Domain;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Application.CultureFeature;

public interface ICultureQueryService
{
    Task<CultureListQuerySuccess> List(CultureListQuery query, CancellationToken cancel = default);

    Task<CultureGetByIdQuerySuccess> GetById(
        CultureGetByIdQuery query,
        CancellationToken cancel = default
    );

    Task<CultureGetByIdsQuerySuccess> GetByIds(
        CultureGetByIdsQuery query,
        CancellationToken cancel = default
    );
}

public sealed class CultureQueryServiceDecorator(
    ICultureQueryService service,
    CultureQueryServiceTracingAspect tracingAspect,
    LoggingAspect loggingAspect,
    FluentValidationAspect validationAspect,
    CultureQueryServiceMetricsAspect metricsAspect
)
    : StandardServiceDecorator(tracingAspect, loggingAspect, validationAspect, metricsAspect),
        ICultureQueryService,
        IDisposable
{
    public Task<CultureListQuerySuccess> List(
        CultureListQuery query,
        CancellationToken cancel = default
    ) =>
        ExecuteAsync(
            targetType: service.GetType(),
            parameters: [query],
            action: () => service.List(query, cancel),
            cancel: cancel
        );

    public Task<CultureGetByIdQuerySuccess> GetById(
        CultureGetByIdQuery query,
        CancellationToken cancel = default
    ) =>
        ExecuteAsync(
            targetType: service.GetType(),
            parameters: [query],
            action: () => service.GetById(query, cancel),
            cancel: cancel
        );

    public Task<CultureGetByIdsQuerySuccess> GetByIds(
        CultureGetByIdsQuery query,
        CancellationToken cancel = default
    ) =>
        ExecuteAsync(
            targetType: service.GetType(),
            parameters: [query],
            action: () => service.GetByIds(query, cancel),
            cancel: cancel
        );

    public void Dispose()
    {
        service.DisposeIfNeeded();
    }
}

public class CultureQueryServiceTracingAspect(ActivitySource activitySource)
    : TracingAspect(activitySource)
{
    protected override IEnumerable<KeyValuePair<string, object?>>? CreateTags(
        Type targetType,
        string methodName
    )
    {
        return methodName switch
        {
            nameof(ICultureQueryService.List) => TeamBuildProjectsApplication.OperationTags(
                CultureEntity.Caption,
                TeamBuildCoreDomain.OperationListName
            ),

            nameof(ICultureQueryService.GetById) => TeamBuildProjectsApplication.OperationTags(
                CultureEntity.Caption,
                TeamBuildCoreDomain.OperationFetchName
            ),

            nameof(ICultureQueryService.GetByIds) => TeamBuildProjectsApplication.OperationTags(
                CultureEntity.Caption,
                TeamBuildCoreDomain.OperationFetchName
            ),

            _ => null,
        };
    }
}

public class CultureQueryServiceMetricsAspect : MetricsAspect
{
    private static readonly Counter<long> ListCounter =
        TeamBuildProjectsApplication.Meter.CreateCounter<long>(
            name: "culture-list",
            description: "Culture list queries"
        );

    private static readonly Counter<long> GetByIdCounter =
        TeamBuildProjectsApplication.Meter.CreateCounter<long>(
            name: "culture-get-one",
            description: "Culture get by id queries"
        );

    private static readonly Counter<long> GetByIdsCounter =
        TeamBuildProjectsApplication.Meter.CreateCounter<long>(
            name: "culture-get-many",
            description: "Culture get by ids queries"
        );

    protected override void Count(
        string methodName,
        IReadOnlyList<object?> parameters,
        object? result
    )
    {
        switch (methodName)
        {
            case nameof(ICultureQueryService.List):
                {
                    var success = (CultureListQuerySuccess)result!;
                    ListCounter.Add(
                        1,
                        KeyValuePair.Create("count", (object?)success.CultureList.Count)
                    );
                }
                break;

            case nameof(ICultureQueryService.GetById):
                GetByIdCounter.Add(1);
                break;

            case nameof(ICultureQueryService.GetByIds):
                {
                    var success = (CultureGetByIdsQuerySuccess)result!;
                    GetByIdsCounter.Add(
                        1,
                        KeyValuePair.Create("count", (object?)success.CultureList.Count)
                    );
                }
                break;
        }
    }
}
