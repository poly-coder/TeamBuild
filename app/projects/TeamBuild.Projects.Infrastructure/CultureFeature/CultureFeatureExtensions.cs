using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace TeamBuild.Projects.Infrastructure.CultureFeature;

public static class CultureFeatureExtensions
{
    public static IServiceCollection AddCultureInfrastructureServices(
        this IServiceCollection services
    )
    {
        return services
            .ConfigureMarten(options =>
            {
                options.Schema.For<CultureDocument>();
            })
            .AddAvailableCultureQueryService()
            .AddCultureMartenCommandService()
            .AddCultureMartenQueryService();
    }
}
