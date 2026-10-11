using System;

namespace TollFeeCalculator.Tests;

/// <summary>
/// Rules: "Some vehicle types are fee-free" and "Weekends and holidays are
/// fee-free."
/// </summary>
public class TollFreeTests
{
    private readonly TollCalculator _calculator = new();

    [Theory]
    [InlineData("Motorbike")]
    [InlineData("Tractor")]
    [InlineData("Emergency")]
    [InlineData("Diplomat")]
    [InlineData("Foreign")]
    [InlineData("Military")]
    public void Toll_free_vehicle_types_are_never_charged(string vehicleType)
    {
        int fee = _calculator.GetTollFee(new NamedVehicle(vehicleType), new[] { Days.Tue(7) });

        Assert.Equal(0, fee);
    }

    [Fact]
    public void Concrete_motorbike_is_free()
    {
        Assert.Equal(0, _calculator.GetTollFee(new Motorbike(), new[] { Days.Tue(7) }));
    }

    [Fact]
    public void Car_is_charged()
    {
        Assert.Equal(18, _calculator.GetTollFee(new Car(), new[] { Days.Tue(7) }));
    }

    [Theory]
    [InlineData(2013, 1, 5)]  // Saturday
    [InlineData(2013, 1, 6)]  // Sunday
    public void Weekends_are_free(int year, int month, int day)
    {
        var date = new DateTime(year, month, day, 7, 0, 0);

        Assert.Equal(0, _calculator.GetTollFee(new Car(), new[] { date }));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(3, 28)]
    [InlineData(3, 29)]
    [InlineData(4, 1)]
    [InlineData(4, 30)]
    [InlineData(5, 1)]
    [InlineData(5, 8)]
    [InlineData(5, 9)]
    [InlineData(6, 5)]
    [InlineData(6, 6)]
    [InlineData(6, 21)]
    [InlineData(11, 1)]
    [InlineData(12, 24)]
    [InlineData(12, 25)]
    [InlineData(12, 26)]
    [InlineData(12, 31)]
    public void Public_holidays_in_2013_are_free(int month, int day)
    {
        var holiday = new DateTime(2013, month, day, 7, 0, 0);

        Assert.Equal(0, _calculator.GetTollFee(new Car(), new[] { holiday }));
    }

    [Fact]
    public void July_is_free_regardless_of_year()
    {
        var july2013 = new DateTime(2013, 7, 10, 7, 0, 0);
        var july2026 = new DateTime(2026, 7, 10, 7, 0, 0);

        Assert.Equal(0, _calculator.GetTollFee(new Car(), new[] { july2013 }));
        Assert.Equal(0, _calculator.GetTollFee(new Car(), new[] { july2026 }));
    }

    [Fact]
    public void Ordinary_weekday_is_charged()
    {
        var ordinaryTuesday = new DateTime(2013, 1, 8, 7, 0, 0);

        Assert.Equal(18, _calculator.GetTollFee(new Car(), new[] { ordinaryTuesday }));
    }

    [Fact]
    public void Year_without_holiday_data_falls_back_to_chargeable()
    {
        // Documents the known limitation: only 2013 holidays are encoded, so a
        // holiday in an unencoded year is (incorrectly) treated as chargeable.
        var christmas2014 = new DateTime(2014, 12, 25, 7, 0, 0);

        Assert.Equal(18, _calculator.GetTollFee(new Car(), new[] { christmas2014 }));
    }
}
