using TollCalculator.Exceptions;
using TollCalculator.Models;
using TollCalculator.Tests.TestData;

namespace TollCalculator.Tests
{
    public class TollFeeCalculatorTests
    {
        [Fact]
        public void CalculateTotalDailyTollFee_ValuesWithin60Min_ShouldReturnHighestFee()
        {
            var car = new Car("ABC-123", Vehicle.VehicleSector.Civilian);
            var firstTollDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 45, 0);
            var secondTollDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 15, 0);
            var firstTollFee = TollFeeCalculator.NewTollFee(car, firstTollDate);
            var secondTollFee = TollFeeCalculator.NewTollFee(car, secondTollDate);
            var tollFees = new TollFee[2] { firstTollFee, secondTollFee };
            var bypassDateTime = DateTime.MinValue;
            var expected = 18;

            var actual = TollFeeCalculator.CalculateTotalDailyTollFee(car, tollFees, bypassDateTime);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CalculateTotalDailyTollFee_ValuesNotWithin60Min_ShouldReturnSum()
        {
            var car = new Car("ABC-123", Vehicle.VehicleSector.Civilian);
            var firstTollDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 45, 0);
            var secondTollDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 15, 0);
            var firstTollFee = TollFeeCalculator.NewTollFee(car, firstTollDate);
            var secondTollFee = TollFeeCalculator.NewTollFee(car, secondTollDate);
            var tollFees = new TollFee[2] { firstTollFee, secondTollFee };
            var bypassDateTime = DateTime.MinValue;
            var expected = 21;

            var actual = TollFeeCalculator.CalculateTotalDailyTollFee(car, tollFees, bypassDateTime);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CalculateTotalDailyTollFee_TotalDailyTollFeeExceeded_ShouldReturn60()
        {
            var car = new Car("ABC-123", Vehicle.VehicleSector.Civilian);
            var firstTollDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 45, 0);
            var secondTollDate = firstTollDate.AddHours(1.05);
            var thirdTollDate = secondTollDate.AddHours(1.05);
            var fourthTollDate = thirdTollDate.AddHours(1.05);
            var fifthTollDate = fourthTollDate.AddHours(1.05);
            var sixthTollDate = fifthTollDate.AddHours(1.05);
            var tollFees = new TollFee[]
            {
                TollFeeCalculator.NewTollFee(car, firstTollDate),
                TollFeeCalculator.NewTollFee(car, secondTollDate),
                TollFeeCalculator.NewTollFee(car, thirdTollDate),
                TollFeeCalculator.NewTollFee(car, fourthTollDate),
                TollFeeCalculator.NewTollFee(car, fifthTollDate),
                TollFeeCalculator.NewTollFee(car, sixthTollDate),
            };
            var bypassDateTime = DateTime.MinValue;
            var expected = 60;

            var actual = TollFeeCalculator.CalculateTotalDailyTollFee(car, tollFees, bypassDateTime);

            Assert.Equal(expected, actual);
        }

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
        public void CalculateTotalDailyTollFee_WithTollFeeLicensePlates_FromDifferentVehicles_ShouldThrowException()
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
            var expected = 0;

            var actual = TollFeeCalculator.NewTollFee(carTollFree, tollDate).TollFeeAmount;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NewTollFee_WithTollFreeDate_ShouldReturnTollFeeAmountZero()
        {
            var car = new Car("ABC-123", Vehicle.VehicleSector.Civilian);
            var tollFreeDate = new DateTime(2023, 1, 1, 6, 15, 0);
            var expected = 0;

            var actual = TollFeeCalculator.NewTollFee(car, tollFreeDate).TollFeeAmount;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NewTollFee_WithoutTollFreeVehicle_WithoutTollFreeDate_ShouldNotReturnTollFeeAmount()
        {
            var car = new Car("ABC-123", Vehicle.VehicleSector.Civilian);
            var tollDate = new DateTime(2023, 1, 2, 6, 15, 0);
            var expected = 8;

            var actual = TollFeeCalculator.NewTollFee(car, tollDate);

            Assert.Equal(expected, actual.TollFeeAmount);
        }

        [Fact]
        public void NewTollFee_WithoutCurrentYear_ShouldThrowException()
        {
            var car = new Car("ABC-123", Vehicle.VehicleSector.Civilian);
            var tollDate = new DateTime(2021, 1, 2, 6, 15, 0);

            Assert.Throws<NotCurrentYearException>(() => TollFeeCalculator.NewTollFee(car, tollDate));
        }

        // Tests that every hourly fee returns correct value
        [Theory]
        [ClassData(typeof(GetTollFeeAmountTestData))]
        public void GetTollFeeAmount(DateTime timeslot, int expected)
        {
            var actual = TollFeeCalculator.GetTollFeeAmount(timeslot);
            Assert.Equal(expected, actual);
        }

        // Tests all public holidays and Saturday + Sunday
        [Theory]
        [ClassData(typeof(IsTollFreeDateTestData))]
        public void IsTollFreeDate(DateTime date)
        {
            var result = TollFeeCalculator.IsTollFreeDate(date);
            Assert.True(result);
        }
    }
}
