using NSubstitute;
using Tollculator.Calculator;
using Tollculator.Services;
using Tollculator.WorkingDayChecker;

namespace Tollculator.Tests;

public class TollCalculatorTest
{
    private readonly TollCalculator _calculator;
    private readonly IWorkingDayChecker _workingDayChecker;
    private readonly IVehicleService _vehicleService;

    public TollCalculatorTest()
    {
        _workingDayChecker = Substitute.For<IWorkingDayChecker>();
        _vehicleService = Substitute.For<IVehicleService>();

        _calculator = new TollCalculator(_workingDayChecker, _vehicleService);
    }

    [Fact]
    public async Task Should_calculate_toll_correctly()
    {
        _workingDayChecker.IsTollFree(Arg.Any<DateOnly>()).Returns(false);
        _vehicleService.IsTollFree(Arg.Any<string>()).Returns(false);

        var result = await _calculator.GetTollFee(
            new DateOnly(2024, 01, 01),
            "GMF069",
            [
                new TimeOnly(14, 31),//First hour start 14:31
                new TimeOnly(14, 55),
                new TimeOnly(15, 29),//First end. The highest one = 13
                new TimeOnly(16, 00),//Second hour period starts 1600
                new TimeOnly(16, 31),//Second end. The highest = 18
                new TimeOnly(17, 30),//Third start 1730
                new TimeOnly(17, 35),
                new TimeOnly(18, 00),
                new TimeOnly(18, 05),//Third end. The highest = 13
            ]);

        Assert.Equal(13 + 18 + 13, result);
    }

    [Fact]
    public async Task Should_be_max_60()
    {
        _workingDayChecker.IsTollFree(Arg.Any<DateOnly>()).Returns(false);
        _vehicleService.IsTollFree(Arg.Any<string>()).Returns(false);

        var result = await _calculator.GetTollFee(
            new DateOnly(2024, 01, 01),
            "GMF069",
            [
                new TimeOnly(06, 00),
                new TimeOnly(09, 00),
                new TimeOnly(10, 00),
                new TimeOnly(11, 00),
                new TimeOnly(12, 00),
                new TimeOnly(13, 00),
                new TimeOnly(14, 00),
                new TimeOnly(18, 00),
            ]);

        Assert.Equal(60, result);
    }

    [Fact]
    public async Task Should_return_zero_if_toll_free_day()
    {
        _workingDayChecker.IsTollFree(Arg.Any<DateOnly>()).Returns(true);
        _vehicleService.IsTollFree(Arg.Any<string>()).Returns(false);

        var result = await _calculator.GetTollFee(
           new DateOnly(2024, 01, 01),
           "GMF069",
           [
               new TimeOnly(14, 31),
           ]);

        Assert.Equal(0, result);
    }

    [Fact]
    public async Task Should_return_zero_if_toll_free_vehicle()
    {
        _workingDayChecker.IsTollFree(Arg.Any<DateOnly>()).Returns(false);
        _vehicleService.IsTollFree(Arg.Any<string>()).Returns(true);

        var result = await _calculator.GetTollFee(
           new DateOnly(2024, 01, 01),
           "GMF069",
           [
               new TimeOnly(14, 31),
           ]);

        Assert.Equal(0, result);
    }
}
