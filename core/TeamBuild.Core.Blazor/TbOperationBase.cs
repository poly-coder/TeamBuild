using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace TeamBuild.Core.Blazor;

public abstract class TbOperationBase<TResult> : IDisposable
{
    public ActivitySource? ActivitySource { get; set; }
    public string? ActivityName { get; set; }
    public ActivityKind ActivityKind { get; set; }
    public IEnumerable<KeyValuePair<string, object?>>? ActivityTags { get; set; }
    public IEnumerable<ActivityLink>? ActivityLinks { get; set; }

    public bool ClearOnStart { get; set; } = true;
    public bool ClearOnCancel { get; set; } = true;

    public bool IsDisposed { get; private set; }

    public TbOperationStage Stage { get; protected set; } = TbOperationStage.Idle;

    public TResult? Result { get; protected set; }

    public bool HasResult { get; protected set; }

    public Exception? Exception { get; protected set; }

    public bool HasException { get; protected set; }

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

    public bool IsRunning => Stage == TbOperationStage.Running;
    public bool IsFinished =>
        Stage
            is TbOperationStage.Completed
                or TbOperationStage.Failed
                or TbOperationStage.Cancelled;

    public Action<TbOperationStage>? OnStageChanged { get; set; }
    public Action? OnCancelled { get; set; }

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
        GC.SuppressFinalize(this);
    }
}
