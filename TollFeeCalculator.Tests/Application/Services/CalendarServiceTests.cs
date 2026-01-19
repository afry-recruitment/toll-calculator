using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using TollFeeCalculator.Application.Interfaces;
using TollFeeCalculator.Application.Options;
using TollFeeCalculator.Application.Services;
using TollFeeCalculator.Tests.TestData;

namespace TollFeeCalculator.Tests.Application.Services;

[TestClass]
public sealed class CalendarServiceTests
{
    private sealed class TestSystemClock(DateTimeOffset utcNow) : ISystemClock
    {
        public DateTimeOffset UtcNow { get; private set; } = utcNow;
        public void Advance(TimeSpan by) => UtcNow = UtcNow.Add(by);
    }

    private sealed class OptionsSnapshotShim<T> : IOptionsSnapshot<T> where T : class, new()
    {
        public OptionsSnapshotShim(T value) => Value = value;
        public T Value { get; }
        public T Get(string name) => Value;
    }

    private sealed class FakeHolidayProvider : IHolidayProvider
    {
        private readonly Func<int, CancellationToken, Task<IReadOnlySet<DateOnly>>> _impl;
        public int Calls { get; private set; }

        public FakeHolidayProvider(Func<int, CancellationToken, Task<IReadOnlySet<DateOnly>>> impl) => _impl = impl;

        public async Task<IReadOnlySet<DateOnly>> GetHolidaysAsync(int year, CancellationToken ct)
        {
            Calls++;
            return await _impl(year, ct);
        }
    }

    private sealed class CapturingMemoryCache : IMemoryCache
    {
        private sealed class CapturingCacheEntry : ICacheEntry
        {
            private readonly CapturingMemoryCache _owner;

            public CapturingCacheEntry(CapturingMemoryCache owner, object key)
            {
                _owner = owner;
                Key = key;
            }

            public object Key { get; }
            public object? Value { get; set; }
            public DateTimeOffset? AbsoluteExpiration { get; set; }
            public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
            public TimeSpan? SlidingExpiration { get; set; }
            public IList<IChangeToken> ExpirationTokens { get; } = new List<IChangeToken>();
            public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; } = new List<PostEvictionCallbackRegistration>();
            public CacheItemPriority Priority { get; set; }
            public long? Size { get; set; }

            public void Dispose()
            {
                _owner.LastSetKey = Key;
                _owner.LastSetOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = AbsoluteExpiration,
                    AbsoluteExpirationRelativeToNow = AbsoluteExpirationRelativeToNow,
                    SlidingExpiration = SlidingExpiration,
                    Priority = Priority,
                    Size = Size
                };

                _owner._values[Key] = Value;
            }
        }

        private readonly Dictionary<object, object?> _values = new();

        public MemoryCacheEntryOptions? LastSetOptions { get; private set; }
        public object? LastSetKey { get; private set; }

        public ICacheEntry CreateEntry(object key) => new CapturingCacheEntry(this, key);

        public void Dispose() { }

        public void Remove(object key) => _values.Remove(key);

        public bool TryGetValue(object key, out object? value) => _values.TryGetValue(key, out value);

        public TItem Set<TItem>(object key, TItem value, MemoryCacheEntryOptions options)
        {
            LastSetKey = key;
            LastSetOptions = options;
            _values[key] = value;
            return value;
        }
    }

    [DataTestMethod]
    [DynamicData(nameof(ApplicationTestData.CalendarService_IsTollFreeAsync_WeekendCases), typeof(ApplicationTestData), DynamicDataSourceType.Method)]
    public async Task IsTollFreeAsync_WeekendCases_ReturnExpected(TollFeeOptions options, DateTime date, bool expected)
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var provider = new FakeHolidayProvider((_, __) => Task.FromResult<IReadOnlySet<DateOnly>>(new HashSet<DateOnly>()));
        var svc = new CalendarService(provider, new OptionsSnapshotShim<TollFeeOptions>(options), cache, NullLogger<CalendarService>.Instance);

        var result = await svc.IsTollFreeAsync(DateOnly.FromDateTime(date), CancellationToken.None);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public async Task IsTollFreeAsync_CachesHolidays_PerYear()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());

        var provider = new FakeHolidayProvider((year, _) =>
            Task.FromResult<IReadOnlySet<DateOnly>>(new HashSet<DateOnly> { new DateOnly(year, 1, 1) }));

        var options = new TollFeeOptions { TollFreeWeekends = false, HolidaysCacheDurationHours = 1 };
        var svc = new CalendarService(provider, new OptionsSnapshotShim<TollFeeOptions>(options), cache, NullLogger<CalendarService>.Instance);

        var ct = CancellationToken.None;

        // Same year twice => provider should be called once due to caching.
        Assert.IsTrue(await svc.IsTollFreeAsync(new DateOnly(2026, 1, 1), ct));
        Assert.IsTrue(await svc.IsTollFreeAsync(new DateOnly(2026, 1, 1), ct));

        Assert.AreEqual(1, provider.Calls);
    }

    [TestMethod]
    public async Task IsTollFreeAsync_WhenProviderFails_ReturnsConfiguredFallback()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());

        var provider = new FakeHolidayProvider((_, __) => throw new InvalidOperationException("boom"));

        var options = new TollFeeOptions
        {
            TollFreeWeekends = false,
            HolidaysFailResultsTollFree = true,
            HolidaysCacheDurationHours = 1
        };

        var svc = new CalendarService(provider, new OptionsSnapshotShim<TollFeeOptions>(options), cache, NullLogger<CalendarService>.Instance);

        // Provider fails -> holidays empty -> return fallback option value
        var result = await svc.IsTollFreeAsync(new DateOnly(2026, 1, 2), CancellationToken.None);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task IsTollFreeAsync_CancellationRequested_Throws()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());

        var provider = new FakeHolidayProvider(async (_, ct) =>
        {
            await Task.Delay(10, ct);
            return new HashSet<DateOnly>();
        });

        var options = new TollFeeOptions { TollFreeWeekends = false };
        var svc = new CalendarService(provider, new OptionsSnapshotShim<TollFeeOptions>(options), cache, NullLogger<CalendarService>.Instance);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsExceptionAsync<OperationCanceledException>(() =>
            svc.IsTollFreeAsync(new DateOnly(2026, 1, 2), cts.Token));
    }

    [TestMethod]
    public async Task IsTollFreeAsync_WhenProviderReturnsNull_UsesFallback()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());

        var provider = new FakeHolidayProvider((_, __) => Task.FromResult<IReadOnlySet<DateOnly>>(null!));

        var options = new TollFeeOptions
        {
            TollFreeWeekends = false,
            HolidaysFailResultsTollFree = true,
            HolidaysCacheDurationHours = 1
        };

        var svc = new CalendarService(provider, new OptionsSnapshotShim<TollFeeOptions>(options), cache, NullLogger<CalendarService>.Instance);

        var result = await svc.IsTollFreeAsync(new DateOnly(2026, 1, 2), CancellationToken.None);

        Assert.IsTrue(result);
        Assert.AreEqual(1, provider.Calls);
    }

    [TestMethod]
    public async Task IsTollFreeAsync_WhenProviderReturnsEmptySet_UsesFallbackFalse()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());

        var provider = new FakeHolidayProvider((_, __) =>
            Task.FromResult<IReadOnlySet<DateOnly>>(new HashSet<DateOnly>()));

        var options = new TollFeeOptions
        {
            TollFreeWeekends = false,
            HolidaysFailResultsTollFree = false,
            HolidaysCacheDurationHours = 1
        };

        var svc = new CalendarService(provider, new OptionsSnapshotShim<TollFeeOptions>(options), cache, NullLogger<CalendarService>.Instance);

        var result = await svc.IsTollFreeAsync(new DateOnly(2026, 1, 2), CancellationToken.None);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task IsTollFreeAsync_CacheEntryNull_DoesNotShortCircuit()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());

        // Manually seed a null cache value for the year.
        using (var entry = cache.CreateEntry("holidays_2026"))
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            entry.Value = null;
        }

        var provider = new FakeHolidayProvider((year, _) =>
            Task.FromResult<IReadOnlySet<DateOnly>>(new HashSet<DateOnly> { new DateOnly(year, 1, 1) }));

        var options = new TollFeeOptions { TollFreeWeekends = false, HolidaysCacheDurationHours = 1 };
        var svc = new CalendarService(provider, new OptionsSnapshotShim<TollFeeOptions>(options), cache, NullLogger<CalendarService>.Instance);

        Assert.IsTrue(await svc.IsTollFreeAsync(new DateOnly(2026, 1, 1), CancellationToken.None));
        Assert.AreEqual(1, provider.Calls);
    }

    [TestMethod]
    public async Task IsTollFreeAsync_CachesSeparately_PerYear()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());

        var provider = new FakeHolidayProvider((year, _) =>
            Task.FromResult<IReadOnlySet<DateOnly>>(new HashSet<DateOnly> { new DateOnly(year, 1, 1) }));

        var options = new TollFeeOptions { TollFreeWeekends = false, HolidaysCacheDurationHours = 1 };
        var svc = new CalendarService(provider, new OptionsSnapshotShim<TollFeeOptions>(options), cache, NullLogger<CalendarService>.Instance);

        Assert.IsTrue(await svc.IsTollFreeAsync(new DateOnly(2026, 1, 1), CancellationToken.None));
        Assert.IsTrue(await svc.IsTollFreeAsync(new DateOnly(2027, 1, 1), CancellationToken.None));

        Assert.AreEqual(2, provider.Calls);
    }

    [TestMethod]
    public async Task IsTollFreeAsync_CacheHit_ReadsFromCache_AndDoesNotCallProvider()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());

        var seeded = new HashSet<DateOnly> { new DateOnly(2026, 1, 2) };
        cache.Set("holidays_2026", (IReadOnlySet<DateOnly>)seeded, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });

        var provider = new FakeHolidayProvider((_, __) =>
            Task.FromResult<IReadOnlySet<DateOnly>>(new HashSet<DateOnly>()));

        var options = new TollFeeOptions { TollFreeWeekends = false, HolidaysCacheDurationHours = 1 };
        var svc = new CalendarService(provider, new OptionsSnapshotShim<TollFeeOptions>(options), cache, NullLogger<CalendarService>.Instance);

        Assert.IsTrue(await svc.IsTollFreeAsync(new DateOnly(2026, 1, 2), CancellationToken.None));
        Assert.AreEqual(0, provider.Calls);
    }

    [TestMethod]
    public async Task IsTollFreeAsync_CacheMiss_SetsCacheEntry()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());

        var provider = new FakeHolidayProvider((year, _) =>
            Task.FromResult<IReadOnlySet<DateOnly>>(new HashSet<DateOnly> { new DateOnly(year, 1, 1) }));

        var options = new TollFeeOptions { TollFreeWeekends = false, HolidaysCacheDurationHours = 1 };
        var svc = new CalendarService(provider, new OptionsSnapshotShim<TollFeeOptions>(options), cache, NullLogger<CalendarService>.Instance);

        Assert.IsTrue(await svc.IsTollFreeAsync(new DateOnly(2026, 1, 1), CancellationToken.None));

        Assert.IsTrue(cache.TryGetValue<IReadOnlySet<DateOnly>>("holidays_2026", out var cached));
        Assert.IsNotNull(cached);
        Assert.IsTrue(cached.Contains(new DateOnly(2026, 1, 1)));
    }

    [TestMethod]
    public async Task IsTollFreeAsync_WhenProviderReturnsEmptySet_CachesEmpty_AndReadsItOnNextCall()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());

        var provider = new FakeHolidayProvider((_, __) =>
            Task.FromResult<IReadOnlySet<DateOnly>>(new HashSet<DateOnly>()));

        var options = new TollFeeOptions
        {
            TollFreeWeekends = false,
            HolidaysFailResultsTollFree = false,
            HolidaysCacheDurationHours = 1
        };

        var svc = new CalendarService(provider, new OptionsSnapshotShim<TollFeeOptions>(options), cache, NullLogger<CalendarService>.Instance);

        // First call populates cache (with empty set)
        Assert.IsFalse(await svc.IsTollFreeAsync(new DateOnly(2026, 1, 2), CancellationToken.None));
        Assert.AreEqual(1, provider.Calls);

        // Second call should hit cache and avoid provider
        Assert.IsFalse(await svc.IsTollFreeAsync(new DateOnly(2026, 1, 2), CancellationToken.None));
        Assert.AreEqual(1, provider.Calls);

        Assert.IsTrue(cache.TryGetValue<IReadOnlySet<DateOnly>>("holidays_2026", out var cached));
        Assert.IsNotNull(cached);
        Assert.AreEqual(0, cached.Count);
    }

    [TestMethod]
    public async Task IsTollFreeAsync_WhenProviderReturnsEmptySet_SetsCacheExpirationTo5Minutes()
    {
        var cache = new CapturingMemoryCache();

        var provider = new FakeHolidayProvider((_, __) =>
            Task.FromResult<IReadOnlySet<DateOnly>>(new HashSet<DateOnly>()));

        var options = new TollFeeOptions
        {
            TollFreeWeekends = false,
            HolidaysCacheDurationHours = 24, // should be overridden to 5 minutes on empty.
            HolidaysFailResultsTollFree = false
        };

        var svc = new CalendarService(provider, new OptionsSnapshotShim<TollFeeOptions>(options), cache, NullLogger<CalendarService>.Instance);

        _ = await svc.IsTollFreeAsync(new DateOnly(2026, 1, 2), CancellationToken.None);

        Assert.AreEqual("holidays_2026", cache.LastSetKey);
        Assert.IsNotNull(cache.LastSetOptions);
        Assert.AreEqual(TimeSpan.FromMinutes(5), cache.LastSetOptions.AbsoluteExpirationRelativeToNow);
    }

    [TestMethod]
    public async Task IsTollFreeAsync_WhenProviderThrows_CachesEmptySet_AndHitsCacheUntilExpiration()
    {
        var clock = new TestSystemClock(new DateTimeOffset(2026,1,1,0,0,0, TimeSpan.Zero));
        using var cache = new MemoryCache(new MemoryCacheOptions { Clock = clock });

        var provider = new FakeHolidayProvider((_, __) => throw new InvalidOperationException("boom"));

        var options = new TollFeeOptions
        {
            TollFreeWeekends = false,
            HolidaysCacheDurationHours =24,
            HolidaysFailResultsTollFree = false
        };

        var svc = new CalendarService(provider, new OptionsSnapshotShim<TollFeeOptions>(options), cache, NullLogger<CalendarService>.Instance);

        var date = new DateOnly(2026,1,2);

        // First call => provider throws => empty set cached with 5 minute TTL.
        Assert.IsFalse(await svc.IsTollFreeAsync(date, CancellationToken.None));
        Assert.AreEqual(1, provider.Calls);

        Assert.IsTrue(cache.TryGetValue<IReadOnlySet<DateOnly>>("holidays_2026", out var cached));
        Assert.IsNotNull(cached);
        Assert.AreEqual(0, cached.Count);

        // Second call before expiry => should hit cache and NOT call provider again.
        Assert.IsFalse(await svc.IsTollFreeAsync(date, CancellationToken.None));
        Assert.AreEqual(1, provider.Calls);

        // Advance past expiry => third call should call provider again (and still return false).
        clock.Advance(TimeSpan.FromMinutes(6));

        Assert.IsFalse(await svc.IsTollFreeAsync(date, CancellationToken.None));
        Assert.AreEqual(2, provider.Calls);
    }
}