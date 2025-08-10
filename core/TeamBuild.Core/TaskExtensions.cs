using System.Runtime.CompilerServices;

namespace TeamBuild.Core;

public static class TaskExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<TResult[]> WhenAll<TResult>(this IEnumerable<Task<TResult>> tasks) =>
        Task.WhenAll(tasks);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task WhenAll(this IEnumerable<Task> tasks) => Task.WhenAll(tasks);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Task<TResult>> WhenAny<TResult>(this IEnumerable<Task<TResult>> tasks) =>
        Task.WhenAny(tasks);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Task> WhenAny(this IEnumerable<Task> tasks) => Task.WhenAny(tasks);
}
