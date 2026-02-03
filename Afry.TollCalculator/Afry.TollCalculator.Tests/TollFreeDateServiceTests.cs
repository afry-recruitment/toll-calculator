using Microsoft.Extensions.Options;
using Afry.TollCalculator.Services;
using Afry.TollCalculator.Infrastructure.Settings;

namespace Afry.TollCalculator.Tests;

public sealed class TollFreeDateServiceTests
{
    private static readonly TollSettings Settings = TollSettings.Load();
    private static readonly ITollFreeDateService Service = new TollFreeDateService(
        Options.Create(Settings));

    public static IEnumerable<object[]> ConfiguredTollFreeDays()
        => Settings.TollFreeDays.Select(d => new object[] { DateOnly.Parse(d) });

    public static IEnumerable<object[]> ConfiguredTollFreeWeekdays()
        => Settings.TollFreeWeekdays.Select(w => new object[] { Enum.Parse<DayOfWeek>(w, ignoreCase: true) });

    [Theory]
    [MemberData(nameof(ConfiguredTollFreeDays))]
    public void IsTollFreeDate_ConfiguredTollFreeDays_AreTollFree(DateOnly date)
    {
        // Arrange
        var dateTimeOffset = new DateTimeOffset(date.ToDateTime(new TimeOnly(12, 0)));

        // Act
        var isTollFree = Service.IsTollFreeDate(dateTimeOffset);

        // Assert
        Assert.True(isTollFree.IsSuccess);
        Assert.True(isTollFree.Value);
    }

    [Theory]
    [MemberData(nameof(ConfiguredTollFreeWeekdays))]
    public void IsTollFreeDate_ConfiguredTollFreeWeekdays_AreTollFree(DayOfWeek dayOfWeek)
    {
        // Arrange
        var start = DateOnly.FromDateTime(new DateTime(2026, 1, 1));
        var date = start;
        for (int i = 0; i < 14 && date.DayOfWeek != dayOfWeek; i++)
        {
            date = date.AddDays(1);
        }

        Assert.Equal(dayOfWeek, date.DayOfWeek);

        var dateTimeOffset = new DateTimeOffset(date.ToDateTime(new TimeOnly(12, 0)));

        // Act
        var isTollFree = Service.IsTollFreeDate(dateTimeOffset);

        // Assert
        Assert.True(isTollFree.IsSuccess);
        Assert.True(isTollFree.Value);
    }

    [Fact]
    public void IsTollFreeDate_NonConfiguredWeekdayAndDate_IsNotTollFree()
    {
        // Arrange
        DateOnly date = new(2026, 2, 2);
        if (Settings.TollFreeDays.Contains(date.ToString("yyyy-MM-dd")) || Settings.TollFreeWeekdays.Contains(date.DayOfWeek.ToString()))
        {
            return;
        }

        var dateTimeOffset = new DateTimeOffset(date.ToDateTime(new TimeOnly(12, 0)));

        // Act
        var isTollFree = Service.IsTollFreeDate(dateTimeOffset);

        // Assert
        Assert.True(isTollFree.IsSuccess);
        Assert.True(isTollFree.Value);
    }
}
