using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TeamBuild.Core.Services;

public static class TeamBuildCoreServicesExtensions
{
    public const string TestingEnvVarName = "Testing";

    public static bool IsTesting(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>(TestingEnvVarName);
    }

    public static IServiceCollection AddOpenTelemetrySources(
        this IServiceCollection services,
        params string[] sourceNames
    )
    {
        services
            .AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                foreach (var name in sourceNames)
                {
                    metrics.AddMeter(name);
                }
            })
            .WithTracing(tracing =>
            {
                foreach (var name in sourceNames)
                {
                    tracing.AddSource(name);
                }
            });

        return services;
    }
}
