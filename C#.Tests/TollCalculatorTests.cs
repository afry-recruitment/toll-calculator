using TollFeeCalculator;

namespace TollFeeCalculator.Tests;

public class TollCalculatorTests
{
    [Fact]
    public void GetTollFee_PassagesMoreThanAnHourApart_AddsBothFees()
    {
        var calculator = new TollCalculator();
        var passages = new[] { PassageAt(7, 0), PassageAt(9, 0) };

        int fee = calculator.GetTollFee(new Vehicle(VehicleType.Car), passages);

        Assert.Equal(26, fee); // 18 + 8
    }

    [Fact]
    public void GetTollFee_ThreePassagesInOneWindow_ChargesHighestFeeOnce()
    {
        var calculator = new TollCalculator();
        var passages = new[] { PassageAt(6, 0), PassageAt(6, 30), PassageAt(6, 45) };

        int fee = calculator.GetTollFee(new Vehicle(VehicleType.Car), passages);

        Assert.Equal(13, fee); // max(8, 13, 13)
    }

    [Fact]
    public void GetTollFee_TwoPassagesInSecondWindow_GroupsThemTogether()
    {
        var calculator = new TollCalculator();
        var passages = new[] { PassageAt(6, 0), PassageAt(7, 1), PassageAt(7, 10) };

        int fee = calculator.GetTollFee(new Vehicle(VehicleType.Car), passages);

        Assert.Equal(26, fee); // 8 + max(18, 18)
    }

    [Theory]
    [InlineData(-1, 18)]
    [InlineData(0, 18)]
    [InlineData(1, 31)]
    public void GetTollFee_AroundSixtyMinuteBoundary_UsesExactElapsedTime(
        int ticksAfterSixtyMinutes, int expectedFee)
    {
        var calculator = new TollCalculator();
        var firstPassage = PassageAt(7, 0);
        var secondPassage = firstPassage.AddMinutes(60).AddTicks(ticksAfterSixtyMinutes);

        int fee = calculator.GetTollFee(new Vehicle(VehicleType.Car), new[] { firstPassage, secondPassage });

        Assert.Equal(expectedFee, fee);
    }

    [Fact]
    public void GetTollFee_LaterCheaperPassageInSameWindow_KeepsHighestFee()
    {
        var calculator = new TollCalculator();
        var passages = new[] { PassageAt(7, 30), PassageAt(8, 0), PassageAt(8, 30) };

        int fee = calculator.GetTollFee(new Vehicle(VehicleType.Car), passages);

        Assert.Equal(18, fee); // max(18, 13, 8)
    }

    [Fact]
    public void GetTollFee_PassageWithinWindow_DoesNotExtendWindow()
    {
        var calculator = new TollCalculator();
        var passages = new[] { PassageAt(6, 0), PassageAt(6, 45), PassageAt(7, 15) };

        int fee = calculator.GetTollFee(new Vehicle(VehicleType.Car), passages);

        Assert.Equal(31, fee); // max(8, 13) + 18
    }

    [Fact]
    public void GetTollFee_UnsortedPassages_ReturnsCorrectFeeWithoutChangingInput()
    {
        var calculator = new TollCalculator();
        var passages = new[] { PassageAt(7, 15), PassageAt(6, 0), PassageAt(6, 45) };
        var originalOrder = (DateTime[])passages.Clone();

        int fee = calculator.GetTollFee(new Vehicle(VehicleType.Car), passages);

        Assert.Equal(31, fee);
        Assert.Equal(originalOrder, passages);
    }

    [Fact]
    public void GetTollFee_DuplicatePassage_DoesNotIncreaseFee()
    {
        var calculator = new TollCalculator();
        var passages = new[] { PassageAt(6, 0), PassageAt(6, 30), PassageAt(6, 30) };

        int fee = calculator.GetTollFee(new Vehicle(VehicleType.Car), passages);

        Assert.Equal(13, fee);
    }

    [Fact]
    public void GetTollFee_TotalAboveDailyLimit_ReturnsSixty()
    {
        var calculator = new TollCalculator();
        var passages = new[]
        {
            PassageAt(7, 0), PassageAt(8, 1), PassageAt(9, 2),
            PassageAt(15, 0), PassageAt(17, 0)
        };

        int fee = calculator.GetTollFee(new Vehicle(VehicleType.Car), passages);

        Assert.Equal(60, fee); // Uncapped total: 18 + 13 + 8 + 13 + 13 = 65
    }

    [Fact]
    public void GetTollFee_ZeroFeePassage_StartsWindow()
    {
        var calculator = new TollCalculator();
        var passages = new[] { PassageAt(5, 50), PassageAt(6, 10), PassageAt(6, 55) };

        int fee = calculator.GetTollFee(new Vehicle(VehicleType.Car), passages);

        Assert.Equal(21, fee); // max(0, 8) + 13
    }

    [Fact]
    public void GetTollFee_EmptyPassages_ReturnsZero()
    {
        var calculator = new TollCalculator();

        int fee = calculator.GetTollFee(new Vehicle(VehicleType.Car), Array.Empty<DateTime>());

        Assert.Equal(0, fee);
    }

    [Fact]
    public void GetTollFee_NullPassages_ThrowsArgumentNullException()
    {
        var calculator = new TollCalculator();

        var exception = Assert.Throws<ArgumentNullException>(
            () => calculator.GetTollFee(new Vehicle(VehicleType.Car), null!));

        Assert.Equal("dates", exception.ParamName);
    }

    private static DateTime PassageAt(int hour, int minute)
    {
        return new DateTime(2013, 1, 2, hour, minute, 0);
    }

    [Theory]
    [InlineData(1)] // Holiday
    [InlineData(2)] // Ordinary weekday
    [InlineData(5)] // Weekend
    public void GetTollFee_NullVehicleForSinglePassage_ThrowsArgumentNullException(int day)
    {
        var calculator = new TollCalculator();
        var passage = new DateTime(2013, 1, day, 7, 0, 0);

        var exception = Assert.Throws<ArgumentNullException>(
            () => calculator.GetTollFee(passage, null!));

        Assert.Equal("vehicle", exception.ParamName);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void GetTollFee_NullVehicleForDailyCalculation_ThrowsArgumentNullException(bool empty)
    {
        var calculator = new TollCalculator();
        var passages = empty ? Array.Empty<DateTime>() : new[] { PassageAt(7, 0) };

        var exception = Assert.Throws<ArgumentNullException>(
            () => calculator.GetTollFee(null!, passages));

        Assert.Equal("vehicle", exception.ParamName);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void GetTollFee_MixedDays_ThrowsArgumentExceptionEvenForExemptVehicle(bool tollFree)
    {
        var calculator = new TollCalculator();
        Vehicle vehicle = tollFree ? new Vehicle(VehicleType.Motorbike) : new Vehicle(VehicleType.Car);
        var passages = new[] { PassageAt(7, 0).AddDays(1), PassageAt(7, 0) };
        var originalOrder = (DateTime[])passages.Clone();

        var exception = Assert.Throws<ArgumentException>(
            () => calculator.GetTollFee(vehicle, passages));

        Assert.Equal("dates", exception.ParamName);
        Assert.Equal(originalOrder, passages);
    }

    [Fact]
    public void GetTollFee_PassagesAcrossMidnightWithinOneHour_ThrowsArgumentException()
    {
        var calculator = new TollCalculator();
        var firstPassage = PassageAt(23, 45);
        var passages = new[] { firstPassage, firstPassage.AddMinutes(30) };

        var exception = Assert.Throws<ArgumentException>(
            () => calculator.GetTollFee(new Vehicle(VehicleType.Car), passages));

        Assert.Equal("dates", exception.ParamName);
    }

    [Theory]
    [InlineData(8, 29, 59, 13)]
    [InlineData(8, 30, 0, 8)]
    [InlineData(9, 0, 0, 8)]
    [InlineData(10, 0, 0, 8)]
    [InlineData(11, 0, 0, 8)]
    [InlineData(12, 0, 0, 8)]
    [InlineData(13, 0, 0, 8)]
    [InlineData(14, 0, 0, 8)]
    [InlineData(14, 29, 59, 8)]
    [InlineData(14, 59, 59, 8)]
    [InlineData(15, 0, 0, 13)]
    public void GetTollFee_CarDuringAndAroundDaytimeBand_ReturnsExpectedFee(
        int hour, int minute, int second, int expectedFee)
    {
        var calculator = new TollCalculator();
        var passage = new DateTime(2013, 1, 2, hour, minute, second);

        int fee = calculator.GetTollFee(passage, new Vehicle(VehicleType.Car));

        Assert.Equal(expectedFee, fee);
    }

    [Theory]
    [InlineData(6, 15, 8)]
    [InlineData(6, 45, 13)]
    [InlineData(7, 15, 18)]
    [InlineData(8, 15, 13)]
    [InlineData(9, 45, 8)]
    [InlineData(15, 15, 13)]
    [InlineData(15, 45, 18)]
    [InlineData(17, 15, 13)]
    [InlineData(18, 15, 8)]
    public void GetTollFee_SingleCarPassageOnOrdinaryWeekday_ReturnsExpectedFee(
        int hour, int minute, int expectedFee)
    {
        var calculator = new TollCalculator();
        // Wednesday, outside the existing holiday exemptions.
        var passage = new DateTime(2013, 1, 2, hour, minute, 0);

        int fee = calculator.GetTollFee(new Vehicle(VehicleType.Car), new[] { passage });

        Assert.Equal(expectedFee, fee);
    }

    [Theory]
    [InlineData(5, 59)]
    [InlineData(18, 30)]
    public void GetTollFee_CarOutsideChargingHours_ReturnsZero(int hour, int minute)
    {
        var calculator = new TollCalculator();
        var passage = new DateTime(2013, 1, 2, hour, minute, 0);

        int fee = calculator.GetTollFee(new Vehicle(VehicleType.Car), new[] { passage });

        Assert.Equal(0, fee);
    }

    [Fact]
    public void GetTollFee_MotorbikeDuringRushHour_ReturnsZero()
    {
        var calculator = new TollCalculator();
        var passage = new DateTime(2013, 1, 2, 7, 15, 0);

        int fee = calculator.GetTollFee(new Vehicle(VehicleType.Motorbike), new[] { passage });

        Assert.Equal(0, fee);
    }

    [Theory]
    [InlineData(5)] // Saturday
    [InlineData(6)] // Sunday
    public void GetTollFee_CarOnWeekend_ReturnsZero(int day)
    {
        var calculator = new TollCalculator();
        var passage = new DateTime(2013, 1, day, 7, 15, 0);

        int fee = calculator.GetTollFee(new Vehicle(VehicleType.Car), new[] { passage });

        Assert.Equal(0, fee);
    }

    [Fact]
    public void GetTollFee_CarOnNewYearsDay2013_ReturnsZero()
    {
        var calculator = new TollCalculator();
        var passage = new DateTime(2013, 1, 1, 7, 15, 0);

        int fee = calculator.GetTollFee(new Vehicle(VehicleType.Car), new[] { passage });

        Assert.Equal(0, fee);
    }
}
