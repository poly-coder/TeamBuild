using System.Diagnostics;

namespace TeamBuild.Core.Blazor;

public abstract class TbOperation<TInput, TResult> : TbOperationBase<TResult>
{
    public Action<TInput>? OnRunning { get; set; }
    public Action<TInput, TResult>? OnCompleted { get; set; }
    public Action<TInput, Exception>? OnFailed { get; set; }

    protected void SetRunning(TInput input, bool notify = true)
    {
        if (IsDisposed)
            return;

        Stage = TbOperationStage.Running;

        if (ClearOnStart)
        {
            Result = default;
            HasResult = false;
            Exception = null;
            HasException = false;
        }

        if (notify)
        {
            OnRunning?.Invoke(input);
            OnStageChanged?.Invoke(Stage);
        }
    }

    protected void SetCompleted(TInput input, TResult result, bool notify = true)
    {
        if (IsDisposed)
            return;

        Stage = TbOperationStage.Completed;
        Result = result;
        HasResult = true;
        Exception = null;
        HasException = false;

        if (notify)
        {
            OnCompleted?.Invoke(input, result);
            OnStageChanged?.Invoke(Stage);
        }
    }

    protected void SetFailed(TInput input, Exception exception, bool notify = true)
    {
        if (IsDisposed)
            return;

        Stage = TbOperationStage.Failed;
        Result = default;
        HasResult = false;
        Exception = exception;
        HasException = true;

        if (notify)
        {
            OnFailed?.Invoke(input, exception);
            OnStageChanged?.Invoke(Stage);
        }
    }
}

public class TbAsyncOperation<TInput, TResult> : TbOperation<TInput, TResult>
{
    private readonly Func<TInput, CancellationToken, Task<TResult>> runner;
    private CancellationTokenSource? currentTokenSource;

    public TbAsyncOperation(Func<TInput, CancellationToken, Task<TResult>> runner)
    {
        ArgumentNullException.ThrowIfNull(runner);

        this.runner = runner;
    }

    public void Execute(TInput input)
    {
        var activity = !string.IsNullOrWhiteSpace(ActivityName)
            ? ActivitySource?.StartActivity(
                name: ActivityName,
                kind: ActivityKind,
                tags: ActivityTags,
                links: ActivityLinks
            )
            : null;

        StopCurrent(false);

        SetRunning(input);

        var cts = new CancellationTokenSource();
        currentTokenSource = cts;

        _ = RunAsync();

        async Task RunAsync()
        {
            try
            {
                var result = await runner(input, cts.Token).ConfigureAwait(true);

                SetCompleted(input, result);

                activity?.SetStatus(ActivityStatusCode.Ok);
                activity?.Stop();
            }
            catch (OperationCanceledException)
            {
                SetCancelled();

                activity?.SetStatus(ActivityStatusCode.Unset);
                activity?.Stop();
            }
            catch (Exception exception)
            {
                SetFailed(input, exception);

                activity?.AddException(exception);
                activity?.SetStatus(ActivityStatusCode.Error);
                activity?.Stop();
            }
        }
    }

    public void Cancel()
    {
        StopCurrent(true);
    }

    private bool StopCurrent(bool notify)
    {
        if (currentTokenSource is { } cts)
        {
            cts.Dispose();
            currentTokenSource = null;

            SetCancelled(notify);

            return true;
        }

        return false;
    }
}
