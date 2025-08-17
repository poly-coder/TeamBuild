using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace TeamBuild.Core.Application.Decorators;

public class FluentValidationAspect(IServiceProvider provider)
{
    public void Before(ReadOnlySpan<object?> inputs)
    {
        var errors = new List<ValidationResult>();

        foreach (var input in inputs)
        {
            if (input is null)
                continue;

            var inputType = input.GetType();

            var validatorType = ValidatorEnumerableTypeCache.GetValue(inputType);

            if (provider.GetRequiredService(validatorType) is not IEnumerable validators)
                continue;

            foreach (IValidator validator in validators)
            {
                var context = CreateContext(inputType, input);

                var validation = validator.Validate(context);

                if (validation is { IsValid: false })
                    errors.Add(validation);
            }
        }

        if (errors is not [])
            throw new ValidationException(errors.SelectMany(e => e.Errors));
    }

    public async Task BeforeAsync(IReadOnlyList<object?> inputs, CancellationToken cancel = default)
    {
        var validationTasks = new ConcurrentBag<Task<ValidationResult>>();

        foreach (var input in inputs)
        {
            if (input is null)
                continue;

            var inputType = input.GetType();

            var validatorType = ValidatorEnumerableTypeCache.GetValue(inputType);

            if (provider.GetRequiredService(validatorType) is not IEnumerable validators)
                continue;

            foreach (IValidator validator in validators)
            {
                var context = CreateContext(inputType, input);

                var validationTask = validator.ValidateAsync(context, cancel);

                validationTasks.Add(validationTask);
            }
        }

        var validations = await Task.WhenAll(validationTasks);

        var errors = validations.SelectMany(e => e.Errors).ToArray();

        if (errors is not [])
            throw new ValidationException(errors);
    }

    public TResult Execute<TResult>(ReadOnlySpan<object?> inputs, Func<TResult> action)
    {
        Before(inputs);

        return action();
    }

    public void Execute(ReadOnlySpan<object?> inputs, Action action)
    {
        Before(inputs);

        action();
    }

    public async Task<TResult> ExecuteAsync<TResult>(
        IReadOnlyList<object?> inputs,
        Func<Task<TResult>> action,
        CancellationToken cancel = default
    )
    {
        await BeforeAsync(inputs, cancel);

        return await action();
    }

    public async Task ExecuteAsync(
        IReadOnlyList<object?> inputs,
        Func<Task> action,
        CancellationToken cancel = default
    )
    {
        await BeforeAsync(inputs, cancel);

        await action();
    }

    private static readonly WeakCachedSyncFunction<Type, Type> ValidatorEnumerableTypeCache = new(
        inputType =>
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(inputType);
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(validatorType);
            return enumerableType;
        }
    );

    private static readonly WeakCachedSyncFunction<
        Type,
        ConstructorInfo
    > ValidationContextConstructorCache = new(inputType =>
        typeof(ValidationContext<>).MakeGenericType(inputType).GetConstructor([inputType])!
    );

    private static IValidationContext CreateContext(Type inputType, object input)
    {
        var constructor = ValidationContextConstructorCache.GetValue(inputType);

        return (IValidationContext)constructor.Invoke([input]);
    }
}
