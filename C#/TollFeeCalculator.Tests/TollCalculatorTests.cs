using System;
using TollFeeCalculator;
using TollFeeCalculator.Policies;
using TollFeeCalculator.Services;
using Xunit;

namespace TollFeeCalculatorTests;

public class TollCalculatorTests
{
    private readonly TollCalculator calculator = new();
    private readonly Car car = new Car();
    [Fact]
    public void GetTollFee_UsesHighestFeeWithinOneHour()
    {
        DateTime[] dates =
        [
            new DateTime(2013, 2, 4, 7, 0, 0),
            new DateTime(2013, 2, 4, 6, 5, 0)
        ];

        Assert.Equal(18, calculator.GetTollFee(car, dates));
    }

    [Fact]
    public void GetTollFee_ChargesAgainAfterOneHour()
    {
        DateTime[] dates =
        [
            new DateTime(2013, 2, 4, 6, 0, 0),
            new DateTime(2013, 2, 4, 7, 1, 0)
        ];

        Assert.Equal(26, calculator.GetTollFee(car, dates));
    }

    [Theory]
    [InlineData(6, 0, 8)]
    [InlineData(6, 30, 13)]
    [InlineData(7, 0, 18)]
    [InlineData(8, 0, 13)]
    [InlineData(8, 30, 8)]
    [InlineData(9, 15, 8)]
    [InlineData(15, 0, 13)]
    [InlineData(15, 30, 18)]
    [InlineData(17, 0, 13)]
    [InlineData(18, 0, 8)]
    [InlineData(18, 30, 0)]
    public void GetTollFee_UsesPublishedFeeBands(int hour, int minute, int expectedFee)
    {
        DateTime date = new(2013, 2, 4, hour, minute, 0);

        Assert.Equal(expectedFee, calculator.GetTollFee(date, car));
    }

    [Fact]
    public void GetTollFee_AppliesDailyMaximum()
    {
        DateTime[] dates =
        [
            new DateTime(2013, 2, 4, 7, 0, 0),
            new DateTime(2013, 2, 4, 8, 1, 0),
            new DateTime(2013, 2, 4, 15, 30, 0),
            new DateTime(2013, 2, 4, 17, 1, 0),
            new DateTime(2013, 2, 4, 18, 1, 0)
        ];

        Assert.Equal(60, calculator.GetTollFee(car, dates));
    }

    [Fact]
    public void GetTollFee_MotorbikesWeekendsAndHolidaysAreFree()
    {
        Assert.Equal(0, calculator.GetTollFee(new DateTime(2013, 2, 4, 7, 0, 0), new Motorbike()));
        Assert.Equal(0, calculator.GetTollFee(new DateTime(2013, 2, 9, 7, 0, 0), car));
        Assert.Equal(0, calculator.GetTollFee(new DateTime(2013, 1, 1, 7, 0, 0), car));
    }

    [Fact]
    public void GetTollFee_EmptyDatesAreFree()
    {
        Assert.Equal(0, calculator.GetTollFee(car, []));
    }

    [Fact]
    public void TollCalculator_CanUseCustomPolicies()
    {
        var service = new TollFeeService(
            new FixedFeeSchedule(42),
            new NoTollFreeVehicles(),
            new NoHolidays());
        var configuredCalculator = new TollCalculator(service);

        Assert.Equal(42, configuredCalculator.GetTollFee(
            new DateTime(2026, 9, 30, 12, 0, 0), car));
    }

    private class FixedFeeSchedule(int fee) : ITollFeeSchedule
    {
        public int GetFee(DateTime date) => fee;
    }

    private class NoTollFreeVehicles : ITollFreeVehiclePolicy
    {
        public bool IsTollFree(Vehicle vehicle) => false;
    }

    private class NoHolidays : IHolidayCalendar
    {
        public bool IsTollFree(DateTime date) => false;
    }
}
