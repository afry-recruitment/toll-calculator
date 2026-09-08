using TollFeeCalculatorLibrary;

namespace TollFeeCalculatorTests;

public class TollCalculatorMultipleDatesTests
{
    readonly TollCalculator sut;
    readonly TestVehicle car;
    readonly TestVehicle tollFree;

    public TollCalculatorMultipleDatesTests()
    {
        sut = new TollCalculator();
        car = new TestVehicle("Car");
        tollFree = new TestVehicle(TollFreeVehicles.Emergency.ToString());
    }

    [Fact]
    public void ShouldSumFeesFromDifferentHours()
    {
        DateTime[] dates =
        [
            new DateTime(2023, 10, 10, 7, 0, 0),  // 18 SEK
            new DateTime(2023, 10, 10, 15, 30, 0) // 18 SEK
        ];

        int result = sut.GetTollFee(car, dates);

        Assert.Equal(36, result);
    }

    [Fact]
    public void SameHour_ShouldOnlyChargeOnceAtHighestRate()
    {
        DateTime[] dates =
        [
            new DateTime(2023, 10, 10, 7, 10, 0),
            new DateTime(2023, 10, 10, 7, 40, 0)
        ];

        int result = sut.GetTollFee(car, dates);

        Assert.Equal(18, result);
    }

    [Fact]
    public void ShouldCapAtSixtySek()
    {
        DateTime[] dates =
        [
            new DateTime(2023, 10, 10, 7, 0, 0),
            new DateTime(2023, 10, 10, 15, 30, 0),
            new DateTime(2023, 10, 10, 16, 0, 0),
            new DateTime(2023, 10, 10, 17, 0, 0)
        ];

        int result = sut.GetTollFee(car, dates);

        Assert.Equal(60, result);
    }

    [Fact]
    public void ShouldThrowExceptionIfSpanningMultipleDays()
    {
        DateTime[]? dates =
        [
            new DateTime(2023, 10, 10, 10, 0, 0),
            new DateTime(2023, 10, 11, 10, 0, 0) // Precondition violation
        ];

        Assert.Throws<Exception>(() => sut.GetTollFee(car, dates));
    }
}