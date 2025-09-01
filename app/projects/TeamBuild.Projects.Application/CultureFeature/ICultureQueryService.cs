using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Logging;
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
    CultureQueryServiceAspect aspect
) : ICultureQueryService, IDisposable
{
    public Task<CultureListQuerySuccess> List(
        CultureListQuery query,
        CancellationToken cancel = default
    ) =>
        aspect.ExecuteAsync(
            targetType: service.GetType(),
            parameters: [query],
            action: () => service.List(query, cancel),
            cancel: cancel
        );

    public async Task<CultureGetByIdQuerySuccess> GetById(
        CultureGetByIdQuery query,
        CancellationToken cancel = default
    ) =>
        await aspect.ExecuteAsync(
            targetType: service.GetType(),
            parameters: [query],
            action: () => service.GetById(query, cancel),
            cancel: cancel
        );

    public async Task<CultureGetByIdsQuerySuccess> GetByIds(
        CultureGetByIdsQuery query,
        CancellationToken cancel = default
    ) =>
        await aspect.ExecuteAsync(
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

public sealed class CultureQueryServiceAspect(
    ILoggerFactory loggerFactory,
    FluentValidationAspect validationAspect
)
    : StandardServiceAspect(
        new CultureQueryServiceTracingAspect(TeamBuildProjectsApplication.ActivitySource),
        new CultureQueryServiceLoggingAspect(loggerFactory),
        validationAspect,
        new CultureQueryServiceMetricsAspect()
    );

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
                CultureResource.DefinitionName,
                TeamBuildCoreDomain.OperationListName
            ),

            nameof(ICultureQueryService.GetById) => TeamBuildProjectsApplication.OperationTags(
                CultureResource.DefinitionName,
                TeamBuildCoreDomain.OperationGetByIdName
            ),

            nameof(ICultureQueryService.GetByIds) => TeamBuildProjectsApplication.OperationTags(
                CultureResource.DefinitionName,
                TeamBuildCoreDomain.OperationGetByIdsName
            ),

            _ => null,
        };
    }
}

public class CultureQueryServiceLoggingAspect(ILoggerFactory loggerFactory)
    : LoggingAspect(loggerFactory)
{
    protected override string GetObjectCaption(object? value)
    {
        return value switch
        {
            null => "null",

            CultureListQuery query => $"Query[q:'{query.Filter?.Search}']",

            CultureGetByIdQuery query => $"Query[id:{query.CultureCode}]",

            CultureGetByIdsQuery query => $"Query[ids:({query.CultureCodes.Count})]",

            CultureListQuerySuccess success => $"Success[count:{success.CultureList.Count}]",

            CultureGetByIdQuerySuccess success =>
                $"Success[Culture=Name:{success.Culture.EnglishName}]",

            CultureGetByIdsQuerySuccess success => $"Success[count:{success.CultureList.Count}]",

            _ => "***",
        };
    }
}

[ExcludeFromCodeCoverage]
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
        switch (result)
        {
            case CultureListQuerySuccess success:
                {
                    ListCounter.Add(
                        1,
                        KeyValuePair.Create("count", (object?)success.CultureList.Count)
                    );
                }
                break;

            case CultureGetByIdQuerySuccess:
                GetByIdCounter.Add(1);
                break;

            case CultureGetByIdsQuerySuccess success:
                {
                    GetByIdsCounter.Add(
                        1,
                        KeyValuePair.Create("count", (object?)success.CultureList.Count)
                    );
                }
                break;
        }
    }
}
