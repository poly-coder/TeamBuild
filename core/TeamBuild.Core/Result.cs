using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace TeamBuild.Core;

public abstract record Result<TValue, TError>
{
    private Result() { }

    public abstract bool IsOk { get; }

    public bool IsError => !IsOk;

    public abstract TResult Match<TResult>(Func<TValue, TResult> ok, Func<TError, TResult> error);

    public static implicit operator Result<TValue, TError>(TValue value) =>
        Result.Ok<TValue, TError>(value);

    public static implicit operator Result<TValue, TError>(TError error) =>
        Result.Error<TValue, TError>(error);

    public sealed record Error(TError Err) : Result<TValue, TError>
    {
        public override bool IsOk => false;

        public override TResult Match<TResult>(
            Func<TValue, TResult> ok,
            Func<TError, TResult> error
        )
        {
            return error(Err);
        }
    }

    public sealed record Ok(TValue Value) : Result<TValue, TError>
    {
        public override bool IsOk => true;

        public override TResult Match<TResult>(
            Func<TValue, TResult> ok,
            Func<TError, TResult> error
        )
        {
            return ok(Value);
        }
    }
}

public static class Result
{
    public static Result<TValue, TError> Error<TValue, TError>(TError error) =>
        new Result<TValue, TError>.Error(error);

    public static Result<TValue, TError> Ok<TValue, TError>(TValue value) =>
        new Result<TValue, TError>.Ok(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOutput, TError> Bind<TInput, TOutput, TError>(
        this Result<TInput, TError> option,
        Func<TInput, Result<TOutput, TError>> binder
    ) => option.Match(ok: binder, error: Error<TOutput, TError>);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOutput, TError> Map<TInput, TOutput, TError>(
        this Result<TInput, TError> option,
        Func<TInput, TOutput> mapper
    ) => option.Match(ok: value => mapper(value), error: Error<TOutput, TError>);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue, TOutputError> BindError<TValue, TInputError, TOutputError>(
        this Result<TValue, TInputError> result,
        Func<TInputError, Result<TValue, TOutputError>> binder
    ) => result.Match(ok: Ok<TValue, TOutputError>, error: binder);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue, TOutputError> MapError<TValue, TInputError, TOutputError>(
        this Result<TValue, TInputError> result,
        Func<TInputError, TOutputError> mapper
    ) =>
        result.Match(
            ok: Ok<TValue, TOutputError>,
            error: err => Error<TValue, TOutputError>(mapper(err))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlyList<TValue> ToEnumerable<TValue, TError>(
        this Result<TValue, TError> option
    )
    {
        return option.Match<IReadOnlyList<TValue>>(ok: value => [value], error: _ => []);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlyList<TError> ToEnumerableError<TValue, TError>(
        this Result<TValue, TError> option
    )
    {
        return option.Match<IReadOnlyList<TError>>(ok: _ => [], error: err => [err]);
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

    public static bool TryGetValue<TValue, TError>(
        this Result<TValue, TError> result,
        out TValue value
    )
    {
        switch (result)
        {
            case Result<TValue, TError>.Ok ok:
                value = ok.Value;
                return true;

            case Result<TValue, TError>.Error:
                value = default!;
                return false;

            default:
                throw new InvalidOperationException("Unknown result type");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TValue GetValue<TValue, TError>(this Result<TValue, TError> option)
    {
        return option.Match(
            ok: x => x,
            error: _ => throw new InvalidOperationException("Result is in error state")
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TValue GetValueOrDefault<TValue, TError>(
        this Result<TValue, TError> option,
        TValue defaultValue
    )
    {
        return option.Match(ok: x => x, error: _ => defaultValue);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TValue GetValueOrDefault<TValue, TError>(
        this Result<TValue, TError> option,
        Func<TValue> defaultValueFactory
    )
    {
        return option.Match(ok: x => x, error: _ => defaultValueFactory());
    }

    public static bool TryGetError<TValue, TError>(
        this Result<TValue, TError> result,
        out TError error
    )
    {
        switch (result)
        {
            case Result<TValue, TError>.Ok:
                error = default!;
                return false;

            case Result<TValue, TError>.Error err:
                error = err.Err;
                return true;

            default:
                throw new InvalidOperationException("Unknown result type");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TError GetError<TValue, TError>(this Result<TValue, TError> option)
    {
        return option.Match(
            ok: _ => throw new InvalidOperationException("Result is in Ok state"),
            error: x => x
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TError GetErrorOrDefault<TValue, TError>(
        this Result<TValue, TError> option,
        TError defaultError
    )
    {
        return option.Match(ok: _ => defaultError, error: x => x);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TError GetErrorOrDefault<TValue, TError>(
        this Result<TValue, TError> option,
        Func<TError> defaultErrorFactory
    )
    {
        return option.Match(ok: _ => defaultErrorFactory(), error: x => x);
    }
}
