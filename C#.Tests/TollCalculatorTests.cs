using System;
using TollFeeCalculator.Models;
using TollFeeCalculator.Services;
using Xunit;

namespace TollFeeCalculator.Tests;

public class TollCalculatorTests
{
    private readonly TollCalculator _calc = new();

    // Base day used for the chargeable-time tests: 2024-02-13 is a Tuesday,
    // clear of weekends, July and Swedish public holidays.
    private static DateTime At(int hour, int minute) =>
        new DateTime(2024, 2, 13, hour, minute, 0);

    private static int Fee(TollCalculator calc, IVehicle vehicle, params DateTime[] dates) =>
        calc.GetTollFee(vehicle, dates);

    // ---- Fee table, including the boundary minutes ----

    [Theory]
    [InlineData(5, 59, 0)]   // before the first charged window
    [InlineData(6, 0, 8)]
    [InlineData(6, 29, 8)]
    [InlineData(6, 30, 13)]
    [InlineData(6, 59, 13)]
    [InlineData(7, 0, 18)]
    [InlineData(7, 59, 18)]
    [InlineData(8, 0, 13)]
    [InlineData(8, 29, 13)]
    [InlineData(8, 30, 8)]
    [InlineData(14, 59, 8)]
    [InlineData(15, 0, 13)]
    [InlineData(15, 29, 13)]
    [InlineData(15, 30, 18)]
    [InlineData(16, 59, 18)]
    [InlineData(17, 0, 13)]
    [InlineData(17, 59, 13)]
    [InlineData(18, 0, 8)]
    [InlineData(18, 29, 8)]
    [InlineData(18, 30, 0)]   // after the last charged window
    [InlineData(22, 0, 0)]    // night
    public void Single_passage_is_charged_per_the_fee_table(int hour, int minute, int expected)
    {
        Assert.Equal(expected, Fee(_calc, new Car(), At(hour, minute)));
    }

    // ---- "Once an hour": highest fee within the window applies ----

    [Fact]
    public void Passages_within_the_same_hour_charge_only_the_highest()
    {
        // 06:00 (8) and 06:30 (13), 30 minutes apart -> highest = 13.
        Assert.Equal(13, Fee(_calc, new Car(), At(6, 0), At(6, 30)));
    }

    [Fact]
    public void Passages_exactly_60_minutes_apart_count_as_one_window()
    {
        // 06:00 (8) and 07:00 (18), exactly 60 min apart -> still one window, max = 18.
        Assert.Equal(18, Fee(_calc, new Car(), At(6, 0), At(7, 0)));
    }

    [Fact]
    public void Passages_more_than_60_minutes_apart_are_charged_separately()
    {
        // 06:00 (8) and 07:01 (18), 61 min apart -> two windows: 8 + 18 = 26.
        Assert.Equal(26, Fee(_calc, new Car(), At(6, 0), At(7, 1)));
    }

    // ---- Daily cap ----

    [Fact]
    public void Total_fee_is_capped_at_60()
    {
        // Six passages, each more than 60 min apart, sum to 73 -> capped at 60.
        int total = Fee(_calc, new Car(),
            At(6, 0),    // 8
            At(7, 30),   // 18
            At(9, 0),    // 8
            At(15, 0),   // 13
            At(16, 30),  // 18
            At(18, 0));  // 8  => 73 raw
        Assert.Equal(60, total);
    }

    // ---- Toll-free vehicles ----

    [Fact]
    public void Toll_free_vehicle_is_never_charged()
    {
        Assert.Equal(0, Fee(_calc, new Motorbike(), At(7, 0)));
    }

    [Theory]
    [MemberData(nameof(TollFreeVehicles))]
    public void All_toll_free_vehicle_types_pay_nothing(IVehicle vehicle)
    {
        Assert.Equal(0, Fee(_calc, vehicle, At(7, 0), At(15, 30)));
    }

    public static TheoryData<IVehicle> TollFreeVehicles() => new()
    {
        new Motorbike(),
        new Tractor(),
        new Emergency(),
        new Diplomat(),
        new Foreign(),
        new Military(),
    };

    [Fact]
    public void Toll_free_vehicle_short_circuits_before_validation()
    {
        // By design the toll-free check runs before date validation, so an
        // otherwise-invalid multi-day input returns 0 rather than throwing.
        var multiDay = new[] { At(7, 0), new DateTime(2024, 2, 14, 7, 0, 0) };
        Assert.Equal(0, _calc.GetTollFee(new Motorbike(), multiDay));
    }

    // ---- Toll-free dates ----

    [Fact]
    public void Weekends_are_free()
    {
        // 2024-02-17 is a Saturday.
        Assert.Equal(0, _calc.GetTollFee(new Car(), new[] { new DateTime(2024, 2, 17, 7, 0, 0) }));
    }

    [Fact]
    public void July_is_free()
    {
        Assert.Equal(0, _calc.GetTollFee(new Car(), new[] { new DateTime(2024, 7, 9, 7, 0, 0) }));
    }

    [Fact]
    public void Public_holidays_are_free()
    {
        // New Year's Day.
        Assert.Equal(0, _calc.GetTollFee(new Car(), new[] { new DateTime(2024, 1, 1, 7, 0, 0) }));
    }

    [Fact]
    public void Day_before_a_public_holiday_is_free()
    {
        // 2024-12-24, the day before Christmas Day.
        Assert.Equal(0, _calc.GetTollFee(new Car(), new[] { new DateTime(2024, 12, 24, 7, 0, 0) }));
    }

    // ---- Input handling ----

    [Fact]
    public void Empty_input_returns_zero()
    {
        Assert.Equal(0, _calc.GetTollFee(new Car(), Array.Empty<DateTime>()));
    }

    [Fact]
    public void Null_dates_returns_zero()
    {
        Assert.Equal(0, _calc.GetTollFee(new Car(), null!));
    }

    [Fact]
    public void Null_vehicle_throws()
    {
        Assert.Throws<ArgumentNullException>(() => _calc.GetTollFee(null!, new[] { At(7, 0) }));
    }

    [Fact]
    public void Dates_before_2013_are_rejected()
    {
        Assert.Throws<ArgumentException>(
            () => _calc.GetTollFee(new Car(), new[] { new DateTime(2012, 6, 1, 7, 0, 0) }));
    }

    [Fact]
    public void Dates_spanning_multiple_days_are_rejected()
    {
        var twoDays = new[] { At(7, 0), new DateTime(2024, 2, 14, 7, 0, 0) };
        Assert.Throws<ArgumentException>(() => _calc.GetTollFee(new Car(), twoDays));
    }

    [Fact]
    public void Unsorted_input_yields_the_same_result_as_sorted()
    {
        int sorted = Fee(_calc, new Car(), At(6, 0), At(7, 1));
        int unsorted = Fee(_calc, new Car(), At(7, 1), At(6, 0));
        Assert.Equal(sorted, unsorted);
        Assert.Equal(26, unsorted);
    }

    [Fact]
    public void Input_array_is_not_mutated()
    {
        var dates = new[] { At(7, 0), At(6, 0) };
        var original = (DateTime[])dates.Clone();
        _calc.GetTollFee(new Car(), dates);
        Assert.Equal(original, dates);
    }
}
