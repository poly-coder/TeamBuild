﻿using FluentValidation;
using TeamBuild.Core.Domain;
using TeamBuild.Core.Domain.FluentValidations;

namespace TeamBuild.Projects.Domain.CultureFeature;

public abstract record CultureCommand : IDomainCommand;

public record CultureCreateCommand(string CultureCode, string EnglishName, string NativeName)
    : CultureCommand;

public record CultureUpdateCommand(string CultureCode, string EnglishName, string NativeName)
    : CultureCommand;

public record CultureDeleteCommand(string CultureCode) : CultureCommand;

public class CultureCreateCommandValidator : AbstractValidator<CultureCreateCommand>
{
    public CultureCreateCommandValidator()
    {
        RuleFor(e => e.CultureCode).ValidateWith(CultureValidations.CultureCode);
        RuleFor(e => e.EnglishName).ValidateWith(CultureValidations.EnglishName);
        RuleFor(e => e.NativeName).ValidateWith(CultureValidations.NativeName);
    }
}

public class CultureUpdateCommandValidator : AbstractValidator<CultureUpdateCommand>
{
    public CultureUpdateCommandValidator()
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
