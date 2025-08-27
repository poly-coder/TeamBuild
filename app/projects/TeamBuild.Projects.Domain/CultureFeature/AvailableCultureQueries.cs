using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;
using TeamBuild.Core.Domain;
using TeamBuild.Projects.Domain.ModuleFeature;

namespace TeamBuild.Projects.Domain.CultureFeature;

[JsonDerivedType(typeof(AvailableCultureListQuery), "list")]
public abstract record AvailableCultureQuery : IDomainQuery;

[JsonDerivedType(typeof(AvailableCultureListQuerySuccess), "list")]
public abstract record AvailableCultureQuerySuccess : IDomainQuerySuccess;

public record AvailableCultureListQuery(AvailableCultureListQueryFilter? Filter = null)
    : AvailableCultureQuery
{
    public const string DefinitionName = TeamBuildCoreDomain.OperationListName;

    public static readonly DefinitionKey DefinitionKey =
        AvailableCultureResource.DefinitionKey.Operation(DefinitionName);

    public static readonly Counter<long> Counter =
        TeamBuildProjectsDomain.Meter.CreateCounter<long>(DefinitionKey.FullName);

    private static readonly Lazy<OperationDefinition> LazyDefinition = new(CreateDefinition);
    public static OperationDefinition Definition => LazyDefinition.Value;

    private static OperationDefinition CreateDefinition()
    {
        return new OperationDefinition
        {
            Name = DefinitionName,
            Id = DefinitionKey.Id,
            FullName = DefinitionKey.FullName,
        };
    }
}

public record AvailableCultureListQueryFilter(string? Search = null) : AvailableCultureQuery;

public record AvailableCultureListQuerySuccess(IReadOnlyList<CultureDetails> CultureList)
    : AvailableCultureQuerySuccess;
