using FluentAssertions;
using TrafficToll.Internals.Enums;
using TrafficToll.Internals.Services;
using TrafficToll.Internals.ValueObjects;
using Xunit;

namespace TrafficToll.Tests.ClassTests;

public class TollPassingVerifyerTests
{
    private VehicleType[] FreeVehicleTypes => Enumerable.Range(0, 5).Select(x => (VehicleType)x).ToArray();
    private IEnumerable<(int year, int month, int day)> Holidays => new[] { (2022, 5, 1), (2022, 12, 26) };
    private TollPassingVerifyer _tollPassingVerifyer => new TollPassingVerifyer(new TollableParameters(FreeVehicleTypes, Holidays));

    [Fact]
    public void PassingsWhereThereIsNoHoliday_expect_remove_one_holiday_passing()
    {
        // Arrange
        var passings = new[]
        {
            new DateTime(2022,5,1,7,7,7),
            new DateTime(2022,5,2,7,7,7),
            new DateTime(2022,5,3,7,7,7),
        };

        // Act & Assert
        _tollPassingVerifyer.GetTollablePassings(passings).Should().HaveCount(2);
    }

    [Fact]
    public void PassingsWhereThereIsNoHoliday_expect_unchanged()
    {
        // Arrange
        var passings = new[]
        {
            new DateTime(2022,5,2,7,7,7),
            new DateTime(2022,5,3,7,7,7),
            new DateTime(2022,5,4,7,7,7),
        };

        var holidays = Array.Empty<(int year, int month, int day)>();

        // Act & Assert
        _tollPassingVerifyer.GetTollablePassings(passings).Should().HaveCount(3);
    }

    [Fact]
    public void PassingsWhichAreNotDuringWeekend_expect_adjusted_passings()
    {
        // Arrange

        var friday = new DateTime(2022, 12, 23, 0, 0, 0);
        var saturday = new DateTime(2022, 12, 24, 0, 0, 0);
        var sunday = new DateTime(2022, 12, 25, 0, 0, 0);

        var passings = new[]
        {
            friday, saturday, sunday
        };

        // Act & Assert
        _tollPassingVerifyer.GetTollablePassings(passings).Should().HaveCount(1);
    }

    [Fact]
    public void GetTollablePassings_expect_adjusted_passings()
    {
        // Arrange
        var saturday = new DateTime(2022, 12, 24, 0, 0, 0);
        var sunday = new DateTime(2022, 12, 25, 0, 0, 0);
        var monday = new DateTime(2022, 12, 26, 0, 0, 0);

        var passings = new[]
        {
            saturday, sunday, monday
        };

        // Act & Assert
        _tollPassingVerifyer.GetTollablePassings(passings).Should().HaveCount(0);
    }
}
