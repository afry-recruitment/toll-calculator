using TollCalculator.Exceptions;
using TollCalculator.Models;
using TollCalculator.Tests.TestData;

namespace TollCalculator.Tests
{
    public class TollFeeCalculatorTests
    {
        [Fact]
        public void CalculateTotalDailyTollFee_WithTollFeeDates_NotFromToday_ShouldReturnException()
        {
            var car = new Car("ABC-123", Vehicle.VehicleSector.Civilian);
            var firstTollDate = DateTime.Now;
            var secondTollDate = DateTime.Now.AddDays(2);
            var firstTollFee = TollFeeCalculator.NewTollFee(car, firstTollDate);
            var secondTollFee = TollFeeCalculator.NewTollFee(car, secondTollDate);
            var tollFees = new TollFee[2] { firstTollFee, secondTollFee };
            var bypassDateTime = DateTime.MinValue;

            Assert.Throws<TollDateDayException>(() => TollFeeCalculator.CalculateTotalDailyTollFee(car, tollFees, bypassDateTime));
        }

        [Fact]
        public void CalculateTotalDailyTollFee_WithTollFeeLicensePlates_FromDifferentVehicles_ShouldReturnException()
        {
            var firstCar = new Car("ABC-123", Vehicle.VehicleSector.Civilian);
            var secondCar = new Car("AAA-123", Vehicle.VehicleSector.Civilian);
            var tollDate = DateTime.Now;
            var firstTollFee = TollFeeCalculator.NewTollFee(firstCar, tollDate);
            var secondTollFee = TollFeeCalculator.NewTollFee(secondCar, tollDate);
            var tollFees = new TollFee[2] { firstTollFee, secondTollFee };
            var bypassDateTime = DateTime.MinValue;

            Assert.Throws<VehicleLicensePlateException>(() => TollFeeCalculator.CalculateTotalDailyTollFee(firstCar, tollFees, bypassDateTime));
        }

        [Fact]
        public void NewTollFee_WithTollFreeVehicle_ShouldReturnTollFeeAmountZero()
        {
            var carTollFree = new Car("ABC-123", Vehicle.VehicleSector.Military);
            var tollDate = new DateTime(2023, 1, 2, 6, 15, 0);

            var actual = TollFeeCalculator.NewTollFee(carTollFree, tollDate);

            Assert.Equal(0, actual.TollFeeAmount);
        }

        [Fact]
        public void NewTollFee_WithTollFreeDate_ShouldReturnTollFeeAmountZero()
        {
            var car = new Car("ABC-123", Vehicle.VehicleSector.Civilian);
            var tollFreeDate = new DateTime(2023, 1, 1, 6, 15, 0);

            var actual = TollFeeCalculator.NewTollFee(car, tollFreeDate);

            Assert.Equal(0, actual.TollFeeAmount);
        }

        [Fact]
        public void NewTollFee_WithoutTollFreeVehicle_WithoutTollFreeDate_ShouldNotReturnTollFeeAmountZero()
        {
            var car = new Car("ABC-123", Vehicle.VehicleSector.Civilian);
            var tollDate = new DateTime(2023, 1, 2, 6, 15, 0);

            var actual = TollFeeCalculator.NewTollFee(car, tollDate);

            Assert.NotEqual(0, actual.TollFeeAmount);
        }

        [Fact]
        public void NewTollFee_WithoutCurrentYear_ShouldThrowException()
        {
            var car = new Car("ABC-123", Vehicle.VehicleSector.Civilian);
            var tollDate = new DateTime(2021, 1, 2, 6, 15, 0);

            var actual = TollFeeCalculator.NewTollFee(car, tollDate);

            Assert.Throws<NotCurrentYearException>(() => TollFeeCalculator.NewTollFee(car, tollDate));
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
