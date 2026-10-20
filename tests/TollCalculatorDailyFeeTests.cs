namespace TollFeeCalculator.Tests;

public class TollCalculatorDailyFeeTests
{
    [Fact]
    public void EmptyPassages_CostNothing()
    {
        var calculator = new TollCalculator();

        Assert.Equal(0, calculator.GetTollFee(VehicleType.Car, Array.Empty<DateTime>()));
    }

    [Theory]
    [InlineData(6, 59, 59, 13)]
    [InlineData(7, 0, 0, 18)]
    [InlineData(7, 0, 1, 26)]
    public void PassagesAround60MinuteBoundary_ChargeExpectedFee(
        int hour, int minute, int second, int expectedFee)
    {
        var calculator = new TollCalculator();
        var passages = new[] { PassageAtHourMinSec(6, 0, 0), PassageAtHourMinSec(hour, minute, second) };

        Assert.Equal(expectedFee, calculator.GetTollFee(VehicleType.Car, passages));
    }

    [Fact]
    // Ensures that the 06:45 passage does not extend the first period to 07:45.
    public void PassageWithinPeriod_DoesNotExtendThePeriod()
    {
        var calculator = new TollCalculator();
        var passages = new[] { PassageAtHourMinSec(6, 0, 0), PassageAtHourMinSec(6, 45, 0), PassageAtHourMinSec(7, 10, 0) };

        Assert.Equal(31, calculator.GetTollFee(VehicleType.Car, passages)); // 13 + 18
    }

    [Theory]
    // Hour and minute for each of the three passages.
    [InlineData(7, 30, 8, 0, 8, 30)] // Highest fee first.
    [InlineData(6, 45, 7, 0, 7, 30)] // Highest fee in the middle.
    [InlineData(6, 0, 6, 30, 7, 0)] // Highest fee last.
    public void Period_ChargesHighestFeeRegardlessOfPosition(
        int firstHour, int firstMinute,
        int secondHour, int secondMinute,
        int thirdHour, int thirdMinute)
    {
        var calculator = new TollCalculator();

        var passages = new[]
        {
            PassageAtHourMinSec(firstHour, firstMinute, 0),
            PassageAtHourMinSec(secondHour, secondMinute, 0),
            PassageAtHourMinSec(thirdHour, thirdMinute, 0)
        };

        Assert.Equal(18, calculator.GetTollFee(VehicleType.Car, passages));
    }

    [Fact]
    public void MultiplePeriods_ChargeEachPeriodsHighestFee()
    {
        var calculator = new TollCalculator();
        var passages = new[]
        {
            PassageAtHourMinSec(6, 0, 0), PassageAtHourMinSec(6, 30, 0), // 13
            PassageAtHourMinSec(8, 0, 0), PassageAtHourMinSec(8, 30, 0), PassageAtHourMinSec(8, 45, 0), // 13
            PassageAtHourMinSec(15, 0, 0), PassageAtHourMinSec(15, 30, 0), PassageAtHourMinSec(15, 45, 0) // 18
        };

        Assert.Equal(44, calculator.GetTollFee(VehicleType.Car, passages));
    }

    [Theory]
    [InlineData(new int[] { 7, 9, 11, 13, 15 }, 55)]     // 18 + 8 + 8 + 8 + 13 = 55
    [InlineData(new int[] { 7, 9, 11, 13, 16, 18 }, 60)] // 18 + 8 + 8 + 8 + 18 + 8 = 68 before cap.
    public void DailyTotal_OnlyCapsFeesAbove60Sek(int[] hours, int expectedFee)
    {
        var calculator = new TollCalculator();

        var passages = new DateTime[hours.Length];
        for (int i = 0; i < hours.Length; i++)
        {
            passages[i] = PassageAtHourMinSec(hours[i], 0, 0);
        }

        Assert.Equal(expectedFee, calculator.GetTollFee(VehicleType.Car, passages));
    }

    [Fact]
    public void UnsortedPassages_GiveSameFeeAsChronologicalPassages()
    {
        var calculator = new TollCalculator();
        var sorted = new[]
        {
            PassageAtHourMinSec(6, 0, 0), PassageAtHourMinSec(6, 45, 0), PassageAtHourMinSec(7, 10, 0), PassageAtHourMinSec(9, 0, 0)
        };
        var unsorted = new[] { sorted[2], sorted[0], sorted[3], sorted[1] };

        Assert.Equal(39, calculator.GetTollFee(VehicleType.Car, sorted)); // 13 + 18 + 8
        Assert.Equal(39, calculator.GetTollFee(VehicleType.Car, unsorted));
    }

    private static DateTime PassageAtHourMinSec(int hour, int minute, int second)
    {
        return new DateTime(2013, 1, 2, hour, minute, second);
    }

    [Fact]
    public void PassagesWithin60Minutes_ChargeOnlyHighestFee()
    {
        var calculator = new TollCalculator();
        var passages = new[]
        {
            PassageAtHourMinSec(6, 0, 0),  // 8 SEK
            PassageAtHourMinSec(6, 30, 0), // 13 SEK
            PassageAtHourMinSec(6, 45, 0), // 13 SEK
        };

        int actualFee = calculator.GetTollFee(VehicleType.Car, passages);

        Assert.Equal(13, actualFee);
    }
}

