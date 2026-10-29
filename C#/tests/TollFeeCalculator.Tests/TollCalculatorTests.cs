namespace TollFeeCalculator.Tests;

public class TollCalculatorTests
{
    private static readonly Vehicle Car = new(VehicleType.Car);
    private static readonly DateOnly Weekday = new(2025, 3, 5); // ordinary Wednesday
    private readonly TollCalculator _calculator = new();

    private static DateTime At(string time, DateOnly? date = null) =>
        (date ?? Weekday).ToDateTime(TimeOnly.Parse(time));

    private int Fee(params string[] times) => _calculator.GetTollFee(Car, times.Select(t => At(t)));

    [Fact]
    public void NoPassages_IsFree() => Assert.Equal(0, Fee());

    [Fact]
    public void SinglePassage_ChargesScheduledFee() => Assert.Equal(18, Fee("07:15"));

    [Fact]
    public void PassagesWithinOneHour_ChargeHighestFeeOnce() => Assert.Equal(18, Fee("06:20", "06:45", "07:10"));

    [Fact]
    public void PassagesMoreThanOneHourApart_AreChargedSeparately() => Assert.Equal(8 + 8, Fee("10:00", "11:01"));

    [Fact]
    public void PassageExactlyOneHourLater_StartsNewWindow() => Assert.Equal(8 + 8, Fee("10:00", "11:00"));

    [Fact]
    public void WindowStartsAtFirstPassage_NotAtEachPassage() =>
        // 06:20 opens a window (max 18 via 07:10); 07:25 is a new window.
        Assert.Equal(18 + 18, Fee("06:20", "07:10", "07:25"));

    [Fact]
    public void FreePassages_DoNotOpenWindow() =>
        // 05:50 is free, so the window opens at 06:20 and includes 06:40.
        Assert.Equal(13, Fee("05:50", "06:20", "06:40"));

    [Fact]
    public void PassagesCompareOnFullTime_NotMillisecondComponent() =>
        // The original code compared DateTime.Millisecond, merging everything into one window.
        Assert.Equal(8 + 8, Fee("09:00", "14:00"));

    [Fact]
    public void UnorderedPassages_AreHandled() => Assert.Equal(18, Fee("07:10", "06:20", "06:45"));

    [Fact]
    public void DailyFee_IsCappedAt60() =>
        Assert.Equal(TollCalculator.MaxDailyFee, Fee("06:30", "07:30", "08:30", "15:30", "16:30", "17:30"));

    [Fact]
    public void DailyCap_IsAppliedPerDay()
    {
        string[] rushHours = ["06:30", "07:30", "08:30", "15:30", "16:30", "17:30"];
        var nextDay = Weekday.AddDays(1);
        var passages = rushHours.Select(t => At(t)).Concat(rushHours.Select(t => At(t, nextDay)));

        Assert.Equal(120, _calculator.GetTollFee(Car, passages));
    }

    [Theory]
    [InlineData(VehicleType.Motorbike)]
    [InlineData(VehicleType.Tractor)]
    [InlineData(VehicleType.Emergency)]
    [InlineData(VehicleType.Diplomat)]
    [InlineData(VehicleType.Foreign)]
    [InlineData(VehicleType.Military)]
    public void TollFreeVehicles_PayNothing(VehicleType type)
    {
        var vehicle = new Vehicle(type);
        Assert.Equal(0, _calculator.GetTollFee(vehicle, [At("07:15"), At("16:00")]));
        Assert.Equal(0, _calculator.GetTollFee(vehicle, At("07:15")));
    }

    [Theory]
    [InlineData("2025-03-08")] // Saturday
    [InlineData("2025-03-09")] // Sunday
    [InlineData("2025-07-15")] // July
    [InlineData("2025-06-06")] // National day
    [InlineData("2025-06-05")] // Day before holiday
    [InlineData("2025-04-18")] // Good Friday
    [InlineData("2025-04-17")] // Maundy Thursday (day before Good Friday)
    [InlineData("2025-12-31")] // Day before New Year's Day (crosses year boundary)
    [InlineData("2030-05-29")] // Day before Ascension Day, not hard coded
    public void TollFreeDates_AreFree(string date) =>
        Assert.Equal(0, _calculator.GetTollFee(Car, [At("07:15", DateOnly.Parse(date))]));

    [Fact]
    public void SinglePassageOverload_ReturnsScheduledFee() =>
        Assert.Equal(13, _calculator.GetTollFee(Car, At("08:15")));

    [Fact]
    public void NullArguments_Throw()
    {
        Assert.Throws<ArgumentNullException>(() => _calculator.GetTollFee(null!, [At("07:00")]));
        Assert.Throws<ArgumentNullException>(() => _calculator.GetTollFee(Car, (IEnumerable<DateTime>)null!));
    }
}
