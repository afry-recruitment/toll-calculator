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
            var car = new Car();

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
            var car = new Car();

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
            var car = new Car();

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
            var car = new Car();
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
            var car = new Car();
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
        [InlineData("Motorbike")]
        [InlineData("Tractor")]
        [InlineData("Emergency")]
        [InlineData("Diplomat")]
        [InlineData("Foreign")]
        [InlineData("Military")]
        public void GetTotalTollFee_ShouldReturnZero_ForExemptVehicles(string vehicleType)
        {
            // Arrange
            var mockVehicle = new Mock<Vehicle>();
            mockVehicle.Setup(v => v.GetVehicleType()).Returns(vehicleType);
            var rushHour = new DateTime[] { new DateTime(2013, 1, 2, 7, 0, 0) };    // 07:00

            // Act
            int totalFee = _calculator.GetTotalTollFee(mockVehicle.Object, rushHour);

            // Assert
            int expectedFee = 0;
            Assert.Equal(expectedFee, totalFee);
        }

        [Theory]
        [InlineData(2013, 1, 1)]   // New Year's - Free
        [InlineData(2013, 7, 14)]  // July - Free
        [InlineData(2013, 11, 1)]  // All Saints - Free
        public void GetTotalTollFee_ShouldBeFree_OnWeekendsHolidaysAndJuly(int year, int month, int day)
        {
            // Arrange
            var car = new Car();
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
            var car = new Car();
            DateTime[] passages = {
		        // Monday - hits the 60 SEK cap
		        new DateTime(2013, 1, 14, 7, 0, 0),  // 18 SEK
		        new DateTime(2013, 1, 14, 9, 0, 0),  // 8 SEK
		        new DateTime(2013, 1, 14, 11, 0, 0), // 8 SEK
		        new DateTime(2013, 1, 14, 13, 0, 0), // 8 SEK
		        new DateTime(2013, 1, 14, 15, 30, 0),// 18 SEK
		        new DateTime(2013, 1, 14, 17, 30, 0),// 13 SEK -> Total 73, capped to 60
		
		        // Tuesday - should start a fresh 60 SEK cap
		        new DateTime(2013, 1, 15, 7, 0, 0),  // 18 SEK
	        };

            // Act
            int totalFee = _calculator.GetTotalTollFee(car, passages);

            // Assert
            // Expected: 60 (Monday) + 18 (Tuesday) = 78
            Assert.Equal(78, totalFee);
        }

    }
}
