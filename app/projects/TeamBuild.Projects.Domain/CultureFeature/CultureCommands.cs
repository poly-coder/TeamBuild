using FluentValidation;
using TeamBuild.Core.Domain;
using TeamBuild.Core.Domain.FluentValidations;

namespace TeamBuild.Projects.Domain.CultureFeature;

public abstract record CultureCommand : IDomainCommand;

public record CultureCreateCommand(CultureEntity Culture) : CultureCommand;

public record CultureUpdateCommand(CultureEntity Culture) : CultureCommand;

public record CultureDeleteCommand(string CultureCode) : CultureCommand;

public class CultureCreateCommandValidator : AbstractValidator<CultureCreateCommand>
{
    public CultureCreateCommandValidator(IValidator<CultureEntity> modelValidator)
    {
        RuleFor(e => e.Culture).NotNull().SetValidator(modelValidator);
    }
}

public class CultureUpdateCommandValidator : AbstractValidator<CultureUpdateCommand>
{
    public CultureUpdateCommandValidator(IValidator<CultureEntity> modelValidator)
    {
        RuleFor(e => e.Culture).NotNull().SetValidator(modelValidator);
    }
}

public class CultureDeleteCommandValidator : AbstractValidator<CultureDeleteCommand>
{
    public CultureDeleteCommandValidator()
    {
        RuleFor(e => e.CultureCode).ValidateWith(CultureValidations.CultureCode);
    }
}
