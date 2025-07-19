using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace TeamBuild.Core;

public abstract record Option<T>
{
    private Option() { }

    public abstract bool IsSome { get; }
    public bool IsNone => !IsSome;

    public abstract TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none);

    public static implicit operator Option<T>(T value) => Option.Some(value);

    public sealed record None : Option<T>
    {
        public static readonly None Instance = new();

        public override bool IsSome => false;

        public override TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            return none();
        }
    }

    public sealed record Some(T Value) : Option<T>
    {
        public override bool IsSome => true;

        public override TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            return some(Value);
        }
    }
}

public static class Option
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> None<T>() => Option<T>.None.Instance;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Some<T>(T value) => new Option<T>.Some(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TOutput> Bind<TInput, TOutput>(
        this Option<TInput> option,
        Func<TInput, Option<TOutput>> binder
    ) => option.Match(some: binder, none: None<TOutput>);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TOutput> Map<TInput, TOutput>(
        this Option<TInput> option,
        Func<TInput, TOutput> mapper
    ) => option.Match(some: value => mapper(value), none: None<TOutput>);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> OfObj<T>(T value) => value is null ? None<T>() : Some(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? ToObj<T>(this Option<T> option)
        where T : class
    {
        return option.Match(some: value => value, none: () => (T?)null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlyList<T> ToEnumerable<T>(this Option<T> option)
    {
        return option.Match<IReadOnlyList<T>>(some: value => [value], none: () => []);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> OfNullable<T>(T? value)
        where T : struct
    {
        return value.HasValue ? Some(value.Value) : None<T>();
    }

    public static Option<T> OfFunc<T>(Func<T> func)
    {
        try
        {
            return func();
        }
        catch
        {
            return None<T>();
        }
    }

    public static bool TryGetValue<T>(this Option<T> option, out T value)
    {
        switch (option)
        {
            case Option<T>.Some someOption:
                value = someOption.Value;
                return true;

            case Option<T>.None:
                value = default!;
                return false;

            default:
                throw new InvalidOperationException("Unknown option type");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetValue<T>(this Option<T> option)
    {
        return option.Match(
            some: x => x,
            none: () => throw new InvalidOperationException("Option has no value")
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetValueOrDefault<T>(this Option<T> option, T defaultValue)
    {
        return option.Match(some: x => x, none: () => defaultValue);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetValueOrDefault<T>(this Option<T> option, Func<T> defaultValueFactory)
    {
        return option.Match(some: x => x, none: defaultValueFactory);
    }
}
