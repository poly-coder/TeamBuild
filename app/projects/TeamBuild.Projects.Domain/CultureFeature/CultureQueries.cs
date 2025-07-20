using FluentValidation;
using TeamBuild.Core;
using TeamBuild.Core.Domain;
using TeamBuild.Core.Domain.FluentValidations;

namespace TeamBuild.Projects.Domain.CultureFeature;

public abstract record CultureQuery : IDomainQuery;

public abstract record CultureQuerySuccess : IDomainQuerySuccess;

public record CultureGetByIdQuery(string CultureCode) : CultureQuery;

public record CultureGetByIdQuerySuccess(CultureAggregate Culture) : CultureQuerySuccess;

public record CultureGetByIdsQuery(string[] CultureCodes) : CultureQuery;

public record CultureGetByIdsQuerySuccess(Option<CultureAggregate>[] CultureList)
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
