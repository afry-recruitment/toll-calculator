using Xunit;

namespace TollFeeCalculator.Tests;

public class TariffTests
{
    private readonly TollCalculator calculator = new();
    private const VehicleType car = VehicleType.Car;
    private static DateTime At(int hour, int minute = 0) => new(2013, 2, 4, hour, minute, 0);

    public static IEnumerable<object[]> RateBoundaries()
    {
        yield return new object[] { 6, 0, 0, 8 };
        yield return new object[] { 6, 30, 8, 13 };
        yield return new object[] { 7, 0, 13, 18 };
        yield return new object[] { 8, 0, 18, 13 };
        yield return new object[] { 8, 30, 13, 8 };
        yield return new object[] { 15, 0, 8, 13 };
        yield return new object[] { 15, 30, 13, 18 };
        yield return new object[] { 17, 0, 18, 13 };
        yield return new object[] { 18, 0, 13, 8 };
        yield return new object[] { 18, 30, 8, 0 };
    }

    [Theory]
    [MemberData(nameof(RateBoundaries))]
    public void Rate_changes_at_exact_boundary(int hour, int minute, int before, int after)
    {
        DateTime boundary = At(hour, minute);
        Assert.Equal(before, calculator.GetPassageFee(car, boundary.AddTicks(-1)));
        Assert.Equal(after, calculator.GetPassageFee(car, boundary));
        Assert.Equal(after, calculator.GetPassageFee(car, boundary.AddTicks(1)));
    }

    [Fact]
    public void Every_minute_matches_the_tariff_table()
    {
        var bands = new (int Start, int End, int Fee)[]
        {
            (0, 359, 0), (360, 389, 8), (390, 419, 13), (420, 479, 18),
            (480, 509, 13), (510, 899, 8), (900, 929, 13), (930, 1019, 18),
            (1020, 1079, 13), (1080, 1109, 8), (1110, 1439, 0)
        };
        foreach (var (start, end, fee) in bands)
        {
            for (int minute = start; minute <= end; minute++)
            {
                DateTime passage = At(0).AddMinutes(minute);
                Assert.Equal(fee, calculator.GetPassageFee(car, passage));
                Assert.Equal(fee, calculator.GetPassageFee(car, passage.AddSeconds(59)));
            }
        }
    }

}
