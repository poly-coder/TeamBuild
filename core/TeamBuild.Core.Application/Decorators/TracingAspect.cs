using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TeamBuild.Core.Application.Decorators;

public class TracingAspect(ActivitySource activitySource)
{
    public State Before(Type targetType, string methodName)
    {
        var activity = activitySource.StartActivity($"{targetType.Name}.{methodName}");

        return new State(activity);
    }

    public void After(State state)
    {
        if (state.Activity is { } activity)
        {
            activity.SetStatus(ActivityStatusCode.Ok);
            activity.Stop();
        }
    }

    public void Catch(State state, Exception exception)
    {
        if (state.Activity is { } activity)
        {
            activity.AddException(exception);
            activity.SetStatus(ActivityStatusCode.Error);
            activity.Stop();
        }
    }

    public record State(Activity? Activity);

    public TResult Execute<TResult>(
        Type targetType,
        Func<TResult> action,
        [CallerMemberName] string methodName = ""
    )
    {
        var state = Before(targetType, methodName);
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

    public void Execute(Type targetType, Action action, [CallerMemberName] string methodName = "")
    {
        var state = Before(targetType, methodName);
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
        Func<Task<TResult>> action,
        [CallerMemberName] string methodName = ""
    )
    {
        var state = Before(targetType, methodName);
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
        Func<Task> action,
        [CallerMemberName] string methodName = ""
    )
    {
        var state = Before(targetType, methodName);
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
