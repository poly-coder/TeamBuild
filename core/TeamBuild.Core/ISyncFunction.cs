using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using static TeamBuild.Core.Disposable;

namespace TeamBuild.Core;

public interface ISyncFunction<in TKey, out TValue>
    where TKey : notnull
{
    /// <summary>
    /// Gets the value for the given key, constructing and caching it if necessary.
    /// </summary>
    /// <param name="key">The key to look up or construct.</param>
    /// <returns>The cached or newly constructed value.</returns>
    TValue GetValue(TKey key);
}

public static class SyncFunction
{
    public static ISyncFunction<TKey, TValue> Uncached<TKey, TValue>(Func<TKey, TValue> factory)
        where TKey : notnull => new UncachedSyncFunction<TKey, TValue>(factory);

    public static ISyncFunction<TKey, TValue> PermanentCached<TKey, TValue>(
        Func<TKey, TValue> factory,
        IEqualityComparer<TKey>? keyComparer = null
    )
        where TKey : notnull => new PermanentCachedSyncFunction<TKey, TValue>(factory, keyComparer);

    public static ISyncFunction<TKey, TValue> WeakCached<TKey, TValue>(
        Func<TKey, TValue> factory,
        IEqualityComparer<TKey>? keyComparer = null
    )
        where TKey : notnull => new WeakCachedSyncFunction<TKey, TValue>(factory, keyComparer);
}

public class UncachedSyncFunction<TKey, TValue> : ISyncFunction<TKey, TValue>
    where TKey : notnull
{
    private readonly Func<TKey, TValue> factory;

    public UncachedSyncFunction(Func<TKey, TValue> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        this.factory = factory;
    }

    public TValue GetValue(TKey key) => factory(key);
}

public class PermanentCachedSyncFunction<TKey, TValue> : ISyncFunction<TKey, TValue>
    where TKey : notnull
{
    private readonly Func<TKey, TValue> factory;
    private readonly ConcurrentDictionary<TKey, TValue> cache;

    public PermanentCachedSyncFunction(
        Func<TKey, TValue> factory,
        IEqualityComparer<TKey>? keyComparer = null
    )
    {
        ArgumentNullException.ThrowIfNull(factory);

        this.factory = factory;
        cache = new ConcurrentDictionary<TKey, TValue>(
            keyComparer ?? EqualityComparer<TKey>.Default
        );
    }

    public TValue GetValue(TKey key)
    {
        if (cache.TryGetValue(key, out var value))
            return value;

        return cache.GetOrAdd(key, factory);
    }
}

public class WeakCachedSyncFunction<TKey, TValue> : ISyncFunction<TKey, TValue>, IDisposable
    where TKey : notnull
{
    private readonly Func<TKey, TValue> factory;
    private readonly IEqualityComparer<TKey> keyComparer;
    private readonly ConditionalWeakTable<BoxedKey, BoxedValue> cache;
    private readonly ReaderWriterLockSlim cacheLock = new();

    public WeakCachedSyncFunction(
        Func<TKey, TValue> factory,
        IEqualityComparer<TKey>? keyComparer = null
    )
    {
        ArgumentNullException.ThrowIfNull(factory);

        this.factory = factory;
        this.keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;

        cache = new ConditionalWeakTable<BoxedKey, BoxedValue>();
    }

    public TValue GetValue(TKey key)
    {
        var boxedKey = new BoxedKey(key, keyComparer);

        cacheLock.EnterUpgradeableReadLock();
        using var _1 = Defer(() => cacheLock.ExitUpgradeableReadLock());

        if (cache.TryGetValue(boxedKey, out var boxedValue))
            return boxedValue.Value;

        cacheLock.EnterWriteLock();
        using var _2 = Defer(() => cacheLock.ExitWriteLock());

        var value = factory(key);
        boxedValue = new BoxedValue(value);
        cache.Add(boxedKey, boxedValue);
        return value;
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

    void IDisposable.Dispose()
    {
        cacheLock.Dispose();
        GC.SuppressFinalize(this);
    }
}
