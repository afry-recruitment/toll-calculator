﻿using TollCalculator.Models;
using TollCalculator.Tests.TestData;

namespace TollCalculator.Tests
{
    public class TollFeeCalculatorTests
    {
        [Fact]
        public void NewTollFee_WithTollFreeVehicle_ShouldReturnTollFeeAmountZero()
        {
            var carTollFree = new Car("ABC-123", Vehicle.VehicleSector.Military);
            var tollDate = new DateTime(2023, 1, 2, 6, 15, 0);

            var result = TollFeeCalculator.NewTollFee(carTollFree, tollDate);

            Assert.Equal(0, result.TollFeeAmount);
        }

        [Fact]
        public void NewTollFee_WithTollFreeDate_ShouldReturnTollFeeAmountZero()
        {
            var car = new Car("ABC-123", Vehicle.VehicleSector.Civilian);
            var tollFreeDate = new DateTime(2023, 1, 1, 6, 15, 0);

            var result = TollFeeCalculator.NewTollFee(car, tollFreeDate);

            Assert.Equal(0, result.TollFeeAmount);
        }

        [Fact]
        public void NewTollFee_WithoutTollFreeVehicle_WithoutTollFreeDate_ShouldNotReturnTollFeeAmountZero()
        {
            var car = new Car("ABC-123", Vehicle.VehicleSector.Civilian);
            var tollDate = new DateTime(2023, 1, 2, 6, 15, 0);

            var result = TollFeeCalculator.NewTollFee(car, tollDate);

            Assert.NotEqual(0, result.TollFeeAmount);
        }

        [Theory]
        [ClassData(typeof(HourlyTollFeeTestData))]
        public void GetTollFeeAmount(DateTime timeslot, int expected)
        {
            var actual = TollFeeCalculator.GetTollFeeAmount(timeslot);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [ClassData(typeof(PublicHolidayTestData))]
        public void IsTollFreeDate_SwedishPublicHolidaysAndWeekends_ShouldReturnTrue(DateTime date)
        {
            var result = TollFeeCalculator.IsTollFreeDate(date);
            Assert.True(result);
        }
    }
}
