﻿using Microsoft.Extensions.DependencyInjection;
using TeamBuild.Projects.Application.CultureFeature;

namespace TeamBuild.Projects.Infrastructure.CultureFeature;

public static class CultureFeatureExtensions
{
    public static IServiceCollection AddCultureInfrastructureServices(
        this IServiceCollection services
    )
    {
        return services.AddScoped<ICultureAggregateService, CultureAggregateMartenService>();
    }
}
