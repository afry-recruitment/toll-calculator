using TollFeeCalculator.Exceptions;
using TollFeeCalculator.Models;
using TollFeeCalculator.Policies;

namespace TollFeeCalculator.UnitTests;

public class CalculateTollFeeServiceTests
{
    private readonly CalculateTollFeeService _sut = new(new TestTollFeePolicy());

    [Fact]
    public void CalculateTotalTollFeeForDate_VehicleTypeIsTollFree_NoFee()
    {
        var passageTimes = new[]
        {
                new TimeOnly(6,0),
        };

        var result = _sut.CalculateTotalTollFee(VehicleType.Emergency, new DateOnly(2025, 03, 12), passageTimes);

        Assert.NotNull(result);
        Assert.Equal(0, result.TotalFee);
    }

    [Fact]
    public void CalculateTotalTollFeeForDate_DateIsTollFree_NoFee()
    {
        var passageTimes = new[]
        {
            new TimeOnly(6,0),
        };

        var result = _sut.CalculateTotalTollFee(VehicleType.Car, new DateOnly(2025, 03, 15), passageTimes);

        Assert.NotNull(result);
        Assert.Equal(0, result.TotalFee);
    }

    [Fact]
    public void CalculateTotalTollFeeForDate_ThreeTollablePassagesAndOneTollFree_ExpectedResult()
    {
        var passageTimes = new[]
        {
            new TimeOnly(0,0,32),
            new TimeOnly(9,29,36),
            new TimeOnly(12,13,56),
            new TimeOnly(22,1,12),
        };

        var date = new DateOnly(2025, 03, 13);

        var result = _sut.CalculateTotalTollFee(VehicleType.Car, date, passageTimes);

        Assert.NotNull(result);
        Assert.Equal(6, result.TotalFee);
        Assert.Equal("SEK", result.Currency);
        Assert.Equal(date, result.Date);
    }

    [Fact]
    public void CalculateTotalTollFeeForDate_TotalFeeExceedsDailyMax_TotalFeeIsDailyMax()
    {
        var passageTimes = new[]
        {
            new TimeOnly(0,0,3),
            new TimeOnly(9,29,59),
            new TimeOnly(12,59,8),
            new TimeOnly(21,15,35),
            new TimeOnly(22,20,32),
            new TimeOnly(23,59,59,999),
        };

        var result = _sut.CalculateTotalTollFee(VehicleType.Car, new DateOnly(2025, 03, 13), passageTimes);

        Assert.NotNull(result);
        Assert.Equal(10, result.TotalFee);
    }

    [Fact]
    public void CalculateTotalTollFeeForDate_TwoPassesWithinTheSameHour_HighestFeeIsSelectedAndAddedToTotal()
    {
        var passageTimes = new[]
        {
            new TimeOnly(6,0,3,456),
            new TimeOnly(9,29,56,456),
            new TimeOnly(9,59,59,999),
        };

        var result = _sut.CalculateTotalTollFee(VehicleType.Car, new DateOnly(2025, 03, 13), passageTimes);

        Assert.NotNull(result);
        Assert.Equal(3, result.TotalFee);
    }

    [Fact]
    public void CalculateTotalTollFeeForDate_MissingRateInterval_ExpectedExceptionIsThrown()
    {
        var passageTimes = new[]
        {
            new TimeOnly(5,0,34,879),
        };

        Assert.Throws<MissingTollFeeRateException>(() =>
            _sut.CalculateTotalTollFee(VehicleType.Car, new DateOnly(2025, 03, 13), passageTimes));
    }

    private class TestTollFeePolicy : ITollFeePolicy
    {
        public string Currency => "SEK";
        public decimal DailyMaxFee => 10;

        public TollFeeRate[] TollFeeRates =>
        [
            new(new TimeOnly(6, 0), new TimeOnly(9, 29), 1),
            new(new TimeOnly(9, 30), new TimeOnly(12, 59), 2),
            new(new TimeOnly(13, 0), new TimeOnly(23, 59), 3),
            new(new TimeOnly(0, 0), new TimeOnly(4, 29), 0),
        ];

        public bool IsTollFreeDate(DateOnly date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        public bool IsTollFreeVehicleType(VehicleType vehicleType)
        {
            return vehicleType != VehicleType.Car && vehicleType != VehicleType.Motorbike;
        }
    }
}
