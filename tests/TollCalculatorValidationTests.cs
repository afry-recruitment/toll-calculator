namespace TollFeeCalculator.Tests;

public class TollCalculatorValidationTests
{
    [Theory]
    [InlineData(VehicleType.Unknown, 1, 2)] // Chargeable weekday.
    [InlineData(VehicleType.Unknown, 1, 1)] // New Year's Day.
    [InlineData((VehicleType)(-1), 1, 2)]
    [InlineData((VehicleType)(-1), 1, 1)]
    [InlineData((VehicleType)999, 1, 2)]
    [InlineData((VehicleType)999, 1, 1)]
    public void InvalidVehicle_ThrowsBeforeExemptionChecksInBothMethods(
        VehicleType vehicle, int month, int day)
    {
        var calculator = new TollCalculator();
        var passage = new DateTime(2013, month, day, 7, 0, 0);

        var singleError = Assert.Throws<ArgumentOutOfRangeException>(
            () => calculator.GetTollFee(passage, vehicle));
        var dailyError = Assert.Throws<ArgumentOutOfRangeException>(
            () => calculator.GetTollFee(vehicle, [passage]));

        Assert.Equal("vehicle", singleError.ParamName);
        Assert.Equal("vehicle", dailyError.ParamName);
    }

    [Theory]
    [InlineData(VehicleType.Unknown)]
    [InlineData((VehicleType)(-1))]
    [InlineData((VehicleType)999)]
    public void DailyTotal_InvalidVehicleWithEmptyPassages_Throws(VehicleType vehicle)
    {
        var calculator = new TollCalculator();

        var error = Assert.Throws<ArgumentOutOfRangeException>(
            () => calculator.GetTollFee(vehicle, Array.Empty<DateTime>()));

        Assert.Equal("vehicle", error.ParamName);
    }

    [Fact]
    public void DailyTotal_NullPassages_Throws()
    {
        var calculator = new TollCalculator();

        var error = Assert.Throws<ArgumentNullException>(
            () => calculator.GetTollFee(VehicleType.Car, null!));

        Assert.Equal("dates", error.ParamName);
    }

    [Theory]
    [InlineData(1, 2, 1, 3)]
    [InlineData(1, 2, 2, 2)] // Same day number, different months.
    public void PassagesOnDifferentDates_Throw(int firstMonth, int firstDay, int secondMonth, int secondDay)
    {
        var calculator = new TollCalculator();
        var passages = new[]
        {
            new DateTime(2013, firstMonth, firstDay, 23, 59, 59),
            new DateTime(2013, secondMonth, secondDay, 0, 0, 0)
        };

        foreach (VehicleType vehicle in new VehicleType[] { VehicleType.Car, VehicleType.Motorbike })
        {
            var error = Assert.Throws<ArgumentException>(() => calculator.GetTollFee(vehicle, passages));
            Assert.Equal("dates", error.ParamName);
            Assert.Contains("All passages must be on the same calendar date", error.Message);
        }
    }

    [Theory]
    [InlineData(2012, 12, 31)]
    [InlineData(2014, 1, 1)]
    public void DatesOutside2013_ThrowForBothMethods(int year, int month, int day)
    {
        var calculator = new TollCalculator();
        var passage = new DateTime(year, month, day, 7, 0, 0);

        foreach (VehicleType vehicle in new VehicleType[] { VehicleType.Car, VehicleType.Motorbike })
        {
            var singleError = Assert.Throws<ArgumentOutOfRangeException>(
                () => calculator.GetTollFee(passage, vehicle));
            Assert.Equal("date", singleError.ParamName);
            Assert.Contains("Only dates in 2013 are supported", singleError.Message);

            var dailyError = Assert.Throws<ArgumentOutOfRangeException>(
                () => calculator.GetTollFee(vehicle, new[] { new DateTime(2013, 1, 2), passage }));
            Assert.Equal("dates", dailyError.ParamName);
            Assert.Contains("Only dates in 2013 are supported", dailyError.Message);
        }
    }

    [Theory]
    [InlineData(1, 1, 0, 0, 0)]
    [InlineData(12, 31, 23, 59, 59)]
    public void DatesAt2013Boundaries_AreAcceptedByBothMethods(
        int month, int day, int hour, int minute, int second)
    {
        var calculator = new TollCalculator();
        var passage = new DateTime(2013, month, day, hour, minute, second);
        var car = VehicleType.Car;

        Assert.Equal(0, calculator.GetTollFee(passage, car));
        Assert.Equal(0, calculator.GetTollFee(car, new[] { passage }));
    }

}

