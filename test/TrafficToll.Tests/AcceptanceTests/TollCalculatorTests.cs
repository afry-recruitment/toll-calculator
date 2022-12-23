using FluentAssertions;
using Xunit;

namespace TrafficToll.Tests.AcceptanceTests;


public class TollCalculatorTests
{
    [Fact]
    public void Some_vehicle_types_are_fee_free()
    {
        // Arrange
        var tollCalculator = new TollCalculator();
        var passings = Get24TollablePassings();

        // Act & Assert
        tollCalculator.GetTollFee(passings, 0).Should().Be(0);
        tollCalculator.GetTollFee(passings, 1).Should().Be(0);
        tollCalculator.GetTollFee(passings, 2).Should().Be(0);
        tollCalculator.GetTollFee(passings, 3).Should().Be(0);
        tollCalculator.GetTollFee(passings, 4).Should().Be(0);
        tollCalculator.GetTollFee(passings, 5).Should().Be(0);
        tollCalculator.GetTollFee(passings, 6).Should().NotBe(0);
    }

    [Fact]
    public void Holidays_are_fee_free()
    {
        // Arrange
        var juldagen = new DateTime(2013, 12, 25, 8, 0, 0);
        var nyårsdagen = new DateTime(2013, 1, 1, 8, 0, 0);
        var förstaMaj = new DateTime(2013, 4, 1, 8, 0, 0);

        var passings = new DateTime[]
        {
            juldagen, nyårsdagen, förstaMaj
        };

        var tollCalculator = new TollCalculator();

        // Act & Assert
        tollCalculator.GetTollFee(passings, 6).Should().Be(0);
    }

    [Fact]
    public void Weekends_are_fee_free()
    {
        // Arrange
        var saturday = new DateTime(2013, 1, 5, 8, 0, 0);
        var sunday = new DateTime(2013, 1, 6, 8, 0, 0);

        var passings = new DateTime[]
        {
            saturday, sunday
        };

        var tollCalculator = new TollCalculator();

        // Act & Assert
        tollCalculator.GetTollFee(passings, 6).Should().Be(0);
    }

    [Fact]
    public void The_maximum_fee_for_one_day_is_60_SEK()
    {
        // Arrange

        var passings = Get24TollablePassings();

        var tollCalculator = new TollCalculator();

        // Act & Assert
        tollCalculator.GetTollFee(passings, 6).Should().Be(60);
    }

    [Fact]
    public void Rush_hour_traffic_will_render_highest_fee()
    {
        var passings = new DateTime[]{
            new DateTime(2013, 1, 4, 7, 30, 0),
            new DateTime(2013, 1, 4, 16, 30, 0),
        };

        var tollCalculator = new TollCalculator();

        // Act & Assert
        tollCalculator.GetTollFee(passings, 6).Should().Be(36);
    }


    [Fact]
    public void A_vehicle_should_only_be_charged_once_an_hour_but_the_highest_fee()
    {
        var midTraffic = new DateTime(2013, 1, 4, 6, 30, 0);
        var rushHourTraffic = new DateTime(2013, 1, 4, 7, 1, 59);

        var tollCalculator = new TollCalculator();

        // Act & Assert
        tollCalculator.GetTollFee(new[] { midTraffic, rushHourTraffic }, 6).Should().Be(18);
    }

    private static IEnumerable<DateTime> Get24TollablePassings()
    {
        var passings = new List<DateTime>();
        for (int i = 0; i < 23; i++)
        {
            passings.Add(new DateTime(2013, 2, 1, i, 0, 0));
        }
        return passings;
    }
}
