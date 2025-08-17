using System.Runtime.CompilerServices;
using static TeamBuild.Core.Disposable;

namespace TeamBuild.Core;

public interface IAsyncFunction<in TKey, TValue>
    where TKey : notnull
{
    /// <summary>
    /// Gets the value for the given key, constructing and caching it if necessary.
    /// </summary>
    /// <param name="key">The key to look up or construct.</param>
    /// <param name="cancel"></param>
    /// <returns>The cached or newly constructed value.</returns>
    Task<TValue> GetValueAsync(TKey key, CancellationToken cancel = default);
}

public class UncachedAsyncFunction<TKey, TValue> : IAsyncFunction<TKey, TValue>
    where TKey : notnull
{
    private readonly Func<TKey, CancellationToken, Task<TValue>> factory;

    public UncachedAsyncFunction(Func<TKey, CancellationToken, Task<TValue>> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        this.factory = factory;
    }

    public async Task<TValue> GetValueAsync(TKey key, CancellationToken cancel = default) =>
        await factory(key, cancel);
}

public sealed class PermanentCachedAsyncFunction<TKey, TValue>
    : IAsyncFunction<TKey, TValue>,
        IDisposable
    where TKey : notnull
{
    private readonly Func<TKey, CancellationToken, Task<TValue>> factory;
    private readonly Dictionary<TKey, TValue> cache;
    private readonly ReaderWriterLockSlim cacheLock = new();

    public PermanentCachedAsyncFunction(
        Func<TKey, CancellationToken, Task<TValue>> factory,
        IEqualityComparer<TKey>? keyComparer = null
    )
    {
        ArgumentNullException.ThrowIfNull(factory);

        this.factory = factory;
        cache = new Dictionary<TKey, TValue>(keyComparer ?? EqualityComparer<TKey>.Default);
    }

    public async Task<TValue> GetValueAsync(TKey key, CancellationToken cancel = default)
    {
        cacheLock.EnterUpgradeableReadLock();
        using var _1 = Defer(() => cacheLock.ExitUpgradeableReadLock());

        if (cache.TryGetValue(key, out var value))
            return value;

        cacheLock.EnterWriteLock();
        using var _2 = Defer(() => cacheLock.ExitWriteLock());

        value = await factory(key, cancel);
        cache[key] = value;
        return value;
    }

    public void Dispose()
    {
        cacheLock.Dispose();
    }
}

public sealed class WeakCachedAsyncFunction<TKey, TValue>
    : IAsyncFunction<TKey, TValue>,
        IDisposable
    where TKey : notnull
{
    private readonly Func<TKey, CancellationToken, Task<TValue>> factory;
    private readonly IEqualityComparer<TKey> keyComparer;
    private readonly ConditionalWeakTable<BoxedKey, BoxedValue> cache;
    private readonly ReaderWriterLockSlim cacheLock = new();

    public WeakCachedAsyncFunction(
        Func<TKey, CancellationToken, Task<TValue>> factory,
        IEqualityComparer<TKey>? keyComparer = null
    )
    {
        ArgumentNullException.ThrowIfNull(factory);

        this.factory = factory;
        this.keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;

        cache = new ConditionalWeakTable<BoxedKey, BoxedValue>();
    }

    // BoxedKey is a reference type wrapper for the key, required by ConditionalWeakTable.
    private sealed class BoxedKey(TKey key, IEqualityComparer<TKey> comparer)
    {
        public TKey Key { get; } = key;

        public override bool Equals(object? obj)
        {
            return obj is BoxedKey { Key: var otherKey } && comparer.Equals(Key, otherKey);
        }

        public override int GetHashCode() => comparer.GetHashCode(Key);
    }

    // BoxedValue is a reference type wrapper for the value.
    private sealed class BoxedValue(TValue value)
    {
        public TValue Value => value;
    }

    public async Task<TValue> GetValueAsync(TKey key, CancellationToken cancel = default)
    {
        var boxedKey = new BoxedKey(key, keyComparer);

        cacheLock.EnterUpgradeableReadLock();
        using var _1 = Defer(() => cacheLock.ExitUpgradeableReadLock());

        if (cache.TryGetValue(boxedKey, out var boxedValue))
            return boxedValue.Value;

        cacheLock.EnterWriteLock();
        using var _2 = Defer(() => cacheLock.ExitWriteLock());

        var value = await factory(key, cancel);
        boxedValue = new BoxedValue(value);
        cache.Add(boxedKey, boxedValue);
        return value;
    }

    public void Dispose()
    {
        cacheLock.Dispose();
    }
}
