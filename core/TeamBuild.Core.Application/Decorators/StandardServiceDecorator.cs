using System.Runtime.CompilerServices;

namespace TeamBuild.Core.Application.Decorators;

public abstract class StandardServiceAspect(
    TracingAspect tracingAspect,
    LoggingAspect loggingAspect,
    FluentValidationAspect validationAspect,
    MetricsAspect metricsAspect
)
{
    public State Before(
        Type targetType,
        IReadOnlyList<object?> parameters,
        [CallerMemberName] string methodName = ""
    )
    {
        var tracingState = tracingAspect.Before(targetType, methodName);
        var loggerState = loggingAspect.Before(targetType, methodName, parameters);
        validationAspect.Before(parameters);

        return new State(tracingState, loggerState);
    }

    public async Task<State> BeforeAsync(
        Type targetType,
        IReadOnlyList<object?> parameters,
        CancellationToken cancel = default,
        [CallerMemberName] string methodName = ""
    )
    {
        var tracingState = tracingAspect.Before(targetType, methodName);
        var loggerState = loggingAspect.Before(targetType, methodName, parameters);
        await validationAspect.BeforeAsync(parameters, cancel);

        return new State(tracingState, loggerState);
    }

    public void After(
        State state,
        IReadOnlyList<object?> parameters,
        object? result,
        [CallerMemberName] string methodName = ""
    )
    {
        metricsAspect.After(parameters, result, methodName);
        loggingAspect.After(state.LoggerState, result);
        tracingAspect.After(state.TracingState);
    }

    public void Caught(State state, Exception exception)
    {
        loggingAspect.Caught(state.LoggerState, exception);
        tracingAspect.Caught(state.TracingState, exception);
    }

    public record State(TracingAspect.State TracingState, LoggingAspect.State LoggerState);

    public TResult Execute<TResult>(
        Type targetType,
        IReadOnlyList<object?> parameters,
        Func<TResult> action,
        [CallerMemberName] string methodName = ""
    )
    {
        var state = Before(targetType, parameters, methodName);
        try
        {
            var result = action();
            After(state, parameters, result);
            return result;
        }
        catch (Exception exception)
        {
            Caught(state, exception);
            throw;
        }
    }

    public void Execute(
        Type targetType,
        IReadOnlyList<object?> parameters,
        Action action,
        [CallerMemberName] string methodName = ""
    )
    {
        var state = Before(targetType, parameters, methodName);
        try
        {
            action();
            After(state, parameters, null);
        }
        catch (Exception exception)
        {
            Caught(state, exception);
            throw;
        }
    }

    public async Task<TResult> ExecuteAsync<TResult>(
        Type targetType,
        IReadOnlyList<object?> parameters,
        Func<Task<TResult>> action,
        CancellationToken cancel = default,
        [CallerMemberName] string methodName = ""
    )
    {
        var state = await BeforeAsync(targetType, parameters, cancel, methodName);
        try
        {
            var result = await action();
            After(state, parameters, result);
            return result;
        }
        catch (Exception exception)
        {
            Caught(state, exception);
            throw;
        }
    }

    public async Task ExecuteAsync(
        Type targetType,
        IReadOnlyList<object?> parameters,
        Func<Task> action,
        CancellationToken cancel = default,
        [CallerMemberName] string methodName = ""
    )
    {
        var state = await BeforeAsync(targetType, parameters, cancel, methodName);
        try
        {
            await action();
            After(state, parameters, null);
        }
        catch (Exception exception)
        {
            Caught(state, exception);
            throw;
        }
    }
}
