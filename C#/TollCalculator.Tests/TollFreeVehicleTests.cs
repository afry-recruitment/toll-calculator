using TollCalculator.Models;

namespace TollCalculator.Tests;

public class TollFreeVehicleTests
{
    private readonly TollCalculator _calculator = new();
    private readonly DateTime[] _rushHour = new[] { new DateTime(2026, 6, 23, 7, 0, 0) };

    [Fact]
    public void GetTollFee_Motorbike_Returns0()
        => Assert.Equal(0, _calculator.GetTollFee(new Motorbike(), _rushHour));

    [Fact]
    public void GetTollFee_Tractor_Returns0()
        => Assert.Equal(0, _calculator.GetTollFee(new Tractor(), _rushHour));

    [Fact]
    public void GetTollFee_Emergency_Returns0()
        => Assert.Equal(0, _calculator.GetTollFee(new Emergency(), _rushHour));

    [Fact]
    public void GetTollFee_Diplomat_Returns0()
        => Assert.Equal(0, _calculator.GetTollFee(new Diplomat(), _rushHour));

    [Fact]
    public void GetTollFee_Foreign_Returns0()
        => Assert.Equal(0, _calculator.GetTollFee(new Foreign(), _rushHour));

    [Fact]
    public void GetTollFee_Military_Returns0()
        => Assert.Equal(0, _calculator.GetTollFee(new Military(), _rushHour));

    [Fact]
    public void GetTollFee_Car_ReturnsFee()
        => Assert.True(_calculator.GetTollFee(new Car(), _rushHour) > 0);
}