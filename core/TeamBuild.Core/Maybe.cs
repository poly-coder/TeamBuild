using System.Runtime.CompilerServices;

namespace TeamBuild.Core;

public abstract record Maybe<T>
{
    private Maybe() { }

    public abstract bool IsSome { get; }
    public bool IsNone => !IsSome;

    public abstract TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none);

    public static implicit operator Maybe<T>(T value) => Maybe.Some(value);

    public sealed record None : Maybe<T>
    {
        public static readonly None Instance = new();

        public override bool IsSome => false;

        public override TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            return none();
        }
    }

    public sealed record Some(T Value) : Maybe<T>
    {
        public override bool IsSome => true;

        public override TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            return some(Value);
        }
    }
}

public static class Maybe
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<T> None<T>() => Maybe<T>.None.Instance;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<T> Some<T>(T value) => new Maybe<T>.Some(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TOutput> Bind<TInput, TOutput>(
        this Maybe<TInput> optional,
        Func<TInput, Maybe<TOutput>> binder
    ) => optional.Match(some: binder, none: None<TOutput>);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TOutput> Map<TInput, TOutput>(
        this Maybe<TInput> optional,
        Func<TInput, TOutput> mapper
    ) => optional.Match(some: value => mapper(value), none: None<TOutput>);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<T> OfObj<T>(T? value)
        where T : class => value is null ? None<T>() : Some(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? ToObj<T>(this Maybe<T> optional)
        where T : class
    {
        return optional.Match(some: value => value, none: () => (T?)null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlyList<T> ToEnumerable<T>(this Maybe<T> optional)
    {
        return optional.Match<IReadOnlyList<T>>(some: value => [value], none: () => []);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<T> OfNullable<T>(T? value)
        where T : struct
    {
        return value.HasValue ? Some(value.Value) : None<T>();
    }

    public static Maybe<T> OfFunc<T>(Func<T> func)
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

    public static bool TryGetValue<T>(this Maybe<T> optional, out T value)
    {
        switch (optional)
        {
            case Maybe<T>.Some someOption:
                value = someOption.Value;
                return true;

            case Maybe<T>.None:
                value = default!;
                return false;

            default:
                throw new InvalidOperationException("Unknown option type");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetValueOrThrow<T>(this Maybe<T> optional)
    {
        return optional.Match(
            some: x => x,
            none: () => throw new InvalidOperationException("Option has no value")
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetValueOrDefault<T>(this Maybe<T> optional, T defaultValue)
    {
        return optional.Match(some: x => x, none: () => defaultValue);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetValueOrDefault<T>(this Maybe<T> optional, Func<T> defaultValueFactory)
    {
        return optional.Match(some: x => x, none: defaultValueFactory);
    }
}
