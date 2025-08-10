using System.Runtime.CompilerServices;

namespace TeamBuild.Core.Application.Decorators;

public abstract class MetricsAspect
{
    public void After(IReadOnlyList<object?> parameters, object? result, string methodName)
    {
        Count(methodName, parameters, result);
    }

    protected abstract void Count(
        string methodName,
        IReadOnlyList<object?> parameters,
        object? result
    );

    public TResult Execute<TResult>(
        IReadOnlyList<object?> parameters,
        Func<TResult> action,
        [CallerMemberName] string methodName = ""
    )
    {
        var result = action();

        After(parameters, result, methodName);

        return result;
    }

    public void Execute(
        IReadOnlyList<object?> parameters,
        Action action,
        [CallerMemberName] string methodName = ""
    )
    {
        action();

        After(parameters, null, methodName);
    }

    public async Task<TResult> ExecuteAsync<TResult>(
        IReadOnlyList<object?> parameters,
        Func<Task<TResult>> action,
        [CallerMemberName] string methodName = ""
    )
    {
        var result = await action();

        After(parameters, result, methodName);

        return result;
    }

    public async Task ExecuteAsync(
        IReadOnlyList<object?> parameters,
        Func<Task> action,
        [CallerMemberName] string methodName = ""
    )
    {
        await action();

        After(parameters, null, methodName);
    }
}
