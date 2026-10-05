using TollFeeCalculator;

namespace TollCalculatorTests;

public class TollCalculatorTests
{
    private readonly TollCalculator _tollCalculator = new TollCalculator();
    private readonly Vehicle _car = new Car();

    [Theory]
    [InlineData(6, 29, 8)]
    [InlineData(6, 30, 13)]
    [InlineData(7, 0, 18)]
    [InlineData(8, 29, 13)]
    [InlineData(8, 30, 8)]
    [InlineData(14, 59, 8)]
    [InlineData(15, 0, 13)]
    [InlineData(15, 30, 18)]
    [InlineData(16, 0, 18)]
    [InlineData(17, 0, 13)]
    [InlineData(18, 29, 8)]
    [InlineData(18, 30, 0)]
    public void GetTollFee_ReturnsCorrectFee(int hour, int minute, int expected)
    {
        DateTime date = new DateTime(2013, 6, 14, hour, minute, 0);

        int result = _tollCalculator.GetTollFee(date, _car);

        Assert.Equal(expected, result);
    }

    [Fact]
public void TwoPassesWithin60Minutes_ShouldChargeHighestFee()
{
    DateTime[] dates =
    {
        new DateTime(2013, 6, 14, 7, 0, 0),
        new DateTime(2013, 6, 14, 7, 30, 0)
    };

    int result = _tollCalculator.GetTollFee(_car, dates);

    Assert.Equal(18, result);
}

[Fact]
public void ThreePassesWithin60Minutes_ShouldChargeHighestFee()
{
    DateTime[] dates =
    {
        new DateTime(2013, 6, 14, 7, 0, 0),
        new DateTime(2013, 6, 14, 7, 30, 0),
        new DateTime(2013, 6, 14, 7, 50, 0)
    };

    int result = _tollCalculator.GetTollFee(_car, dates);

    Assert.Equal(18, result);
}

[Fact]
public void Exactly60Minutes_ShouldBeOneCharge()
{
    DateTime[] dates =
    {
        new DateTime(2013, 6, 14, 6, 30, 0),
        new DateTime(2013, 6, 14, 7, 30, 0)
    };

    int result = _tollCalculator.GetTollFee(_car, dates);

    Assert.Equal(18, result);
}

[Fact]
public void MoreThan60Minutes_ShouldChargeBothPasses()
{
    DateTime[] dates =
    {
        new DateTime(2013, 6, 14, 6, 30, 0),
        new DateTime(2013, 6, 14, 7, 31, 0)
    };

    int result = _tollCalculator.GetTollFee(_car, dates);

    Assert.Equal(31, result);
}
[Fact]
public void Motorbike_ShouldBeTollFree()
{
    Vehicle motorbike = new Motorbike();
    DateTime date = new DateTime(2013, 6, 14, 7, 0, 0);

    int result = _tollCalculator.GetTollFee(date, motorbike);

    Assert.Equal(0, result);
}

[Fact]
public void Saturday_ShouldBeTollFree()
{
    DateTime saturday = new DateTime(2013, 6, 15, 7, 0, 0);

    int result = _tollCalculator.GetTollFee(saturday, _car);

    Assert.Equal(0, result);
}

[Fact]
public void Sunday_ShouldBeTollFree()
{
    DateTime sunday = new DateTime(2013, 6, 16, 7, 0, 0);

    int result = _tollCalculator.GetTollFee(sunday, _car);

    Assert.Equal(0, result);
}

[Fact]
public void TollFreeHoliday_ShouldBeTollFree()
{
    DateTime holiday = new DateTime(2013, 6, 6, 7, 0, 0);

    int result = _tollCalculator.GetTollFee(holiday, _car);

    Assert.Equal(0, result);
}
[Fact]
public void DailyFee_ShouldNotExceed60()
{
    DateTime[] dates =
{
    new DateTime(2013, 6, 14, 6, 30, 0),
    new DateTime(2013, 6, 14, 7, 31, 0),
    new DateTime(2013, 6, 14, 8, 32, 0),
    new DateTime(2013, 6, 14, 15, 30, 0),
    new DateTime(2013, 6, 14, 16, 31, 0)
};

    int result = _tollCalculator.GetTollFee(_car, dates);

    Assert.Equal(60, result);
}

[Fact]
public void UnsortedDates_ShouldStillCalculateCorrectly()
{
    DateTime[] dates =
    {
        new DateTime(2013, 6, 14, 7, 30, 0),
        new DateTime(2013, 6, 14, 6, 30, 0)
    };

    int result = _tollCalculator.GetTollFee(_car, dates);

    Assert.Equal(18, result);
}

}