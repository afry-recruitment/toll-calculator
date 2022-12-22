using FluentAssertions;
using TrafficToll.Internals.DataAccess;
using TrafficToll.Internals.Services;
using Xunit;

namespace TrafficToll.tests;


public class AcceptanceTests
{
    [Fact]
    public void Some_vehicle_types_are_fee_free()
    {
        // Arrange
        var tollCalculator = new TollCalculator();
        var passings = Get23ValidPassings();

        // Act & Assert
        tollCalculator.GetTollFee(passings, 0).Should().Be(0);
        tollCalculator.GetTollFee(passings, 1).Should().Be(0);
        tollCalculator.GetTollFee(passings, 2).Should().Be(0);
        tollCalculator.GetTollFee(passings, 3).Should().Be(0);
        tollCalculator.GetTollFee(passings, 4).Should().Be(0);
        tollCalculator.GetTollFee(passings, 5).Should().Be(0);
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

    }

    [Fact]
    public void The_maximum_fee_for_one_day_is_60_SEK()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Rush_hour_traffic_will_render_highest_fee()
    {
        throw new NotImplementedException();
    }


    [Fact]
    public void A_vehicle_should_only_be_charged_once_an_hour_but_the_highest_fee()
    {
        throw new NotImplementedException();
    }

    private static IEnumerable<DateTime> Get23ValidPassings()
    {
        var passings = new List<DateTime>();
        for (int i = 0; i < 23; i++)
        {
            passings.Add(new DateTime(2013, 2, 1, i, 0, 0));
        }
        return passings;
    }
}
