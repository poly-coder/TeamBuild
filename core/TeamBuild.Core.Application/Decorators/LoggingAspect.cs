using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TeamBuild.Core.Application.Decorators;

public class LoggingAspect(IServiceProvider provider)
{
    public virtual LogLevel BeforeLogLevel => LogLevel.Information;
    public virtual LogLevel AfterLogLevel => LogLevel.Information;
    public virtual LogLevel CaughtLogLevel => LogLevel.Error;

    public virtual string BeforeStageName => "Before";
    public virtual string AfterStageName => "After";
    public virtual string CaughtStageName => "Caught";

    public State Before(Type targetType, int parameterCount, string methodName)
    {
        var factory = provider.GetRequiredService<ILoggerFactory>();
        var logger = factory.CreateLogger(targetType);

        var caption = $"{targetType.Name}.{methodName}({parameterCount} params)";

        logger.LogLoggingAspectBefore(BeforeLogLevel, BeforeStageName, caption);

        var startTimestamp = Stopwatch.GetTimestamp();

        return new State(logger, caption, startTimestamp);
    }

    public void After(State state)
    {
        var elapsed = Stopwatch.GetElapsedTime(state.StartTimestamp);

        state.Logger.LogLoggingAspectAfter(
            AfterLogLevel,
            AfterStageName,
            state.Caption,
            elapsed.TotalMilliseconds
        );
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

    public record State(ILogger Logger, string Caption, long StartTimestamp);

    public TResult Execute<TResult>(
        Type targetType,
        int parameterCount,
        Func<TResult> action,
        [CallerMemberName] string methodName = ""
    )
    {
        var state = Before(targetType, parameterCount, methodName);
        try
        {
            var result = action();
            After(state);
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
        int parameterCount,
        Action action,
        [CallerMemberName] string methodName = ""
    )
    {
        var state = Before(targetType, parameterCount, methodName);
        try
        {
            action();
            After(state);
        }
        catch (Exception exception)
        {
            Caught(state, exception);
            throw;
        }
    }

    public async Task<TResult> ExecuteAsync<TResult>(
        Type targetType,
        int parameterCount,
        Func<Task<TResult>> action,
        [CallerMemberName] string methodName = ""
    )
    {
        var state = Before(targetType, parameterCount, methodName);
        try
        {
            var result = await action();
            After(state);
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
        int parameterCount,
        Func<Task> action,
        [CallerMemberName] string methodName = ""
    )
    {
        var state = Before(targetType, parameterCount, methodName);
        try
        {
            await action();
            After(state);
        }
        catch (Exception exception)
        {
            Caught(state, exception);
            throw;
        }
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
