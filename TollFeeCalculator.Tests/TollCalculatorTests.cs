using Xunit;
using TollFeeCalculator;
using System;

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
                new DateTime(2013, 1, 2, 7, 0, 0),      // 18 SEK
		        new DateTime(2013, 1, 2, 9, 0, 0),      // 8 SEK
		        new DateTime(2013, 1, 2, 11, 0, 0),     // 8 SEK
		        new DateTime(2013, 1, 2, 13, 0, 0),     // 8 SEK
		        new DateTime(2013, 1, 2, 15, 30, 0),    // 18 SEK
		        new DateTime(2013, 1, 2, 17, 30, 0)     // 13 SEK
	        };

            // Act
            int totalFee = _calculator.GetTotalTollFee(car, passages);

            // Assert
            int expectedFee = 60;
            Assert.Equal(expectedFee, totalFee);
        }

        [Fact]
        public void GetTotalTollFee_ShouldChargeOnlyOncePerHour()
        {
            // Arrange
            var car = new Car();
            DateTime[] passages = {
                new DateTime(2013, 1, 2, 17, 15, 0),
                new DateTime(2013, 1, 2, 17, 30, 0),
                new DateTime(2013, 1, 2, 17, 45, 0)
            };

            // Act
            int totalFee = _calculator.GetTotalTollFee(car, passages);

            // Assert
            int expectedFee = 13;
            Assert.Equal(expectedFee, totalFee);
        }
    }
}
