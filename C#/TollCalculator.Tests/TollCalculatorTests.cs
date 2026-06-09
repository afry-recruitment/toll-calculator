using TollFeeCalculator;
using Xunit;

namespace TollCalculator.Tests;

public class TollCalculatorTests
{
    // 2013-03-01 is a Friday and not in the 2013 holiday list, so it is a safe
    // non-free weekday for time-of-day tests.
    private static DateTime At(int hour, int minute, int second = 0) =>
        new(2013, 3, 1, hour, minute, second);

    private readonly TollFeeCalculator.TollCalculator calculator = new();

    [Theory]
    [InlineData(5, 30, 0)]
    [InlineData(6, 0, 8)]
    [InlineData(6, 29, 8)]
    [InlineData(6, 30, 13)]
    [InlineData(6, 59, 13)]
    [InlineData(7, 0, 18)]
    [InlineData(7, 59, 18)]
    [InlineData(8, 0, 13)]
    [InlineData(8, 29, 13)]
    [InlineData(8, 30, 8)]
    [InlineData(9, 15, 8)]
    [InlineData(12, 0, 8)]
    [InlineData(14, 59, 8)]
    [InlineData(15, 0, 13)]
    [InlineData(15, 29, 13)]
    [InlineData(15, 30, 18)]
    [InlineData(16, 59, 18)]
    [InlineData(17, 0, 13)]
    [InlineData(17, 59, 13)]
    [InlineData(18, 0, 8)]
    [InlineData(18, 29, 8)]
    [InlineData(18, 30, 0)]
    [InlineData(23, 59, 0)]
    public void SinglePass_ReturnsFeeForTimeOfDay(int hour, int minute, int expectedFee)
    {
        Assert.Equal(expectedFee, calculator.GetTollFee(At(hour, minute), new Car()));
    }

    [Fact]
    public void RushHours_ReturnHighestFee()
    {
        Assert.Equal(18, calculator.GetTollFee(At(7, 30), new Car()));
        Assert.Equal(18, calculator.GetTollFee(At(16, 0), new Car()));
    }

    [Fact]
    public void Motorbike_IsAlwaysFree()
    {
        Assert.Equal(0, calculator.GetTollFee(At(7, 30), new Motorbike()));
        Assert.Equal(0, calculator.GetTollFee(new Motorbike(), new[] { At(7, 30), At(16, 0) }));
    }

    [Theory]
    [InlineData("Motorbike")]
    [InlineData("Tractor")]
    [InlineData("Emergency")]
    [InlineData("Diplomat")]
    [InlineData("Foreign")]
    [InlineData("Military")]
    public void TollFreeVehicleTypes_AreFree(string type)
    {
        var vehicle = new TollFreeVehicle(type);
        Assert.Equal(0, calculator.GetTollFee(At(7, 30), vehicle));
        Assert.Equal(0, calculator.GetTollFee(vehicle, new[] { At(7, 30), At(16, 0) }));
    }

    [Fact]
    public void TollFreeVehicleType_IsCaseInsensitive()
    {
        Assert.Equal(0, calculator.GetTollFee(At(7, 30), new TollFreeVehicle("EMERGENCY")));
    }

    [Fact]
    public void Saturday_IsFree()
    {
        var saturday = new DateTime(2013, 3, 2, 7, 30, 0);
        Assert.Equal(DayOfWeek.Saturday, saturday.DayOfWeek);
        Assert.Equal(0, calculator.GetTollFee(saturday, new Car()));
    }

    [Fact]
    public void Sunday_IsFree()
    {
        var sunday = new DateTime(2013, 3, 3, 7, 30, 0);
        Assert.Equal(DayOfWeek.Sunday, sunday.DayOfWeek);
        Assert.Equal(0, calculator.GetTollFee(sunday, new Car()));
    }

    [Theory]
    [InlineData(2013, 1, 1)]
    [InlineData(2013, 3, 28)]
    [InlineData(2013, 3, 29)]
    [InlineData(2013, 4, 1)]
    [InlineData(2013, 4, 30)]
    [InlineData(2013, 5, 1)]
    [InlineData(2013, 5, 8)]
    [InlineData(2013, 5, 9)]
    [InlineData(2013, 6, 5)]
    [InlineData(2013, 6, 6)]
    [InlineData(2013, 6, 21)]
    [InlineData(2013, 7, 15)]
    [InlineData(2013, 11, 1)]
    [InlineData(2013, 12, 24)]
    [InlineData(2013, 12, 25)]
    [InlineData(2013, 12, 26)]
    [InlineData(2013, 12, 31)]
    public void Holidays2013_AreFree(int year, int month, int day)
    {
        var holiday = new DateTime(year, month, day, 7, 30, 0);
        Assert.Equal(0, calculator.GetTollFee(holiday, new Car()));
    }

    [Fact]
    public void NonHolidayWeekday_IsCharged()
    {
        Assert.Equal(18, calculator.GetTollFee(At(7, 30), new Car()));
    }

    [Fact]
    public void TwoPassesWithinHour_ChargeHighest()
    {
        // 06:15 (8 SEK) and 07:00 (18 SEK), 45 min apart.
        var passes = new[] { At(6, 15), At(7, 0) };
        Assert.Equal(18, calculator.GetTollFee(new Car(), passes));
    }

    [Fact]
    public void ThreePassesWithinHour_ChargeHighest()
    {
        // All within 60 minutes of the first pass.
        var passes = new[] { At(6, 15), At(6, 45), At(7, 10) };
        Assert.Equal(18, calculator.GetTollFee(new Car(), passes));
    }

    [Fact]
    public void PassesMoreThanHourApart_AreChargedSeparately()
    {
        // 07:00 (18 SEK) and 08:30 (8 SEK), 90 min apart.
        var passes = new[] { At(7, 0), At(8, 30) };
        Assert.Equal(18 + 8, calculator.GetTollFee(new Car(), passes));
    }

    [Fact]
    public void RollingWindow_ResetsAfterFirstIntervalEnds()
    {
        // 06:00 (8) + 06:30 (13) in window 1 -> 13
        // 07:30 (18) starts window 2 (90 min from 06:00) -> 18
        // 09:00 (8)  starts window 3 (90 min from 07:30) -> 8
        var passes = new[] { At(6, 0), At(6, 30), At(7, 30), At(9, 0) };
        Assert.Equal(13 + 18 + 8, calculator.GetTollFee(new Car(), passes));
    }

    [Fact]
    public void DailyTotal_IsCappedAt60Sek()
    {
        // 7 passes spaced just over an hour apart accumulate well above 60 SEK
        // before the cap is applied.
        var passes = Enumerable.Range(0, 7)
            .Select(i => At(6, 30).AddMinutes(i * 61))
            .ToArray();
        Assert.Equal(60, calculator.GetTollFee(new Car(), passes));
    }

    [Fact]
    public void EmptyDates_ReturnsZero()
    {
        Assert.Equal(0, calculator.GetTollFee(new Car(), Array.Empty<DateTime>()));
    }

    [Fact]
    public void NullDates_ReturnsZero()
    {
        Assert.Equal(0, calculator.GetTollFee(new Car(), null));
    }

    [Fact]
    public void NullVehicle_ReturnsZero()
    {
        Assert.Equal(0, calculator.GetTollFee(null, new[] { At(7, 30) }));
        Assert.Equal(0, calculator.GetTollFee(At(7, 30), null));
    }

    [Fact]
    public void SinglePass_IsHandled()
    {
        Assert.Equal(18, calculator.GetTollFee(new Car(), new[] { At(7, 30) }));
    }

    [Fact]
    public void UnsortedPasses_AreNormalised()
    {
        // Sorted: 06:00 (8), 07:00 (18), 09:00 (8).
        // Window 1 covers 06:00-07:00 (exactly 60 min) -> max 18.
        // 09:00 is 180 min from 06:00, so it starts its own window -> 8.
        var passes = new[] { At(9, 0), At(6, 0), At(7, 0) };
        Assert.Equal(18 + 8, calculator.GetTollFee(new Car(), passes));
    }

    [Fact]
    public void HolidayCalendarIsScopedTo2013()
    {
        // 2014-01-01 is a Wednesday and is intentionally treated as a normal
        // workday because the 2013-only holiday calendar does not cover it.
        // This documents the kata's known limitation.
        var date = new DateTime(2014, 1, 1, 7, 30, 0);
        Assert.Equal(DayOfWeek.Wednesday, date.DayOfWeek);
        Assert.Equal(18, calculator.GetTollFee(date, new Car()));
    }
}
