namespace TollFeeCalculator.Tests;

public class TollCalculatorTests
{
    [Fact]
    public void Car_in_morning_rush_hour_pays_18()
    {
        var calculator = new TollCalculator();
        var fee = calculator.GetTollFee(new DateTime(2013, 2, 4, 7, 0, 0), new Car());
        Assert.Equal(18, fee);
    }
}