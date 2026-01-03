using Xunit;
using TollFeeCalculator;
using System;

namespace TollFeeCalculator.Tests
{
    public class TollCalculatorTests
    {
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
            var calculator = new TollCalculator();
            var car = new Car();

            var dateTime = new DateTime(2013, 1, 2, hour, minute, 0);

            // Act
            int actualFee = calculator.GetTollFee(dateTime, car);

            // Assert
            Assert.Equal(expectedFee, actualFee);
        }

        [Fact]
        public void GetTotalTollFee_ShouldPickHighestFee_WhenRushHourOccursWithinSameHour()
        {
            // Arrange
            var calculator = new TollCalculator();
            var car = new Car();

            // Passage 1: 06:20 (8 SEK)
            // Passage 2: 07:10 (18 SEK - Rush Hour)
            // Total time difference is 50 minutes (within the 60-min rule)
            DateTime[] passages = {
                new DateTime(2013, 1, 2, 6, 20, 0),
                new DateTime(2013, 1, 2, 7, 10, 0)
            };

            // Act
            int totalFee = calculator.GetTotalTollFee(car, passages);

            // Assert
            // It should NOT be 8, and it should NOT be 26 (8+18). 
            // It should be 18 because 18 is the highest fee in that 60-min window.
            Assert.Equal(18, totalFee);
        }
    }
}
