namespace TollFeeCalculator.Tests;

public class TollCalculatorTariffTests
{
    [Theory]
    // Hour, minute, second, expected fee.
    [InlineData(5,  59, 59, 0)]
    [InlineData(6,  0 , 0 , 8)]
    [InlineData(6,  29, 59, 8)]
    [InlineData(6,  30, 0 , 13)]
    [InlineData(6,  59, 59, 13)]
    [InlineData(7,  0 , 0 , 18)]
    [InlineData(7,  59, 59, 18)]
    [InlineData(8,  0 , 0 , 13)]
    [InlineData(8,  29, 59, 13)]
    [InlineData(8,  30, 0 , 8)]
    [InlineData(14, 59, 59, 8)]
    [InlineData(15, 0 , 0 , 13)]
    [InlineData(15, 29, 59, 13)]
    [InlineData(15, 30, 0 , 18)]
    [InlineData(16, 59, 59, 18)]
    [InlineData(17, 0 , 0 , 13)]
    [InlineData(17, 59, 59, 13)]
    [InlineData(18, 0 , 0 , 8)]
    [InlineData(18, 29, 59, 8)]
    [InlineData(18, 30, 0 , 0)]
    public void Car_OnChargeableDay_PaysExpectedFeeInBothMethods(
        int hour, int minute, int second, int expectedFee)
    {
        var calculator = new TollCalculator();
        var passage = new DateTime(2013, 1, 2, hour, minute, second);
        var car = VehicleType.Car;

        Assert.Equal(expectedFee, calculator.GetTollFee(passage, car));
        Assert.Equal(expectedFee, calculator.GetTollFee(car, new[] { passage }));
    }

    // Regression: the original minute >= 30 condition left gaps in every hour from 09 to 14.
    [Fact]
    public void DaytimeHour_FirstHalfHour_Costs8Sek()
    {
        var calculator = new TollCalculator();
        var car = VehicleType.Car;

        var passage = new DateTime(2013, 1, 2, 9, 15, 0);

        Assert.Equal(8, calculator.GetTollFee(passage, car));
        Assert.Equal(8, calculator.GetTollFee(car, [passage]));
    }
}
