namespace TeamBuild.Core;

public static class Disposable
{
    public static DeferDisposable Defer(Action action)
    {
        return new DeferDisposable(action);
    }

    public static DeferAsyncDisposable DeferAsync(Func<ValueTask> action)
    {
        return new DeferAsyncDisposable(action);
    }

    public struct DeferDisposable : IDisposable
    {
        private Action? action;

        public DeferDisposable(Action action)
        {
            ArgumentNullException.ThrowIfNull(action);
            this.action = action;
        }

        public void Dispose()
        {
            Action? toDispose = Interlocked.Exchange(ref action, null);
            toDispose?.Invoke();
        }
    }

    public struct DeferAsyncDisposable : IAsyncDisposable
    {
        private Func<ValueTask>? action;

        public DeferAsyncDisposable(Func<ValueTask> action)
        {
            ArgumentNullException.ThrowIfNull(action);
            this.action = action;
        }

        public ValueTask DisposeAsync()
        {
            Func<ValueTask>? toDispose = Interlocked.Exchange(ref action, null);
            return toDispose?.Invoke() ?? default;
        }
    }

    public static void DisposeIfNeeded(this object? source)
    {
        if (source is IDisposable disposable)
            disposable.Dispose();
    }
}
