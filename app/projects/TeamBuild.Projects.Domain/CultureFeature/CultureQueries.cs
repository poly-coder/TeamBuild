using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;
using FluentValidation;
using TeamBuild.Core.Domain;
using TeamBuild.Core.Domain.FluentValidations;
using TeamBuild.Projects.Domain.ModuleFeature;

namespace TeamBuild.Projects.Domain.CultureFeature;

[JsonDerivedType(typeof(CultureListQuery), "list")]
[JsonDerivedType(typeof(CultureGetByIdQuery), "get-one")]
[JsonDerivedType(typeof(CultureGetByIdsQuery), "get-many")]
public abstract record CultureQuery : IDomainQuery;

public abstract record CultureQuerySuccess : IDomainQuerySuccess;

// List

public record CultureListQuery(CultureListQueryFilter? Filter = null) : CultureQuery
{
    public const string DefinitionName = TeamBuildCoreDomain.OperationListName;

    public static readonly DefinitionKey DefinitionKey = CultureResource.DefinitionKey.Operation(
        DefinitionName
    );

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

public record CultureListQueryFilter(string? Search = null) : CultureQuery;

public record CultureListQuerySuccess(IReadOnlyList<CultureDetails> CultureList)
    : CultureQuerySuccess;

// Get by id

public record CultureGetByIdQuery(string CultureCode) : CultureQuery
{
    public const string DefinitionName = TeamBuildCoreDomain.OperationGetByIdName;

    public static readonly DefinitionKey DefinitionKey = CultureResource.DefinitionKey.Operation(
        DefinitionName
    );

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

public record CultureGetByIdQuerySuccess(CultureDetails Culture) : CultureQuerySuccess;

// Get by ids

public record CultureGetByIdsQuery(IReadOnlyList<string> CultureCodes) : CultureQuery
{
    public const string DefinitionName = TeamBuildCoreDomain.OperationGetByIdsName;

    public static readonly DefinitionKey DefinitionKey = CultureResource.DefinitionKey.Operation(
        DefinitionName
    );

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

public record CultureGetByIdsQuerySuccess(IReadOnlyList<CultureDetails> CultureList)
    : CultureQuerySuccess;

public class CultureCreateQueryValidator : AbstractValidator<CultureGetByIdQuery>
{
    public CultureCreateQueryValidator()
    {
        RuleFor(e => e.CultureCode).ValidateWith(CultureValidations.CultureCode);
    }
}

public class CultureUpdateQueryValidator : AbstractValidator<CultureGetByIdsQuery>
{
    public CultureUpdateQueryValidator()
    {
        RuleFor(e => e.CultureCodes).NotNull();
        RuleForEach(e => e.CultureCodes).ValidateWith(CultureValidations.CultureCode);
    }
}
