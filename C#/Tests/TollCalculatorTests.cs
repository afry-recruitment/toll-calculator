using System;
using TollFeeCalculator;
using Xunit;

public class TollCalculatorTests
{
    private static readonly DateTime Wednesday = new DateTime(2026, 11, 18);
    private static readonly DateTime Saturday = new DateTime(2026, 11, 21);
    private static readonly DateTime ChristmasDay = new DateTime(2026, 12, 25);

    private readonly TollCalculator calculator = new TollCalculator();
    private readonly Car car = new Car("ABC123");

    private static DateTime At(DateTime day, int hour, int minute) =>
        day.AddHours(hour).AddMinutes(minute);

    [Fact]
    public void SinglePassageInRushHourIsChargedHighFee()
    {
        Assert.Equal(18, calculator.GetTollFee(car, new[] { At(Wednesday, 7, 30) }));
    }

    [Fact]
    public void PassagesWithinOneHourAreChargedHighestFee()
    {
        // 06:20 costs 8 SEK and 06:45 costs 13 SEK; both fall within one hour of 06:20.
        DateTime[] passages = { At(Wednesday, 6, 20), At(Wednesday, 6, 45) };

        Assert.Equal(13, calculator.GetTollFee(car, passages));
    }

    [Fact]
    public void PassageExactlyOneHourAfterIntervalStartIsInSameInterval()
    {
        DateTime[] passages = { At(Wednesday, 7, 0), At(Wednesday, 8, 0) };

        Assert.Equal(18, calculator.GetTollFee(car, passages));
    }

    [Fact]
    public void PassageAfterOneHourStartsNewInterval()
    {
        DateTime[] passages = { At(Wednesday, 7, 0), At(Wednesday, 8, 1) };

        Assert.Equal(31, calculator.GetTollFee(car, passages));
    }

    [Fact]
    public void DailyTotalIsCappedAt60()
    {
        // Four intervals costing 18 + 13 + 18 + 18 = 67 SEK, capped to 60.
        DateTime[] passages =
        {
            At(Wednesday, 7, 0),
            At(Wednesday, 8, 1),
            At(Wednesday, 15, 30),
            At(Wednesday, 16, 31),
        };

        Assert.Equal(60, calculator.GetTollFee(car, passages));
    }

    [Fact]
    public void InputOrderDoesNotAffectResult()
    {
        DateTime[] sorted = { At(Wednesday, 7, 0), At(Wednesday, 8, 1), At(Wednesday, 15, 30) };
        DateTime[] unsorted = { At(Wednesday, 15, 30), At(Wednesday, 7, 0), At(Wednesday, 8, 1) };

        Assert.Equal(calculator.GetTollFee(car, sorted), calculator.GetTollFee(car, unsorted));
    }

    [Fact]
    public void WeekendPassagesAreFeeFree()
    {
        Assert.Equal(0, calculator.GetTollFee(car, new[] { At(Saturday, 7, 30) }));
    }

    [Fact]
    public void HolidayPassagesAreFeeFree()
    {
        Assert.Equal(0, calculator.GetTollFee(car, new[] { At(ChristmasDay, 7, 30) }));
    }

    [Fact]
    public void MotorbikeIsFeeFree()
    {
        Assert.Equal(0, calculator.GetTollFee(new Motorbike("MBK001"), new[] { At(Wednesday, 7, 30) }));
    }

    [Theory]
    [InlineData("")]
    [InlineData("ABC12")]
    [InlineData("ABC1234")]
    public void LicensePlateMustBeSixCharacters(string licensePlate)
    {
        Assert.Throws<ArgumentException>(() => new Car(licensePlate));
    }

    [Fact]
    public void EmergencyVehicleIsFeeFree()
    {
        Assert.Equal(0, calculator.GetTollFee(new Emergency("EMR001"), new[] { At(Wednesday, 7, 30) }));
    }

    [Fact]
    public void TollFreeVehicleClassesAreFeeFree()
    {
        DateTime[] passages = { At(Wednesday, 7, 30) };

        Assert.Equal(0, calculator.GetTollFee(new Tractor("TRC001"), passages));
        Assert.Equal(0, calculator.GetTollFee(new Diplomat("DIP001"), passages));
        Assert.Equal(0, calculator.GetTollFee(new Foreign("FRG001"), passages));
        Assert.Equal(0, calculator.GetTollFee(new Military("MIL001"), passages));
    }

    [Fact]
    public void EmptyPassagesReturnZero()
    {
        Assert.Equal(0, calculator.GetTollFee(car, Array.Empty<DateTime>()));
    }

    [Fact]
    public void NullPassagesThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => calculator.GetTollFee(car, null));
    }

    [Fact]
    public void PassagesOnDifferentDaysThrowArgumentException()
    {
        DateTime[] passages = { At(Wednesday, 7, 30), At(Wednesday.AddDays(1), 7, 30) };

        Assert.Throws<ArgumentException>(() => calculator.GetTollFee(car, passages));
    }
}
