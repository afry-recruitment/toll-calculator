using Microsoft.Extensions.Options;
using TollFeeCalculator.Application.Interfaces;
using TollFeeCalculator.Application.Options;
using TollFeeCalculator.Application.Services;
using TollFeeCalculator.Enterprise.Enums;
using TollFeeCalculator.Enterprise.Interfaces;
using TollFeeCalculator.Enterprise.Policies;
using TollFeeCalculator.Enterprise.ValueObjects;
using TollFeeCalculator.Tests.TestData;

namespace TollFeeCalculator.Tests.Application.Services;

[TestClass]
public sealed class TollFeeServiceTests
{
    private sealed class FakeCalendarService : ICalendarService
    {
        private readonly Func<DateOnly, CancellationToken, Task<bool>> _impl;
        public FakeCalendarService(Func<DateOnly, CancellationToken, Task<bool>> impl) => _impl = impl;
        public Task<bool> IsTollFreeAsync(DateOnly date, CancellationToken ct) => _impl(date, ct);
    }

    private sealed class FakeVehicleService : IVehicleService
    {
        private readonly Func<TollFeeCalculator.Enterprise.Entities.Vehicle, bool> _impl;
        public FakeVehicleService(Func<TollFeeCalculator.Enterprise.Entities.Vehicle, bool> impl) => _impl = impl;
        public bool IsTollFree(TollFeeCalculator.Enterprise.Entities.Vehicle vehicle) => _impl(vehicle);
    }

    private sealed class FakeFeeRangeProvider : IFeeRangeProvider
    {
        private readonly IReadOnlyList<FeeRange> _ranges;
        public FakeFeeRangeProvider(FeeRange[] ranges) => _ranges = ranges;
        public IReadOnlyList<FeeRange> GetRange() => _ranges;
    }

    private static IFeeSchedule CreateGothenburgSchedule()
    {
        var ranges = new[]
        {
            new FeeRange(new TimeOnly(6, 0),  new TimeOnly(6, 30),  8),
            new FeeRange(new TimeOnly(6, 30), new TimeOnly(7, 0),  13),
            new FeeRange(new TimeOnly(7, 0),  new TimeOnly(8, 0),  18),
            new FeeRange(new TimeOnly(8, 0),  new TimeOnly(8, 30), 13),
            new FeeRange(new TimeOnly(8, 30), new TimeOnly(15, 0), 8),
            new FeeRange(new TimeOnly(15, 0), new TimeOnly(15, 30), 13),
            new FeeRange(new TimeOnly(15, 30), new TimeOnly(17, 0), 18),
            new FeeRange(new TimeOnly(17, 0), new TimeOnly(18, 0), 13),
            new FeeRange(new TimeOnly(18, 0), new TimeOnly(18, 30), 8),
        };

        return new FeeSchedule(new FakeFeeRangeProvider(ranges));
    }

    [TestMethod]
    public async Task GetFee_NullVehicle_Throws()
    {
        var svc = new TollFeeService(
            CreateGothenburgSchedule(),
            new FakeCalendarService((_, __) => Task.FromResult(false)),
            new FakeVehicleService(_ => false),
            Options.Create(new TollFeeOptions()));

        await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
            svc.GetFee(new[] { DateTime.UtcNow }, null!, CancellationToken.None));
    }

    [DataTestMethod]
    [DynamicData(nameof(ApplicationTestData.TollFeeService_GetFee_EdgeCases), typeof(ApplicationTestData), DynamicDataSourceType.Method)]
    public async Task GetFee_EdgeCases_ReturnExpectedTotal(DateTime[]? dateTimes, TollFeeCalculator.Enterprise.Entities.Vehicle vehicle, TollFeeOptions options, int expected)
    {
        var svc = new TollFeeService(
            CreateGothenburgSchedule(),
            new FakeCalendarService((_, __) => Task.FromResult(false)),
            new FakeVehicleService(_ => false),
            Options.Create(options));

        var result = await svc.GetFee(dateTimes, vehicle, CancellationToken.None);

        Assert.IsNull(result.Exception);
        Assert.AreEqual(expected, result.Fee);
    }

    [TestMethod]
    public async Task GetFee_TollFreeVehicle_Returns0()
    {
        var svc = new TollFeeService(
            CreateGothenburgSchedule(),
            new FakeCalendarService((_, __) => Task.FromResult(false)),
            new FakeVehicleService(_ => true),
            Options.Create(new TollFeeOptions()));

        var result = await svc.GetFee(new[] { new DateTime(2026, 1, 2, 7, 0, 0) }, new TollFeeCalculator.Enterprise.Entities.Vehicle(VehicleType.Car), CancellationToken.None);

        Assert.AreEqual(0, result.Fee);
    }

    [DataTestMethod]
    [DynamicData(nameof(ApplicationTestData.TollFeeService_GetFee_NormalAndRulesCases), typeof(ApplicationTestData), DynamicDataSourceType.Method)]
    public async Task GetFee_NormalAndRules_Cases_ReturnExpectedTotal(DateTime[] dateTimes, TollFeeOptions options, int expected)
    {
        var svc = new TollFeeService(
            CreateGothenburgSchedule(),
            new FakeCalendarService((_, __) => Task.FromResult(false)),
            new FakeVehicleService(_ => false),
            Options.Create(options));

        var result = await svc.GetFee(dateTimes, new TollFeeCalculator.Enterprise.Entities.Vehicle(VehicleType.Car), CancellationToken.None);

        Assert.AreEqual(expected, result.Fee);
        Assert.IsNull(result.Exception);
    }

    [TestMethod]
    public async Task GetFee_TollFreeDate_SkipsThatDay()
    {
        var svc = new TollFeeService(
            CreateGothenburgSchedule(),
            new FakeCalendarService((date, _) => Task.FromResult(date == new DateOnly(2026, 1, 2))),
            new FakeVehicleService(_ => false),
            Options.Create(new TollFeeOptions()));

        var dateTimes = new[]
        {
            new DateTime(2026, 1, 2, 7, 0, 0), // should be skipped, would be 18
            new DateTime(2026, 1, 3, 6, 15, 0), // included, 8 (calendar returns false for 1/3)
        };

        var result = await svc.GetFee(dateTimes, new TollFeeCalculator.Enterprise.Entities.Vehicle(VehicleType.Car), CancellationToken.None);

        Assert.AreEqual(8, result.Fee);
    }

    [TestMethod]
    public async Task GetFee_CancellationRequested_Throws()
    {
        var svc = new TollFeeService(
            CreateGothenburgSchedule(),
            new FakeCalendarService(async (_, ct) =>
            {
                await Task.Delay(10, ct);
                return false;
            }),
            new FakeVehicleService(_ => false),
            Options.Create(new TollFeeOptions()));

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsExceptionAsync<OperationCanceledException>(() =>
            svc.GetFee(new[] { new DateTime(2026, 1, 2, 7, 0, 0) }, new TollFeeCalculator.Enterprise.Entities.Vehicle(VehicleType.Car), cts.Token));
    }

    [TestMethod]
    public async Task GetFee_AllDatesTollFree_Returns0_AndDoesNotCap()
    {
        var svc = new TollFeeService(
            CreateGothenburgSchedule(),
            new FakeCalendarService((_, __) => Task.FromResult(true)),
            new FakeVehicleService(_ => false),
            Options.Create(new TollFeeOptions { MaxDailyFee =1 }));

        var result = await svc.GetFee(
            new[]
            {
                new DateTime(2026,1,2,7,0,0),
                new DateTime(2026,1,2,8,0,0),
            },
            new TollFeeCalculator.Enterprise.Entities.Vehicle(VehicleType.Car),
            CancellationToken.None);

        Assert.AreEqual(0, result.Fee);
    }

    [TestMethod]
    public async Task GetFee_OrdersInput_AndStillComputesCorrectly()
    {
        var svc = new TollFeeService(
            CreateGothenburgSchedule(),
            new FakeCalendarService((_, __) => Task.FromResult(false)),
            new FakeVehicleService(_ => false),
            Options.Create(new TollFeeOptions { ChargeWindowMinutes =60, MaxDailyFee =60 }));

        // Unsorted input, same day.
        var dateTimes = new[]
        {
            new DateTime(2026,1,2,8,40,0), //8
            new DateTime(2026,1,2,6,15,0), //8
        };

        var result = await svc.GetFee(dateTimes, new TollFeeCalculator.Enterprise.Entities.Vehicle(VehicleType.Car), CancellationToken.None);

        Assert.AreEqual(16, result.Fee);
    }

    [TestMethod]
    public async Task GetFee_MaxDailyFeeBoundary_ExactEquals_NotTruncatedFurther()
    {
        var svc = new TollFeeService(
            CreateGothenburgSchedule(),
            new FakeCalendarService((_, __) => Task.FromResult(false)),
            new FakeVehicleService(_ => false),
            Options.Create(new TollFeeOptions { ChargeWindowMinutes =60, MaxDailyFee =26 }));

        // 6:15 (8) + 7:10 (18) within 55 mins => same window => 18) => total 18
        // add 9:00 (8) => new window => total 26.
        var dateTimes = new[]
        {
            new DateTime(2026,1,2,6,15,0),
            new DateTime(2026,1,2,7,10,0),
            new DateTime(2026,1,2,9,0,0),
        };

        var result = await svc.GetFee(dateTimes, new TollFeeCalculator.Enterprise.Entities.Vehicle(VehicleType.Car), CancellationToken.None);

        Assert.AreEqual(26, result.Fee);
    }

    [TestMethod]
    public async Task GetFee_PreCancelledToken_Throws_BeforeAnyCalendarCall()
    {
        var calendarCalls =0;

        var svc = new TollFeeService(
            CreateGothenburgSchedule(),
            new FakeCalendarService((_, __) =>
            {
                Interlocked.Increment(ref calendarCalls);
                return Task.FromResult(false);
            }),
            new FakeVehicleService(_ => false),
            Options.Create(new TollFeeOptions()));

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsExceptionAsync<OperationCanceledException>(() =>
            svc.GetFee(
                new[] { new DateTime(2026,1,2,7,0,0) },
                new TollFeeCalculator.Enterprise.Entities.Vehicle(VehicleType.Car),
                cts.Token));

        Assert.AreEqual(0, calendarCalls);
    }

    [TestMethod]
    public async Task GetFee_CancellationDuringDayIteration_Throws()
    {
        using var cts = new CancellationTokenSource();

        var callCount =0;

        var svc = new TollFeeService(
            CreateGothenburgSchedule(),
            new FakeCalendarService((_, ct) =>
            {
                // cancel after the first day check so cancellation is observed on the next loop iteration via ThrowIfCancellationRequested.
                if (Interlocked.Increment(ref callCount) ==1)
                {
                    cts.Cancel();
                }

                ct.ThrowIfCancellationRequested();
                return Task.FromResult(false);
            }),
            new FakeVehicleService(_ => false),
            Options.Create(new TollFeeOptions()));

        var dateTimes = new[]
        {
            new DateTime(2026,1,2,7,0,0),
            new DateTime(2026,1,3,7,0,0),
        };

        await Assert.ThrowsExceptionAsync<OperationCanceledException>(() =>
            svc.GetFee(dateTimes, new TollFeeCalculator.Enterprise.Entities.Vehicle(VehicleType.Car), cts.Token));

        Assert.AreEqual(1, callCount);
    }
}