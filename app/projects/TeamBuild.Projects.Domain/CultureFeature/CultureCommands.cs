using FluentValidation;
using TeamBuild.Core;
using TeamBuild.Core.Domain;
using TeamBuild.Core.Domain.FluentValidations;

namespace TeamBuild.Projects.Domain.CultureFeature;

public abstract record CultureCommand : IDomainCommand;

public record CultureDefineCommand(string CultureCode, string EnglishName, string NativeName)
    : CultureCommand
{
    public CultureDefineCommand Coerce() =>
        new(CultureCode.CoerceTrim(), EnglishName.CoerceTrim(), NativeName.CoerceTrim());
}

public record CultureDefineCommandSuccess(CultureDetails Culture) : IDomainCommandSuccess;

public record CultureDeleteCommand(string CultureCode) : CultureCommand
{
    public CultureDeleteCommand Coerce() => new(CultureCode.CoerceTrim());
}

public record CultureDeleteCommandSuccess : IDomainCommandSuccess;

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
