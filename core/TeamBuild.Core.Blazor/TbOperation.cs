using System.Diagnostics.CodeAnalysis;

namespace TeamBuild.Core.Blazor;

public abstract class TbOperation<TResult> : IDisposable
{
    public bool ClearOnStart { get; set; } = true;
    public bool ClearOnCancel { get; set; } = true;

    public bool IsDisposed { get; private set; }

    public TbOperationStage Stage { get; private set; } = TbOperationStage.Idle;

    public TResult? Result { get; private set; }

    public bool HasResult { get; private set; }

    public Exception? Exception { get; private set; }

    public bool HasException { get; private set; }

    public bool TryGetResult([NotNullWhen(true)] out TResult? result)
    {
        result = HasResult ? Result : default;
        return HasResult;
    }

    public bool TryGetException([NotNullWhen(true)] out Exception? exception)
    {
        exception = HasException ? Exception : null;
        return HasException;
    }

    public Action<TbOperationStage>? OnStageChanged { get; set; }
    public Action? OnRunning { get; set; }
    public Action<TResult>? OnCompleted { get; set; }
    public Action<Exception>? OnFailed { get; set; }
    public Action? OnCancelled { get; set; }

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

    protected void SetCancelled(bool notify = true)
    {
        if (IsDisposed)
            return;

        Stage = TbOperationStage.Cancelled;

        if (ClearOnCancel)
        {
            Result = default;
            HasResult = false;
            Exception = null;
            HasException = false;
        }

        if (notify)
        {
            OnCancelled?.Invoke();
            OnStageChanged?.Invoke(Stage);
        }
    }

    public virtual void Dispose()
    {
        IsDisposed = true;
    }
}

public abstract class TbAsyncOperationBase<TResult> : TbOperation<TResult>
{
    private CancellationTokenSource? currentTokenSource;

    protected void ExecuteInternal(Func<CancellationToken, Task<TResult>> runner)
    {
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
            }
            catch (OperationCanceledException)
            {
                SetCancelled();
            }
            catch (Exception exception)
            {
                SetFailed(exception);
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

public class TbAsyncOperation<TResult> : TbAsyncOperationBase<TResult>
{
    private readonly Func<CancellationToken, Task<TResult>> runner;

    public TbAsyncOperation(Func<CancellationToken, Task<TResult>> runner)
    {
        ArgumentNullException.ThrowIfNull(runner);

        this.runner = runner;
    }

    public void Execute()
    {
        ExecuteInternal(runner);
    }
}

public class TbAsyncOperation<TInput, TResult> : TbAsyncOperationBase<TResult>
{
    private readonly Func<TInput, CancellationToken, Task<TResult>> runner;

    public TbAsyncOperation(Func<TInput, CancellationToken, Task<TResult>> runner)
    {
        ArgumentNullException.ThrowIfNull(runner);

        this.runner = runner;
    }

    public void Execute(TInput input)
    {
        ExecuteInternal(ct => runner(input, ct));
    }
}
