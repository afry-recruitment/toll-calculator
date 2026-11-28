using TollFeeCalculator;

namespace TollFeeCalculatorTests;

public class TollFeeCalculatorTests
{
    private readonly TollCalculator _calculator = new();

    [Fact]
    public void GetTollFee_SinglePassage_ReturnsCorrectFee()
    {
        var vehicle = new Car();

        var dates = new[]
        {
            new DateTime(2013, 2, 4, 6, 15, 0)
        };

        var result = _calculator.GetTollFee(vehicle, dates);

        Assert.Equal(8, result);
    }

    [Fact]
    public void GetTollFee_TwoPassagesWithin60Minutes_ChargesHighestFeeOnly()
    {
        var vehicle = new Car();

        var dates = new[]
        {
            new DateTime(2013, 2, 4, 6, 15, 0), // 8 SEK
            new DateTime(2013, 2, 4, 6, 45, 0)  // 13 SEK
        };

        var result = _calculator.GetTollFee(vehicle, dates);

        Assert.Equal(13, result);
    }

    [Fact]
    public void GetTollFee_Exactly60MinutesApart_ChargesHighestFeeOnly()
    {
        var vehicle = new Car();

        var dates = new[]
        {
            new DateTime(2013, 2, 4, 6, 15, 0), // 8 SEK
            new DateTime(2013, 2, 4, 7, 15, 0)  // 18 SEK
        };

        var result = _calculator.GetTollFee(vehicle, dates);

        Assert.Equal(18, result);
    }

    [Fact]
    public void GetTollFee_MoreThan60MinutesApart_ChargesBothFees()
    {
        var vehicle = new Car();

        var dates = new[]
        {
            new DateTime(2013, 2, 4, 6, 15, 0), // 8 SEK
            new DateTime(2013, 2, 4, 7, 16, 0)  // 18 SEK
        };

        var result = _calculator.GetTollFee(vehicle, dates);

        Assert.Equal(26, result);
    }

    [Fact]
    public void GetTollFee_MultiplePassagesWithinSame60MinuteWindow_ChargesHighestFee()
    {
        var vehicle = new Car();

        var dates = new[]
        {
            new DateTime(2013, 2, 4, 6, 5, 0),  // 8 SEK
            new DateTime(2013, 2, 4, 6, 35, 0), // 13 SEK
            new DateTime(2013, 2, 4, 7, 0, 0)   // 18 SEK
        };

        var result = _calculator.GetTollFee(vehicle, dates);

        Assert.Equal(18, result);
    }

    [Fact]
    public void GetTollFee_NewIntervalStartsAfter60MinutesFromIntervalStart()
    {
        var vehicle = new Car();

        var dates = new[]
        {
            new DateTime(2013, 2, 4, 6, 5, 0),  // interval 1
            new DateTime(2013, 2, 4, 6, 35, 0), // highest = 13
            new DateTime(2013, 2, 4, 7, 6, 0)   // new interval = 18
        };

        var result = _calculator.GetTollFee(vehicle, dates);

        Assert.Equal(31, result);
    }

    [Fact]
    public void GetTollFee_TotalExceedsDailyMaximum_CapsAt60()
    {
        var vehicle = new Car();

        var dates = new[]
        {
            new DateTime(2013, 2, 4, 6, 0, 0),   // 8
            new DateTime(2013, 2, 4, 7, 15, 0),  // 18
            new DateTime(2013, 2, 4, 8, 20, 0),  // 13
            new DateTime(2013, 2, 4, 15, 15, 0), // 13
            new DateTime(2013, 2, 4, 16, 30, 0)  // 18
        };

        var result = _calculator.GetTollFee(vehicle, dates);

        Assert.Equal(60, result);
    }

    [Fact]
    public void GetTollFee_Weekend_ReturnsZero()
    {
        var vehicle = new Car();

        var dates = new[]
        {
            new DateTime(2013, 2, 2, 7, 15, 0) // Saturday
        };

        var result = _calculator.GetTollFee(vehicle, dates);

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetTollFee_TollFreeVehicle_ReturnsZero()
    {
        var vehicle = new Motorbike();

        var dates = new[]
        {
            new DateTime(2013, 2, 4, 7, 15, 0)
        };

        var result = _calculator.GetTollFee(vehicle, dates);

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetTollFee_EmptyDates_ReturnsZero()
    {
        var vehicle = new Car();

        var result = _calculator.GetTollFee(vehicle, []);

        Assert.Equal(0, result);
    }

    [Theory]
    [InlineData(6, 0, 8)]
    [InlineData(6, 29, 8)]
    [InlineData(6, 30, 13)]
    [InlineData(7, 30, 18)]
    [InlineData(8, 0, 13)]
    [InlineData(8, 30, 8)]
    [InlineData(9, 0, 8)]
    [InlineData(14, 59, 8)]
    [InlineData(15, 0, 13)]
    [InlineData(15, 30, 18)]
    [InlineData(16, 59, 18)]
    [InlineData(17, 0, 13)]
    [InlineData(18, 29, 8)]
    [InlineData(18, 30, 0)]
    public void GetTollFee_ForTimeOfDay_ReturnsExpectedFee(
    int hour,
    int minute,
    int expectedFee)
    {
        var vehicle = new Car();
        var date = new DateTime(2013, 2, 4, hour, minute, 0);

        var result = _calculator.GetTollFee(date, vehicle);

        Assert.Equal(expectedFee, result);
    }
}