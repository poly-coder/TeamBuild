using System.Runtime.CompilerServices;

namespace TeamBuild.Core.Application.Decorators;

public abstract class StandardServiceDecorator(
    TracingAspect tracingAspect,
    LoggingAspect loggingAspect,
    FluentValidationAspect validationAspect,
    MetricsAspect metricsAspect
)
{
    protected async Task<TSuccess> ExecuteAsync<TSuccess>(
        Type targetType,
        IReadOnlyList<object?> parameters,
        Func<Task<TSuccess>> action,
        CancellationToken cancel,
        [CallerMemberName] string methodName = ""
    )
    {
        TSuccess result;
        var tracingState = tracingAspect.Before(targetType, methodName);
        try
        {
            var loggingState = loggingAspect.Before(targetType, parameters.Count, methodName);
            try
            {
                await validationAspect.BeforeAsync(parameters, cancel);

                result = await action();

                metricsAspect.After(parameters, result, methodName);

                loggingAspect.After(loggingState);
            }
            catch (Exception exception)
            {
                loggingAspect.Catch(loggingState, exception);
                throw;
            }

            tracingAspect.After(tracingState);
        }
        catch (Exception exception)
        {
            tracingAspect.Catch(tracingState, exception);
            throw;
        }

        return result;
    }
}
