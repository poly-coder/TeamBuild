using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace TeamBuild.Core.Application.Decorators;

public class LoggingAspect(ILoggerFactory loggerFactory)
{
    public virtual LogLevel BeforeLogLevel => LogLevel.Information;
    public virtual LogLevel AfterLogLevel => LogLevel.Information;
    public virtual LogLevel CaughtLogLevel => LogLevel.Error;

    public virtual string BeforeStageName => "Before";
    public virtual string AfterStageName => "After";
    public virtual string CaughtStageName => "Caught";

    public State Before(Type targetType, string methodName, IReadOnlyList<object?> parameters)
    {
        var logger = loggerFactory.CreateLogger(targetType);

        var caption = GetCaption(targetType, methodName, parameters);

        logger.LogLoggingAspectBefore(BeforeLogLevel, BeforeStageName, caption);

        var startTimestamp = Stopwatch.GetTimestamp();

        return new State(logger, targetType, methodName, caption, startTimestamp);
    }

    public void After(State state, object? result)
    {
        var elapsed = Stopwatch.GetElapsedTime(state.StartTimestamp);

        var resultCaption = GetResultCaption(state.TargetType, state.MethodName, result);

        if (!string.IsNullOrWhiteSpace(resultCaption))
        {
            state.Logger.LogLoggingAspectAfterResult(
                AfterLogLevel,
                AfterStageName,
                state.Caption,
                resultCaption,
                elapsed.TotalMilliseconds
            );
        }
        else
        {
            state.Logger.LogLoggingAspectAfter(
                AfterLogLevel,
                AfterStageName,
                state.Caption,
                elapsed.TotalMilliseconds
            );
        }
    }

    public void Caught(State state, Exception exception)
    {
        var elapsed = Stopwatch.GetElapsedTime(state.StartTimestamp);

        state.Logger.LogLoggingAspectCaught(
            CaughtLogLevel,
            exception,
            CaughtStageName,
            state.Caption,
            elapsed.TotalMilliseconds
        );
    }

    public record State(
        ILogger Logger,
        Type TargetType,
        string MethodName,
        string Caption,
        long StartTimestamp
    );

    public TResult Execute<TResult>(
        Type targetType,
        IReadOnlyList<object?> parameters,
        Func<TResult> action,
        [CallerMemberName] string methodName = ""
    )
    {
        var state = Before(targetType, methodName, parameters);
        try
        {
            var result = action();
            After(state, result);
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
        var state = Before(targetType, methodName, parameters);
        try
        {
            action();
            After(state, null);
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
        [CallerMemberName] string methodName = ""
    )
    {
        var state = Before(targetType, methodName, parameters);
        try
        {
            var result = await action();
            After(state, result);
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
        [CallerMemberName] string methodName = ""
    )
    {
        var state = Before(targetType, methodName, parameters);
        try
        {
            await action();
            After(state, null);
        }
        catch (Exception exception)
        {
            Caught(state, exception);
            throw;
        }
    }

    protected virtual string GetCaption(
        Type targetType,
        string methodName,
        IReadOnlyList<object?> parameters
    )
    {
        var methodCaption = GetMethodCaption(targetType, methodName);
        var parametersCaption = GetParametersCaption(targetType, methodName, parameters);

        return $"{methodCaption}({parametersCaption})";
    }

    protected virtual string GetMethodCaption(Type targetType, string methodName)
    {
        return $"{targetType.Name}.{methodName}";
    }

    protected virtual string GetParametersCaption(
        Type targetType,
        string methodName,
        IReadOnlyList<object?> parameters
    )
    {
        return string.Join(", ", parameters.Select(GetObjectCaption));
    }

    protected virtual string GetResultCaption(Type targetType, string methodName, object? result)
    {
        return GetObjectCaption(result);
    }

    protected virtual string GetObjectCaption(object? value)
    {
        return value switch
        {
            null => string.Empty,
            _ => "✓",
        };
    }
}

internal static partial class LoggingAspectLogs
{
    [LoggerMessage("{Stage}: {Caption}.")]
    public static partial void LogLoggingAspectBefore(
        this ILogger logger,
        LogLevel logLevel,
        string stage,
        string caption
    );

    [LoggerMessage("{Stage}: {Caption}. Elapsed: {Elapsed:##.000}ms")]
    public static partial void LogLoggingAspectAfter(
        this ILogger logger,
        LogLevel logLevel,
        string stage,
        string caption,
        double elapsed
    );

    [LoggerMessage("{Stage}: {Caption}. Result: {Result}. Elapsed: {Elapsed:##.000}ms")]
    public static partial void LogLoggingAspectAfterResult(
        this ILogger logger,
        LogLevel logLevel,
        string stage,
        string caption,
        string result,
        double elapsed
    );

    [LoggerMessage("{Stage}: {Caption}. Elapsed: {Elapsed:##.000}ms")]
    public static partial void LogLoggingAspectCaught(
        this ILogger logger,
        LogLevel logLevel,
        Exception exception,
        string stage,
        string caption,
        double elapsed
    );
}
