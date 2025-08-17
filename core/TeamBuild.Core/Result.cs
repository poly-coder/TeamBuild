using System.Runtime.CompilerServices;

namespace TeamBuild.Core;

public abstract record Result<TValue, TFailure>
{
    private Result() { }

    public abstract bool IsOk { get; }

    public bool IsFailure => !IsOk;

    public abstract TResult Match<TResult>(
        Func<TValue, TResult> onOk,
        Func<TFailure, TResult> onFailure
    );

    public static implicit operator Result<TValue, TFailure>(TValue value) =>
        Result.Ok<TValue, TFailure>(value);

    public static implicit operator Result<TValue, TFailure>(TFailure failure) =>
        Result.Failure<TValue, TFailure>(failure);

    public sealed record Failure(TFailure Err) : Result<TValue, TFailure>
    {
        public override bool IsOk => false;

        public override TResult Match<TResult>(
            Func<TValue, TResult> onOk,
            Func<TFailure, TResult> onFailure
        )
        {
            return onFailure(Err);
        }
    }

    public sealed record Ok(TValue Value) : Result<TValue, TFailure>
    {
        public override bool IsOk => true;

        public override TResult Match<TResult>(
            Func<TValue, TResult> onOk,
            Func<TFailure, TResult> onFailure
        )
        {
            return onOk(Value);
        }
    }
}

public static class Result
{
    public static Result<TValue, TFailure> Failure<TValue, TFailure>(TFailure failure) =>
        new Result<TValue, TFailure>.Failure(failure);

    public static Result<TValue, TFailure> Ok<TValue, TFailure>(TValue value) =>
        new Result<TValue, TFailure>.Ok(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOutput, TFailure> Bind<TInput, TOutput, TFailure>(
        this Result<TInput, TFailure> option,
        Func<TInput, Result<TOutput, TFailure>> binder
    ) => option.Match(onOk: binder, onFailure: Failure<TOutput, TFailure>);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOutput, TFailure> Map<TInput, TOutput, TFailure>(
        this Result<TInput, TFailure> option,
        Func<TInput, TOutput> mapper
    ) => option.Match(onOk: value => mapper(value), onFailure: Failure<TOutput, TFailure>);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue, TOutputFailure> BindFailure<TValue, TInputFailure, TOutputFailure>(
        this Result<TValue, TInputFailure> result,
        Func<TInputFailure, Result<TValue, TOutputFailure>> binder
    ) => result.Match(onOk: Ok<TValue, TOutputFailure>, onFailure: binder);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue, TOutputFailure> MapFailure<TValue, TInputFailure, TOutputFailure>(
        this Result<TValue, TInputFailure> result,
        Func<TInputFailure, TOutputFailure> mapper
    ) =>
        result.Match(
            onOk: Ok<TValue, TOutputFailure>,
            onFailure: err => Failure<TValue, TOutputFailure>(mapper(err))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlyList<TValue> ToEnumerable<TValue, TFailure>(
        this Result<TValue, TFailure> option
    )
    {
        return option.Match<IReadOnlyList<TValue>>(onOk: value => [value], onFailure: _ => []);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlyList<TFailure> ToEnumerableFailure<TValue, TFailure>(
        this Result<TValue, TFailure> option
    )
    {
        return option.Match<IReadOnlyList<TFailure>>(onOk: _ => [], onFailure: err => [err]);
    }

    public static Result<T, Exception> OfFunc<T>(Func<T> func)
    {
        try
        {
            return func();
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public static bool TryGetValue<TValue, TFailure>(
        this Result<TValue, TFailure> result,
        out TValue value
    )
    {
        switch (result)
        {
            case Result<TValue, TFailure>.Ok ok:
                value = ok.Value;
                return true;

            case Result<TValue, TFailure>.Failure:
                value = default!;
                return false;

            default:
                throw new InvalidOperationException("Unknown result type");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TValue GetValueOrThrow<TValue, TFailure>(this Result<TValue, TFailure> option)
    {
        return option.Match(
            onOk: x => x,
            onFailure: _ => throw new InvalidOperationException("Result is in failure state")
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TValue GetValueOrDefault<TValue, TFailure>(
        this Result<TValue, TFailure> option,
        TValue defaultValue
    )
    {
        return option.Match(onOk: x => x, onFailure: _ => defaultValue);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TValue GetValueOrDefault<TValue, TFailure>(
        this Result<TValue, TFailure> option,
        Func<TValue> defaultValueFactory
    )
    {
        return option.Match(onOk: x => x, onFailure: _ => defaultValueFactory());
    }

    public static bool TryGetFailure<TValue, TFailure>(
        this Result<TValue, TFailure> result,
        out TFailure failure
    )
    {
        switch (result)
        {
            case Result<TValue, TFailure>.Ok:
                failure = default!;
                return false;

            case Result<TValue, TFailure>.Failure err:
                failure = err.Err;
                return true;

            default:
                throw new InvalidOperationException("Unknown result type");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TFailure GetFailureOrThrow<TValue, TFailure>(this Result<TValue, TFailure> option)
    {
        return option.Match(
            onOk: _ => throw new InvalidOperationException("Result is in Ok state"),
            onFailure: x => x
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TFailure GetFailureOrDefault<TValue, TFailure>(
        this Result<TValue, TFailure> option,
        TFailure defaultFailure
    )
    {
        return option.Match(onOk: _ => defaultFailure, onFailure: x => x);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TFailure GetFailureOrDefault<TValue, TFailure>(
        this Result<TValue, TFailure> option,
        Func<TFailure> defaultFailureFactory
    )
    {
        return option.Match(onOk: _ => defaultFailureFactory(), onFailure: x => x);
    }
}
