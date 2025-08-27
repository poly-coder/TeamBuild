using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;
using FluentValidation;
using TeamBuild.Core;
using TeamBuild.Core.Domain;
using TeamBuild.Core.Domain.FluentValidations;
using TeamBuild.Projects.Domain.ModuleFeature;

namespace TeamBuild.Projects.Domain.CultureFeature;

[JsonDerivedType(typeof(CultureDefineCommand), "define")]
[JsonDerivedType(typeof(CultureDeleteCommand), "delete")]
public abstract record CultureCommand : IDomainCommand;

[JsonDerivedType(typeof(CultureDefineCommandSuccess), "define")]
[JsonDerivedType(typeof(CultureDeleteCommandSuccess), "delete")]
public abstract record CultureCommandSuccess : IDomainCommand;

// Define

// [OperationDefinition(typeof(CultureResource), TeamBuildCoreDomain.OperationDefineName)]
public record CultureDefineCommand(string CultureCode, string EnglishName, string NativeName)
    : CultureCommand
{
    public const string DefinitionName = TeamBuildCoreDomain.OperationDefineName;

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

    public CultureDefineCommand Coerce() =>
        new(CultureCode.CoerceTrim(), EnglishName.CoerceTrim(), NativeName.CoerceTrim());
}

public record CultureDefineCommandSuccess(CultureDetails Culture) : CultureCommandSuccess;

// Delete

public record CultureDeleteCommand(string CultureCode) : CultureCommand
{
    public const string DefinitionName = TeamBuildCoreDomain.OperationDeleteName;

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

    public CultureDeleteCommand Coerce() => new(CultureCode.CoerceTrim());
}

public record CultureDeleteCommandSuccess : CultureCommandSuccess;

public class CultureDefineCommandValidator : AbstractValidator<CultureDefineCommand>
{
    public CultureDefineCommandValidator()
    {
        RuleFor(e => e.CultureCode).ValidateWith(CultureValidations.CultureCode);
        RuleFor(e => e.EnglishName).ValidateWith(CultureValidations.EnglishName);
        RuleFor(e => e.NativeName).ValidateWith(CultureValidations.NativeName);
    }
}

public class CultureDeleteCommandValidator : AbstractValidator<CultureDeleteCommand>
{
    public CultureDeleteCommandValidator()
    {
        RuleFor(e => e.CultureCode).ValidateWith(CultureValidations.CultureCode);
    }
}
