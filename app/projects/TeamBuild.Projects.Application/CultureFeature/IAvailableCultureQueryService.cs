using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Logging;
using TeamBuild.Core;
using TeamBuild.Core.Application.Decorators;
using TeamBuild.Core.Domain;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Application.CultureFeature;

public interface IAvailableCultureQueryService
{
    Task<AvailableCultureListQuerySuccess> List(
        AvailableCultureListQuery query,
        CancellationToken cancel = default
    );
}

public sealed class AvailableCultureQueryServiceDecorator(
    IAvailableCultureQueryService service,
    AvailableCultureQueryServiceTracingAspect tracingAspect,
    AvailableCultureQueryServiceLoggingAspect loggingAspect,
    FluentValidationAspect validationAspect,
    AvailableCultureQueryServiceMetricsAspect metricsAspect
)
    : StandardServiceDecorator(tracingAspect, loggingAspect, validationAspect, metricsAspect),
        IAvailableCultureQueryService,
        IDisposable
{
    public Task<AvailableCultureListQuerySuccess> List(
        AvailableCultureListQuery query,
        CancellationToken cancel = default
    ) =>
        ExecuteAsync(
            targetType: service.GetType(),
            parameters: [query],
            action: () => service.List(query, cancel),
            cancel: cancel
        );

    public void Dispose()
    {
        service.DisposeIfNeeded();
    }
}

public class AvailableCultureQueryServiceTracingAspect(ActivitySource activitySource)
    : TracingAspect(activitySource)
{
    protected override IEnumerable<KeyValuePair<string, object?>>? CreateTags(
        Type targetType,
        string methodName
    )
    {
        return methodName switch
        {
            nameof(IAvailableCultureQueryService.List) =>
                TeamBuildProjectsApplication.OperationTags(
                    AvailableCultureResource.DefinitionName,
                    TeamBuildCoreDomain.OperationListName
                ),

            _ => null,
        };
    }
}

public class AvailableCultureQueryServiceLoggingAspect(ILoggerFactory loggerFactory)
    : LoggingAspect(loggerFactory)
{
    protected override string GetObjectCaption(object? value)
    {
        return value switch
        {
            null => "null",

            AvailableCultureListQuery query => $"Query[q:'{query.Filter?.Search}']",

            AvailableCultureListQuerySuccess success =>
                $"Success[count:{success.CultureList.Count}]",

            _ => "***",
        };
    }
}

public class AvailableCultureQueryServiceMetricsAspect : MetricsAspect
{
    private static readonly Counter<long> ListCounter =
        TeamBuildProjectsApplication.Meter.CreateCounter<long>(
            name: "culture-list",
            description: "Culture list queries"
        );

    protected override void Count(
        string methodName,
        IReadOnlyList<object?> parameters,
        object? result
    )
    {
        switch (result)
        {
            case AvailableCultureListQuerySuccess success:
                {
                    ListCounter.Add(
                        1,
                        KeyValuePair.Create("count", (object?)success.CultureList.Count)
                    );
                }
                break;
        }
    }
}
