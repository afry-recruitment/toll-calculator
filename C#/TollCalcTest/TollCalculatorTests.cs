using Xunit;
using FluentAssertions;
using Toll_Calculator.Vehicles;



public class TollCalculatorTests
{

    [Fact]
    public void GetHighestAmoutInAnHour_ShouldReturnHighestToll()
    {
        var calc = new TollCalculator();
        var holidayList = new List<DateTime>
        {
            new DateTime(2024, 11, 29, 8, 0, 0),
            new DateTime(2024, 11, 29, 8, 15, 0),
            new DateTime(2024, 11, 29, 8, 31, 0),
        };

        var feeTogether = calc.GetTollFee(new Car(), holidayList.ToArray());
        feeTogether.Should().Be(13);

    }

    [Fact]
    public void GetHighestAmoutInADay_ShouldReturnMaxPerDay()
    {
        var calc = new TollCalculator();
        var holidayList = new List<DateTime>
        {
            new DateTime(2024, 11, 29, 8, 0, 0),
            new DateTime(2024, 11, 29, 8, 31, 0),
            new DateTime(2024, 11, 29, 9, 15, 0),
            new DateTime(2024, 11, 29, 10, 31, 0),
            new DateTime(2024, 11, 29, 11, 15, 0),
            new DateTime(2024, 11, 29, 12, 31, 0),
            new DateTime(2024, 11, 29, 13, 15, 0),
            new DateTime(2024, 11, 29, 14, 31, 0),
            new DateTime(2024, 11, 29, 15, 31, 0),
            new DateTime(2024, 11, 29, 16, 15, 0),
            new DateTime(2024, 11, 29, 17, 31, 0),
        };

        var feeTogether = calc.GetTollFee(new Car(), holidayList.ToArray());
        feeTogether.Should().Be(60);

    }


    [Fact]
    public void GetTollForFreeVehicle_ShouldReturnZero()
    {
        var calc = new TollCalculator();
        var freeVehicleList = new List<Vehicle>
        {
            new Diplomat(),
            new Emergency(),
            new Foreign(),
            new Military(),
            new Motorbike(),
            new Tractor(),
        };

        foreach(var v in freeVehicleList)
        {
            var fee = calc.GetTollFee(v, [new DateTime(2024, 11, 29, 10, 00, 0)]);
            fee.Should().Be(0);
        }
    }

    [Fact]
    public void GetTollForHolidays_ShouldReturnZero()
    {
        var calc = new TollCalculator();
        var holidayList = new List<DateTime>
        {
            new DateTime(2024, 1, 1, 8, 30, 0),
            new DateTime(2024, 1, 6, 8, 30, 0),
            new DateTime(2024, 3, 28, 8, 30, 0),
            new DateTime(2024, 4, 1, 8, 30, 0),
            new DateTime(2024, 5, 9, 8, 30, 0),
            new DateTime(2024, 5, 1, 8, 30, 0),
            new DateTime(2024, 6, 6, 8, 30, 0),
            new DateTime(2024, 6, 21, 8, 30, 0),
            new DateTime(2024, 7, 1, 8, 30, 0),
            new DateTime(2024, 12, 24, 8, 30, 0),
            new DateTime(2024, 11, 23, 8, 30, 0),
            new DateTime(2024, 11, 24, 8, 30, 0),
        };

        foreach(var h in holidayList)
        {
            var fee = calc.GetTollFee(new Car(), [h]);
            fee.Should().Be(0);
        }
        var feeTogether = calc.GetTollFee(new Car(), holidayList.ToArray());
        feeTogether.Should().Be(0);

    }
    [Fact]
    public void GetTollForTimesOutsideFeeSpan_ShouldReturnZero()
    {
        var calc = new TollCalculator();
        var holidayList = new List<DateTime>
        {
            new DateTime(2024, 11, 29, 5, 20, 0),
            new DateTime(2024, 11, 29, 18, 31, 0),
        };

        foreach (var h in holidayList)
        {
            var fee = calc.GetTollFee(new Car(), [h]);
            fee.Should().Be(0);
        }
        var feeTogether = calc.GetTollFee(new Car(), holidayList.ToArray());
        feeTogether.Should().Be(0);

    }

    [Fact]
    public void GetTollForMultipleDays_ShouldReturnCorrectValue()
    {
        var calc = new TollCalculator();
        var holidayList = new List<DateTime>
        {
            new DateTime(2024, 11, 28, 7, 0, 0),
            new DateTime(2024, 11, 28, 8, 15, 0),
            new DateTime(2024, 11, 28, 8, 31, 0),
            new DateTime(2024, 11, 28, 10, 00, 0),
            new DateTime(2024, 11, 28, 14, 00, 0),
            new DateTime(2024, 11, 28, 20, 00, 0),

            new DateTime(2024, 11, 29, 7, 1, 0), 
            new DateTime(2024, 11, 29, 8, 16, 0),
            new DateTime(2024, 11, 29, 8, 32, 0),
        };

        var feeTogether = calc.GetTollFee(new Car(), holidayList.ToArray());
        feeTogether.Should().Be(78);

    }

}

