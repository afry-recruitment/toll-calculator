using TollCalculator.Models;
using TollCalculator.Services.Policies;

namespace TollCalculator.Tests;

public class TollCalculatorOrchestrationTests
{
    private static TollCalculator.Services.TollCalculator CreateSut(
        int fixedFee = 10,
        bool tollFreeVehicle = false,
        Func<DateTime, bool>? tollFreeDate = null)
    {
        return new TollCalculator.Services.TollCalculator(
            new FakeFeeSchedule(fixedFee),
            new FakeTollFreeVehiclePolicy(tollFreeVehicle),
            new FakeTollFreeDatePolicy(tollFreeDate ?? (_ => false)));
    }

    [Fact]
    public void GetTollFee_EmptyDates_ReturnsZero()
    {
        var sut = CreateSut();
        var result = sut.GetTollFee(new Car(), Array.Empty<DateTime>());
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetTollFee_TollFreeVehicle_ReturnsZero()
    {
        var sut = CreateSut(tollFreeVehicle: true);
        var result = sut.GetTollFee(new Car(), [new DateTime(2013, 1, 2, 7, 0, 0)]);
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetSinglePassFee_TollFreeDate_ReturnsZero()
    {
        var sut = CreateSut(tollFreeDate: d => d.Date == new DateTime(2013, 1, 1));
        var result = sut.GetSinglePassFee(new DateTime(2013, 1, 1, 7, 0, 0), new Car());
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetTollFee_Same60MinuteWindow_UsesHighestFeeOnly()
    {
        var sut = new TollCalculator.Services.TollCalculator(
            new IncreasingFeeSchedule(),
            new FakeTollFreeVehiclePolicy(false),
            new FakeTollFreeDatePolicy(_ => false));

        var dates = new[]
        {
            new DateTime(2013, 1, 2, 6, 00, 0),
            new DateTime(2013, 1, 2, 6, 20, 0),
            new DateTime(2013, 1, 2, 6, 50, 0)
        };

        var result = sut.GetTollFee(new Car(), dates);
        Assert.Equal(30, result); // max in first 60-min window from IncreasingFeeSchedule
    }

    [Fact]
    public void GetTollFee_AppliesDailyCap60()
    {
        var sut = CreateSut(fixedFee: 20);

        var dates = new[]
        {
            new DateTime(2013, 1, 2, 6, 00, 0),
            new DateTime(2013, 1, 2, 7, 10, 0),
            new DateTime(2013, 1, 2, 8, 20, 0),
            new DateTime(2013, 1, 2, 9, 30, 0)
        };

        var result = sut.GetTollFee(new Car(), dates);
        Assert.Equal(60, result);
    }

    private sealed class FakeFeeSchedule(int fee) : ITollFeeSchedule
    {
        public int GetFee(DateTime dateTime) => fee;
    }

    private sealed class IncreasingFeeSchedule : ITollFeeSchedule
    {
        public int GetFee(DateTime dateTime)
        {
            if (dateTime.Minute < 10) return 10;
            if (dateTime.Minute < 40) return 20;
            return 30;
        }
    }

    private sealed class FakeTollFreeVehiclePolicy(bool isTollFree) : ITollFreeVehiclePolicy
    {
        public bool IsTollFree(IVehicle vehicle) => isTollFree;
    }

    private sealed class FakeTollFreeDatePolicy(Func<DateTime, bool> predicate) : ITollFreeDatePolicy
    {
        public bool IsTollFree(DateTime date) => predicate(date);
    }
}