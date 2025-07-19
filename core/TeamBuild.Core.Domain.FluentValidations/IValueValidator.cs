using FluentValidation;

namespace TeamBuild.Core.Domain.FluentValidations;

public interface IValueValidator<TValue>
{
    IRuleBuilder<T, TValue> IsValid<T>(IRuleBuilderInitial<T, TValue> builder);

    IRuleBuilder<T, TValue> IsValid<T>(IRuleBuilderInitialCollection<T, TValue> builder);

    IRuleBuilder<T, TValue> ValidationRules<T>(IRuleBuilder<T, TValue> builder);
}

public abstract class ValueValidator<TValue> : IValueValidator<TValue>
{
    public IRuleBuilder<T, TValue> IsValid<T>(IRuleBuilderInitial<T, TValue> builder)
    {
        return ValidationRules(builder.Cascade(CascadeMode.Stop));
    }

    public IRuleBuilder<T, TValue> IsValid<T>(IRuleBuilderInitialCollection<T, TValue> builder)
    {
        return ValidationRules(builder.Cascade(CascadeMode.Stop));
    }

    public abstract IRuleBuilder<T, TValue> ValidationRules<T>(IRuleBuilder<T, TValue> builder);
}

public static class ValueValidatorExtensions
{
    public static IRuleBuilder<T, TValue> ValidateWith<T, TValue>(
        this IRuleBuilderInitial<T, TValue> source,
        IValueValidator<TValue> validation
    ) => validation.IsValid(source);

    public static IRuleBuilder<T, TValue> ValidateWith<T, TValue>(
        this IRuleBuilderInitialCollection<T, TValue> source,
        IValueValidator<TValue> validation
    ) => validation.IsValid(source);

    public static IRuleBuilder<T, TValue> ValidateWith<T, TValue>(
        this IRuleBuilder<T, TValue> source,
        IValueValidator<TValue> validation
    ) => validation.ValidationRules(source);
}
