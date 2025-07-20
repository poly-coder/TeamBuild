using System.Runtime.CompilerServices;

namespace TeamBuild.Core;

public static class SystemTaskExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<TResult[]> WhenAll<TResult>(this IEnumerable<Task<TResult>> tasks) =>
        Task.WhenAll(tasks);
}
