using System;

namespace TollFeeCalculator.Tests;

/// <summary>
/// The public contract of <see cref="TollCalculator.GetTollFee(Vehicle, DateTime[])"/>:
/// what it does to the input, and how it rejects invalid input.
/// </summary>
public class InputContractTests
{
    private readonly TollCalculator _calculator = new();

    [Fact]
    public void Does_not_modify_the_caller_array()
    {
        DateTime[] passes = { Days.Tue(8), Days.Tue(6, 15) };
        DateTime[] before = (DateTime[])passes.Clone();

        _ = _calculator.GetTollFee(new Car(), passes);

        Assert.Equal(before, passes);
    }

    [Fact]
    public void Result_is_independent_of_input_order()
    {
        int sorted = _calculator.GetTollFee(new Car(), new[] { Days.Tue(6, 15), Days.Tue(8) });
        int shuffled = _calculator.GetTollFee(new Car(), new[] { Days.Tue(8), Days.Tue(6, 15) });

        Assert.Equal(sorted, shuffled);
    }

    [Fact]
    public void Repeated_calls_on_the_same_array_are_stable()
    {
        DateTime[] passes = { Days.Tue(6, 15), Days.Tue(6, 45) };

        int first = _calculator.GetTollFee(new Car(), passes);
        int second = _calculator.GetTollFee(new Car(), passes);

        Assert.Equal(first, second);
    }

    [Fact]
    public void Null_vehicle_throws()
    {
        Assert.Throws<ArgumentNullException>(() => _calculator.GetTollFee(null!, new[] { Days.Tue(7) }));
    }

    [Fact]
    public void Null_dates_throws()
    {
        Assert.Throws<ArgumentNullException>(() => _calculator.GetTollFee(new Car(), null!));
    }

    [Fact]
    public void Empty_dates_throws()
    {
        Assert.Throws<ArgumentException>(() => _calculator.GetTollFee(new Car(), Array.Empty<DateTime>()));
    }

    [Fact]
    public void Dates_on_different_days_throws()
    {
        Assert.Throws<ArgumentException>(() =>
            _calculator.GetTollFee(new Car(), new[] { Days.Tue(7), Days.Tue(7).AddDays(1) }));
    }
}
