using System.Runtime.CompilerServices;

namespace TeamBuild.Core;

public static class Disposable
{
    public static DeferDisposable Defer(
        Action action,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new DeferDisposable(action, filePath, lineNumber);
    }

    public readonly struct DeferDisposable : IDisposable
    {
        private readonly Action action;
        private readonly string filePath;
        private readonly int lineNumber;

        public DeferDisposable(Action action, string filePath, int lineNumber)
        {
            this.action = action;
            this.filePath = filePath;
            this.lineNumber = lineNumber;
        }

        public void Dispose()
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(
                    $"Error in deferred action at {filePath}:{lineNumber} - {ex.Message}"
                );
            }
        }
    }

    public static void DisposeIfNeeded(this object? source)
    {
        if (source is IDisposable disposable)
            disposable.Dispose();
    }
}
