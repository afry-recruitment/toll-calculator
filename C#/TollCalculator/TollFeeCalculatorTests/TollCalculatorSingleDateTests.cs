using TollFeeCalculatorLibrary;

namespace TollFeeCalculatorTests;

public class TollCalculatorSingleDateTests
{
    private readonly TollCalculator sut;
    private readonly TestVehicle notTollFree;
    private readonly TestVehicle tollFree;

    public TollCalculatorSingleDateTests()
    {
        sut = new TollCalculator();
        notTollFree = new TestVehicle("Car");
        tollFree = new TestVehicle("Emergency");
    }

    [Theory]
    [InlineData(6, 15, 8)]   // 06:15 -> 8 SEK
    [InlineData(23, 59, 8)]  // 23:59 -> 0 SEK
    [InlineData(7, 30, 18)]  // 07:30 -> 18 SEK
    public void ShouldReturnCorrectRateForTime(int hour, int minute, int expected)
    {
        DateTime date = new DateTime(2023, 10, 10, hour, minute, 0);
        int result = sut.GetTollFee(date, notTollFree);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ShouldReturnZeroForWeekend()
    {
        DateTime saturday = new (2023, 10, 14, 12, 0, 0);
        int result = sut.GetTollFee(saturday, notTollFree);
        Assert.Equal(0, result);
    }

    [Fact]
    public void ShouldReturnZeroForTollFreeVehicle()
    {
        DateTime date = new (2023, 10, 10, 12, 0, 0);
        int result = sut.GetTollFee(date, tollFree);
        Assert.Equal(0, result);
    }
}