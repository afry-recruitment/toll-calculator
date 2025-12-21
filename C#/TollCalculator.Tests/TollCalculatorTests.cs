using System;
using Xunit;
using TollFeeCalculator;

namespace TollCalculator.Tests
{
    public class TollCalculatorTests
    {
        private readonly TollFeeCalculator.TollCalculator _calculator;

        public TollCalculatorTests()
        {
            _calculator = new TollFeeCalculator.TollCalculator();
        }

        [Fact]
        public void TwoPassesWithinSameHour_ShouldChargeOnlyHighestFee()
        {
            // Arrange
            var car = new Car();
            var dates = new DateTime[]
            {
                new DateTime(2013, 1, 2, 6, 15, 0),  // 6:15 AM - 8 SEK
                new DateTime(2013, 1, 2, 6, 45, 0)   // 6:45 AM (30 min later) - 13 SEK
            };

            // Act
            int totalFee = _calculator.GetTollFee(car, dates);

            // Assert
            // Within 60 minutes, should only charge the highest fee (13 SEK)
            Assert.Equal(13, totalFee);
        }


        [Fact]
        public void PassesMoreThan60MinutesApart_ShouldChargeSeperately()
        {
            // Arrange
            var car = new Car();
            var dates = new DateTime[]
            {
                new DateTime(2013, 1, 2, 6, 0, 0),   // 6:00 AM - 8 SEK (.Millisecond = 0)
                new DateTime(2013, 1, 2, 7, 15, 0)   // 7:15 AM (75 min later) - 18 SEK (.Millisecond = 0)
            };

            // Act
            int totalFee = _calculator.GetTollFee(car, dates);

            // Assert
            // Should charge BOTH fees: 8 + 18 = 26 SEK (different windows)
            Assert.Equal(26, totalFee);
        }

        [Fact]
        public void MaximumFeePerDay_ShouldBeCappedAt60()
        {
            // Arrange
            var car = new Car();
            var dates = new DateTime[]
            {
                new DateTime(2013, 1, 2, 6, 0, 0),   // 6:00 AM - 8 SEK
                new DateTime(2013, 1, 2, 7, 30, 0),  // 7:30 AM - 18 SEK (new window)
                new DateTime(2013, 1, 2, 9, 0, 0),   // 9:00 AM - 8 SEK (new window)
                new DateTime(2013, 1, 2, 11, 0, 0),  // 11:00 AM - 8 SEK (new window)
                new DateTime(2013, 1, 2, 15, 30, 0), // 3:30 PM - 18 SEK (new window)
                new DateTime(2013, 1, 2, 17, 0, 0)   // 5:00 PM - 13 SEK (new window)
            };
            // Total would be: 8 + 18 + 8 + 8 + 18 + 13 = 73 SEK

            // Act
            int totalFee = _calculator.GetTollFee(car, dates);

            // Assert
            // Should be capped at 60 SEK
            Assert.Equal(60, totalFee);
        }

        [Fact]
        public void PassesExactly60MinutesApart_ShouldStartNewWindow()
        {
            // Arrange
            var car = new Car();
            var dates = new DateTime[]
            {
                new DateTime(2013, 1, 2, 6, 0, 0),   // 6:00 AM - 8 SEK
                new DateTime(2013, 1, 2, 7, 0, 0)    // 7:00 AM (exactly 60 min later) - 18 SEK
            };

            // Act
            int totalFee = _calculator.GetTollFee(car, dates);

            // Assert
            // At exactly 60 minutes, should still be in same window (<=60)
            // Should charge only highest: 18 SEK
            Assert.Equal(18, totalFee);
        }


        [Fact]
        public void MotorbikeShouldBeFree()
        {
            // Arrange
            var motorbike = new Motorbike();
            var dates = new DateTime[]
            {
                new DateTime(2013, 1, 2, 7, 0, 0),   // 7:00 AM - would be 18 SEK for car
                new DateTime(2013, 1, 2, 15, 30, 0)  // 3:30 PM - would be 18 SEK for car
            };

            // Act
            int totalFee = _calculator.GetTollFee(motorbike, dates);

            // Assert
            Assert.Equal(0, totalFee);
        }

        [Fact]
        public void NewYearsDayShouldBeFree()
        {
            // Arrange
            var car = new Car();
            var dates = new DateTime[]
            {
                new DateTime(2026, 1, 1, 15, 30, 0)  // New Year's Day 2026 at 3:30 PM - holiday, should be 0 SEK
            };

            // Act
            int totalFee = _calculator.GetTollFee(car, dates);

            // Assert
            Assert.Equal(0, totalFee);
        }
    }
}

