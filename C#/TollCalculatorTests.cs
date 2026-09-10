using System;
using Xunit;
using TollFeeCalculator;

namespace TollCalculatorTests;

public class TollCalculatorTests
{
    // Monday, Feb 4 2013 -- an ordinary weekday, not a toll-free holiday.
    private static DateTime Weekday(int hour, int minute) =>
        new DateTime(2013, 2, 4, hour, minute, 0);

    // ---------- Single-pass fee: time-of-day brackets ----------

    [Theory]
    [InlineData(5, 59, 0)]   // before 6:00 -> free
    [InlineData(6, 0, 8)]
    [InlineData(6, 29, 8)]
    [InlineData(6, 30, 13)]
    [InlineData(7, 0, 18)]
    [InlineData(7, 59, 18)]
    [InlineData(8, 0, 13)]
    [InlineData(8, 29, 13)]
    [InlineData(8, 30, 8)]
    [InlineData(12, 0, 8)]
    [InlineData(15, 0, 13)]
    [InlineData(15, 29, 13)]
    [InlineData(15, 30, 18)]
    [InlineData(16, 59, 18)]
    [InlineData(17, 0, 13)]
    [InlineData(17, 59, 13)]
    [InlineData(18, 0, 8)]
    [InlineData(18, 29, 8)]
    [InlineData(18, 30, 0)]  // 18:30+ -> free
    [InlineData(23, 0, 0)]
    public void GetTollFee_TimeOfDayBrackets_ReturnsExpectedFee(int hour, int minute, int expectedFee)
    {
        var date = Weekday(hour, minute);

        var fee = TollCalculator.GetTollFee(date, new Car());

        Assert.Equal(expectedFee, fee);
    }

    // ---------- Toll-free vehicles ----------

    [Theory]
    [InlineData(TollCalculator.VehicleType.Motorbike)]
    [InlineData(TollCalculator.VehicleType.Emergency)]
    [InlineData(TollCalculator.VehicleType.Foreign)]
    public void GetTollFee_TollFreeVehicleTypes_ReturnsZeroEvenAtPeakTime(TollCalculator.VehicleType vehicleType)
    {
        var date = Weekday(7, 0); // would be 18 for a normal vehicle

        var fee = TollCalculator.GetTollFee(date, new VehicleWithType(vehicleType));

        Assert.Equal(0, fee);
    }

    [Fact]
    public void GetTollFee_NullVehicle_IsTreatedAsNormalFeePayingVehicle()
    {
        // IsTollFreeVehicle returns false for null, so a null vehicle
        // is NOT exempt -- it should be charged the normal fee.
        var date = Weekday(7, 0);

        var fee = TollCalculator.GetTollFee(date, null);

        Assert.Equal(18, fee);
    }

    // ---------- Toll-free dates ----------

    [Fact]
    public void GetTollFee_Saturday_ReturnsZero()
    {
        var date = new DateTime(2013, 2, 9, 8, 0, 0); // Saturday

        Assert.Equal(0, TollCalculator.GetTollFee(date, new Car()));
    }

    [Fact]
    public void GetTollFee_Sunday_ReturnsZero()
    {
        var date = new DateTime(2013, 2, 10, 8, 0, 0); // Sunday

        Assert.Equal(0, TollCalculator.GetTollFee(date, new Car()));
    }

    [Fact]
    public void GetTollFee_NewYearsDay2013_ReturnsZero()
    {
        var date = new DateTime(2013, 1, 1, 8, 0, 0);

        Assert.Equal(0, TollCalculator.GetTollFee(date, new Car()));
    }

    [Fact]
    public void GetTollFee_AnyDayInJuly2013_ReturnsZero()
    {
        var date = new DateTime(2013, 7, 15, 8, 0, 0); // ordinary Monday in July

        Assert.Equal(0, TollCalculator.GetTollFee(date, new Car()));
    }

    [Fact]
    public void GetTollFee_ChristmasEve2013_ReturnsZero()
    {
        var date = new DateTime(2013, 12, 24, 8, 0, 0);

        Assert.Equal(0, TollCalculator.GetTollFee(date, new Car()));
    }

    [Fact]
    public void GetTollFee_SameCalendarDateOutside2013_IsNotTreatedAsHoliday()
    {
        // IsTollFreeDate only has a special-case table for year == 2013.
        // A weekday holiday (e.g. New Year's Day) in a different year
        // is NOT exempted under the current implementation. Documenting
        // this in case it's an oversight rather than intended behavior.
        var date = new DateTime(2014, 1, 1, 8, 0, 0); // Wednesday

        Assert.Equal(13, TollCalculator.GetTollFee(date, new Car()));
    }

    // ---------- Daily total: GetTollFee(Vehicle, DateTime[]) ----------
    //
    // This groups passes into rolling 60-minute windows and charges the
    // highest single fee within each window, summed across windows and
    // capped at 60 for the day.

    [Fact]
    public void GetTollFee_SingleDate_ReturnsItsOwnFee()
    {
        var dates = new[] { Weekday(7, 0) }; // fee 18

        var totalFee = TollCalculator.GetTollFee(new Car(), dates);

        Assert.Equal(18, totalFee);
    }

    [Fact]
    public void GetTollFee_PassesInThreeSeparateWindows_SumsEachWindowsMax()
    {
        var dates = new[]
        {
            Weekday(7, 0),   // window 1 -> 18
            Weekday(8, 0),   // window 2 (exactly 60 min later) -> 13
            Weekday(15, 30), // window 3 -> 18
        };

        var totalFee = TollCalculator.GetTollFee(new Car(), dates);

        Assert.Equal(49, totalFee); // 18 + 13 + 18
    }

    [Fact]
    public void GetTollFee_TwoPassesWithinSameHour_ChargesOnlyTheHigherFee()
    {
        var dates = new[]
        {
            Weekday(6, 20), // 8
            Weekday(6, 50), // 13 -- same 60-min window, and the higher of the two
        };

        var totalFee = TollCalculator.GetTollFee(new Car(), dates);

        Assert.Equal(13, totalFee);
    }

    [Fact]
    public void GetTollFee_HighestFeeEarlierInWindow_StillWins()
    {
        var dates = new[]
        {
            Weekday(7, 45), // 18 -- the window's max, even though it's not last
            Weekday(8, 15), // 13 -- same window
        };

        var totalFee = TollCalculator.GetTollFee(new Car(), dates);

        Assert.Equal(18, totalFee);
    }

    [Fact]
    public void GetTollFee_ManyPassesAcrossDay_IsCappedAtSixty()
    {
        var dates = new[]
        {
            Weekday(6, 0),   // window -> 8
            Weekday(7, 30),  // window -> 18
            Weekday(9, 0),   // window -> 8
            Weekday(10, 30), // window -> 8
            Weekday(12, 0),  // window -> 8
            Weekday(13, 30), // window -> 8
            Weekday(15, 0),  // window -> 13
            Weekday(16, 30), // window -> 18
        };
        // Sum of window maxes = 8+18+8+8+8+8+13+18 = 89, capped at 60.

        var totalFee = TollCalculator.GetTollFee(new Car(), dates);

        Assert.Equal(TollCalculator.MAX_DAY_FEE, totalFee);
    }

    [Fact]
    public void GetTollFee_UnsortedDates_AreSortedBeforeCalculating()
    {
        var dates = new[]
        {
            Weekday(15, 30), // deliberately out of order
            Weekday(7, 0),
            Weekday(8, 0),
        };

        var totalFee = TollCalculator.GetTollFee(new Car(), dates);

        Assert.Equal(49, totalFee); // 18 + 13 + 18, same as the sorted-order case
    }

    [Fact]
    public void GetTollFee_EmptyDates_ReturnsZero()
    {
        var dates = Array.Empty<DateTime>();

        var totalFee = TollCalculator.GetTollFee(new Car(), dates);

        Assert.Equal(0, totalFee);
    }

    [Fact]
    public void GetTollFee_TollFreeVehicleAcrossMultipleDates_ReturnsZero()
    {
        var vehicle = new Motorbike();
        var dates = new[] { Weekday(7, 0), Weekday(16, 0) };

        var totalFee = TollCalculator.GetTollFee(vehicle, dates);

        Assert.Equal(0, totalFee);
    }

    [Fact]
    public void GetTollFee_AllPassesOnWeekend_ReturnsZero()
    {
        var dates = new[]
        {
            new DateTime(2013, 2, 9, 8, 0, 0),  // Saturday
            new DateTime(2013, 2, 9, 17, 0, 0), // Saturday
        };

        var totalFee = TollCalculator.GetTollFee(new Car(), dates);

        Assert.Equal(0, totalFee);
    }
}