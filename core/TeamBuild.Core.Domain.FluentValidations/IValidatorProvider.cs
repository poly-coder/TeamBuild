using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace TeamBuild.Core.Domain.FluentValidations;

public interface IValidatorProvider
{
    bool TryGetValidator(Type instanceType, [NotNullWhen(true)] out IValidator? validator);
}
