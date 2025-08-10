using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TeamBuild.Core.Application.Decorators;

public class LoggingAspect(IServiceProvider provider)
{
    public virtual LogLevel BeforeLogLevel => LogLevel.Information;
    public virtual LogLevel AfterLogLevel => LogLevel.Information;
    public virtual LogLevel CatchLogLevel => LogLevel.Error;

    public virtual string BeforeStageName => "Before";
    public virtual string AfterStageName => "After";
    public virtual string CatchStageName => "Catch";

    public State Before(Type targetType, int parameterCount, string methodName)
    {
        var factory = provider.GetRequiredService<ILoggerFactory>();
        var logger = factory.CreateLogger(targetType);

        var caption = $"{targetType.Name}.{methodName}({parameterCount} params)";

        logger.Log(BeforeLogLevel, "{Stage}: {Caption}.", BeforeStageName, caption);

        var startTimestamp = Stopwatch.GetTimestamp();

        return new State(logger, caption, startTimestamp);
    }

    public void After(State state)
    {
        var elapsed = Stopwatch.GetElapsedTime(state.StartTimestamp);

        state.Logger.Log(
            AfterLogLevel,
            "{Stage}: {Caption}. Elapsed: {Elapsed:##.000}ms",
            AfterStageName,
            state.Caption,
            elapsed.TotalMilliseconds
        );
    }

    public void Catch(State state, Exception exception)
    {
        var elapsed = Stopwatch.GetElapsedTime(state.StartTimestamp);

        state.Logger.Log(
            CatchLogLevel,
            exception,
            "{Stage}: {Caption}. Elapsed: {Elapsed:##.000}ms",
            CatchStageName,
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
            Catch(state, exception);
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
            Catch(state, exception);
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
            Catch(state, exception);
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
            Catch(state, exception);
            throw;
        }
    }
}
