using Xunit;
using TollFeeCalculator;
using System;
using Moq;

namespace TollFeeCalculator.Tests
{
    public class TollCalculatorTests
    {
        private readonly TollCalculator _calculator;

        public TollCalculatorTests()
        {
            _calculator = new TollCalculator();
        }

        [Theory]
        [InlineData(6, 0, 8)]    // Start of 8 SEK 
        [InlineData(6, 29, 8)]   // End of 8 SEK 
        [InlineData(6, 30, 13)]  // Start of 13 SEK 
        [InlineData(7, 0, 18)]   // Start of 18 SEK (Rush hour)
        [InlineData(8, 29, 13)]  // End of 13 SEK 
        [InlineData(8, 30, 8)]   // Start of mid-day 8 SEK
        [InlineData(14, 59, 8)]  // End of mid-day 8 SEK
        [InlineData(15, 0, 13)]  // Start of afternoon 13 SEK
        [InlineData(18, 0, 8)]   // Start of last 8 SEK 
        [InlineData(18, 29, 8)]  // End of last 8 SEK 
        [InlineData(18, 30, 0)]  // Start of free evening
        public void GetTollFee_ShouldReturnCorrectFee_BasedOnTime(int hour, int minute, int expectedFee)
        {
            // Arrange
            var car = new Vehicle(VehicleType.Car);

            var dateTime = new DateTime(2013, 1, 2, hour, minute, 0);

            // Act
            int actualFee = _calculator.GetTollFee(dateTime, car);

            // Assert
            Assert.Equal(expectedFee, actualFee);
        }

        [Fact]
        public void GetTotalTollFee_ShouldPickHighestFee_WhenRushHourOccursWithinSameHour()
        {
            // Arrange
            var car = new Vehicle(VehicleType.Car);

            DateTime[] passages = {
                new DateTime(2013, 1, 2, 6, 20, 0),
                new DateTime(2013, 1, 2, 7, 10, 0)
            };

            // Act
            int totalFee = _calculator.GetTotalTollFee(car, passages);

            // Assert
            int expectedFee = 18;
            Assert.Equal(expectedFee, totalFee);
        }

        [Fact]
        public void GetTotalTollFee_ShouldCapAtSixty_WhenDailyTotalExceedsLimit()
        {
            // Arrange
            var car = new Vehicle(VehicleType.Car);

            DateTime[] passages = {
                new DateTime(2013, 1, 2, 7, 0, 0),      // 07:00    (18 SEK)
		        new DateTime(2013, 1, 2, 9, 0, 0),      // 09:00    (8 SEK)
		        new DateTime(2013, 1, 2, 11, 0, 0),     // 11:00    (8 SEK)
		        new DateTime(2013, 1, 2, 13, 0, 0),     // 13:00    (8 SEK)
		        new DateTime(2013, 1, 2, 15, 30, 0),    // 15:30    (18 SEK)
		        new DateTime(2013, 1, 2, 17, 30, 0)     // 17:30    (13 SEK)
	        };

            // Act
            int totalFee = _calculator.GetTotalTollFee(car, passages);

            // Assert
            int expectedFee = 60;
            Assert.Equal(expectedFee, totalFee);
        }

        [Fact]
        public void GetTotalTollFee_ShouldChargeOnlyOncePerHour_AndPickHighestFee()
        {
            // Arrange
            var car = new Vehicle(VehicleType.Car);
            DateTime[] passages = {
                new DateTime(2013, 1, 2, 6, 15, 0), // 06:15    (8 SEK)   
                new DateTime(2013, 1, 2, 6, 45, 0)  // 06:45    (13 SEK)
            };

            // Act
            int totalFee = _calculator.GetTotalTollFee(car, passages);

            // Assert
            int expectedFee = 13;
            Assert.Equal(expectedFee, totalFee);
        }

        [Fact]
        public void GetTotalTollFee_ShouldChargeTwice_IfPassagesAreExactlySixtyOneMinutesApart()
        {
            // Arrange
            var car = new Vehicle(VehicleType.Car);
            DateTime[] passages = {
                new DateTime(2013, 1, 2, 7, 0, 0),  // 07:00    (18 SEK)
                new DateTime(2013, 1, 2, 8, 1, 0)   // 08:01    (13 SEK)
            };

            // Act
            int totalFee = _calculator.GetTotalTollFee(car, passages);

            // Assert
            int expectedFee = 31;
            Assert.Equal(expectedFee, totalFee);
        }


        [Theory]
        [InlineData(VehicleType.Motorbike)]
        [InlineData(VehicleType.Tractor)]
        [InlineData(VehicleType.Emergency)]
        [InlineData(VehicleType.Diplomat)]
        [InlineData(VehicleType.Foreign)]
        [InlineData(VehicleType.Military)]
        public void GetTotalTollFee_ShouldBeFree_ForExemptVehicleTypes(VehicleType type)
        {
            // Arrange
            var vehicle = new Vehicle(type);
            var times = new DateTime[] { new DateTime(2013, 1, 2, 7, 0, 0) }; // Rush hour

            // Act
            int totalFee = _calculator.GetTotalTollFee(vehicle, times);

            // Assert
            int expectedFee = 0;
            Assert.Equal(expectedFee, totalFee);
        }

        [Theory]
        [InlineData(2025, 1, 4)]    // Lördag 4 januari
        [InlineData(2025, 1, 5)]    // Söndag 5 januari
        [InlineData(2025, 1, 1)]    // Nyårsdagen: Onsdag 1 januari
        [InlineData(2025, 1, 6)]    // Trettondagen: Måndag 6 januari
        [InlineData(2025, 4, 18)]   // Långfredagen: Fredag 18 april
        [InlineData(2025, 4, 20)]   // Påskdagen: Söndag 20 april
        [InlineData(2025, 4, 21)]   // Annandag påsk: Måndag 21 april
        [InlineData(2025, 5, 1)]    // Första maj: Torsdag 1 maj
        [InlineData(2025, 5, 29)]   // Kristi himmelsfärdsdag: Torsdag 29 maj 
        [InlineData(2025, 6, 6)]    // Sveriges nationaldag: Fredag 6 juni
        [InlineData(2025, 6, 8)]    // Pingstdagen: Söndag 8 juni
        [InlineData(2025, 6, 21)]   // Midsommardagen: Lördag 21 juni
        [InlineData(2025, 7, 21)]   // En dag i juli: 21 juli
        [InlineData(2025, 12, 25)]  // Juldagen: Torsdag 25 december
        [InlineData(2025, 12, 26)]   // Annandag jul: Fredag 26 december 
        public void GetTotalTollFee_ShouldBeFree_OnWeekendsHolidaysAndJuly(int year, int month, int day)
        {
            // Arrange
            var car = new Vehicle(VehicleType.Car);
            var passages = new DateTime[] { new DateTime(year, month, day, 14, 0, 0) };

            // Act
            int totalFee = _calculator.GetTotalTollFee(car, passages);

            // Assert
            Assert.Equal(0, totalFee);
        }

        [Fact]
        public void GetTotalTollFee_ShouldResetCap_AtMidnight()
        {
            // Arrange
            var car = new Vehicle(VehicleType.Car);
            DateTime[] passages = {
		        // Monday
		        new DateTime(2013, 1, 14, 7, 0, 0),     // 18 SEK
		        new DateTime(2013, 1, 14, 9, 0, 0),     // 8 SEK
		        new DateTime(2013, 1, 14, 11, 0, 0),    // 8 SEK
		        new DateTime(2013, 1, 14, 13, 0, 0),    // 8 SEK
		        new DateTime(2013, 1, 14, 15, 30, 0),   // 18 SEK
		        new DateTime(2013, 1, 14, 17, 30, 0),   // 13 SEK -> Total 73, capped to 60
		
		        // Tuesday
		        new DateTime(2013, 1, 15, 7, 0, 0),  // 18 SEK
	        };

            // Act
            int totalFee = _calculator.GetTotalTollFee(car, passages);

            // Assert
            // 60 + 18 == 78
            int expectedFee = 78;
            Assert.Equal(expectedFee, totalFee);
        }

        [Theory]
        [InlineData(18, 29, 8)]  // Last minute of the 18:00 window
        [InlineData(18, 30, 0)]  // First minute of the free evening
        [InlineData(23, 59, 0)]  // Just before midnight
        [InlineData(05, 59, 0)]  // Just before the morning toll starts
        public void GetTollFee_ShouldHandleBoundaryTimesCorrectly(int hour, int minute, int expectedFee)
        {
            // Arrange
            var car = new Vehicle(VehicleType.Car);
            var requestTime = new DateTime(2013, 1, 2, hour, minute, 0);

            // Act
            int fee = _calculator.GetTollFee(requestTime, car);

            // Assert
            Assert.Equal(expectedFee, fee);
        }

    }
}
