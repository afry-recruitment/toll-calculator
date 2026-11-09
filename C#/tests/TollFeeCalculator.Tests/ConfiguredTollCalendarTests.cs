using Xunit;

namespace TollFeeCalculator.Tests;

public class ConfiguredTollCalendarTests
{
    [Fact]
    public void Default_calendar_matches_every_day_in_original_policy()
    {
        // Independent fixtures copied from the exercise, not queried from production code.
        string[] exempt = { "01-01", "03-28", "03-29", "04-01", "04-30", "05-01", "05-08", "05-09",
            "06-05", "06-06", "06-21", "11-01", "12-24", "12-25", "12-26", "12-31" };
        var calendar = ConfiguredTollCalendar.For2013();
        for (DateOnly day = new(2013, 1, 1); day.Year == 2013; day = day.AddDays(1))
        {
            bool expected = day.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday
                || day.Month == 7 || exempt.Contains(day.ToString("MM-dd", System.Globalization.CultureInfo.InvariantCulture));
            Assert.Equal(expected, calendar.IsTollFree(day));
        }
    }

    [Fact]
    public void Configured_calendar_can_support_a_different_year()
    {
        // Synthetic test policy, not a claim about the real city's calendar.
        var calendar = new ConfiguredTollCalendar(2026, new[] { new DateOnly(2026, 11, 6) });
        var calculator = new TollCalculator(calendar);
        Assert.Equal(0, calculator.GetPassageFee(VehicleType.Car, new DateTime(2026, 11, 6, 7, 0, 0)));
        Assert.Equal(18, calculator.GetPassageFee(VehicleType.Car, new DateTime(2026, 11, 5, 7, 0, 0)));
    }

    [Fact]
    public void Exemption_input_is_defensively_copied()
    {
        var dates = new List<DateOnly> { new(2026, 11, 6) };
        var calendar = new ConfiguredTollCalendar(2026, dates);
        dates.Clear();
        Assert.True(calendar.IsTollFree(new DateOnly(2026, 11, 6)));
    }

    [Fact]
    public void July_exemption_can_be_disabled()
    {
        var calendar = new ConfiguredTollCalendar(2026, Array.Empty<DateOnly>(), exemptJuly: false);
        Assert.False(calendar.IsTollFree(new DateOnly(2026, 7, 1)));
        Assert.True(calendar.IsTollFree(new DateOnly(2026, 7, 4))); // weekends remain exempt
    }

    [Fact]
    public void Invalid_configuration_is_rejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ConfiguredTollCalendar(0, Array.Empty<DateOnly>()));
        Assert.Throws<ArgumentOutOfRangeException>(() => new ConfiguredTollCalendar(10000, Array.Empty<DateOnly>()));
        Assert.Throws<ArgumentNullException>(() => new ConfiguredTollCalendar(2013, null!));
        Assert.Throws<ArgumentException>(() => new ConfiguredTollCalendar(2013, new[] { new DateOnly(2014, 1, 1) }));
    }

    [Fact]
    public void Unknown_year_is_rejected_even_on_weekends_and_in_july()
    {
        var calendar = ConfiguredTollCalendar.For2013();
        Assert.Throws<ArgumentOutOfRangeException>(() => calendar.IsTollFree(new DateOnly(2014, 7, 1)));
        Assert.Throws<ArgumentOutOfRangeException>(() => calendar.IsTollFree(new DateOnly(2014, 2, 1)));
    }
}
