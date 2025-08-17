using System.Diagnostics;

namespace TeamBuild.Core.Blazor;

public abstract class TbOperation<TResult> : TbOperationBase<TResult>
{
    public Action? OnRunning { get; set; }
    public Action<TResult>? OnCompleted { get; set; }
    public Action<Exception>? OnFailed { get; set; }

    protected void SetRunning(bool notify = true)
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
            OnRunning?.Invoke();
            OnStageChanged?.Invoke(Stage);
        }
    }

    protected void SetCompleted(TResult result, bool notify = true)
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
            OnCompleted?.Invoke(result);
            OnStageChanged?.Invoke(Stage);
        }
    }

    protected void SetFailed(Exception exception, bool notify = true)
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
            OnFailed?.Invoke(exception);
            OnStageChanged?.Invoke(Stage);
        }
    }
}

public class TbAsyncOperation<TResult> : TbOperation<TResult>
{
    private readonly Func<CancellationToken, Task<TResult>> runner;
    private CancellationTokenSource? currentTokenSource;

    public TbAsyncOperation(Func<CancellationToken, Task<TResult>> runner)
    {
        ArgumentNullException.ThrowIfNull(runner);

        this.runner = runner;
    }

    public void Execute()
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

        SetRunning();

        var cts = new CancellationTokenSource();
        currentTokenSource = cts;

        _ = RunAsync();

        async Task RunAsync()
        {
            try
            {
                var result = await runner(cts.Token).ConfigureAwait(true);

                SetCompleted(result);

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
                SetFailed(exception);

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

    private void StopCurrent(bool notify)
    {
        if (currentTokenSource is { } cts)
        {
            cts.Dispose();
            currentTokenSource = null;

            SetCancelled(notify);
        }
    }
}
