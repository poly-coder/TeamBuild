using FluentValidation;
using TeamBuild.Core.Domain.FluentValidations;

namespace TeamBuild.Projects.Domain.CultureFeature;

public record CultureEntity(string CultureCode, string EnglishName, string NativeName);

public class CultureEntityValidator : AbstractValidator<CultureEntity>
{
    public CultureEntityValidator()
    {
        RuleFor(e => e.CultureCode).ValidateWith(CultureValidations.CultureCode);
        RuleFor(e => e.EnglishName).ValidateWith(CultureValidations.EnglishName);
        RuleFor(e => e.NativeName).ValidateWith(CultureValidations.NativeName);
    }
}
