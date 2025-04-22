using Xunit;
using Xunit.Abstractions;

namespace TollFeeCalculator;

public class TollCalculatorTests(ITestOutputHelper testOutput)
{
    [Fact]
    public void Motorbikes_are_toll_free()
    {
        Assert.True(TollCalculator.IsTollFreeVehicle(new Motorbike()));
    }

    [Fact]
    public void Cars_are_not_toll_free()
    {
        Assert.False(TollCalculator.IsTollFreeVehicle(new Car()));
    }

    [Fact]
    public void Null_values_do_not_crash_the_toll_free_check()
    {
        TollCalculator.IsTollFreeVehicle(null);
    }

    public static List<object[]> NonTollFreeDays =>
    [
        [new DateTime(2025, 3, 31)], // The week starting on 2025-03-31 includes no holidays, so it should be five days of tolls.
        [new DateTime(2025, 4, 1)],
        [new DateTime(2025, 4, 2)],
        [new DateTime(2025, 4, 3)],
        [new DateTime(2025, 4, 4)],
    ];

    [Theory]
    [MemberData(nameof(NonTollFreeDays))]
    public void Normal_weekdays_are_not_toll_free(DateTime date)
    {
        Assert.False(TollCalculator.IsTollFreeDate(date));
    }

    public static List<object[]> TollFreeDays =>
    [
        [new DateTime(2025, 4, 5)], // Saturday
        [new DateTime(2025, 4, 6)], // Sunday
    ];

    [Theory]
    [MemberData(nameof(TollFreeDays))]
    public void Weekends_are_toll_free(DateTime date)
    {
        Assert.True(TollCalculator.IsTollFreeDate(date));
    }

    public static List<object[]> PublicHolidays =>
    [
        [new DateTime(2025, 4, 18)], // 2025 Good Friday
        [new DateTime(2025, 4, 21)], // 2025 Easter Monday
        [new DateTime(2025, 6, 6)],  // 2025 Sweden Nationaldag
    ];

    [Theory(Skip = "Not implemented yet, see ticket #123456")]
    [MemberData(nameof(PublicHolidays))]
    public void Public_holidays_are_toll_free(DateTime date)
    {
        Assert.True(TollCalculator.IsTollFreeDate(date));
    }
}