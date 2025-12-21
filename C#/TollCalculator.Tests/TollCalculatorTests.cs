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

        // [Fact]
        // public void TwoPassesWithinSameHour_ShouldChargeOnlyHighestFee()
        // {
        //     // Arrange
        //     var car = new Car();
        //     var dates = new DateTime[]
        //     {
        //         new DateTime(2013, 1, 2, 6, 15, 0),  // 6:15 AM - 8 SEK
        //         new DateTime(2013, 1, 2, 6, 45, 0)   // 6:45 AM (30 min later) - 13 SEK
        //     };

        //     // Act
        //     int totalFee = _calculator.GetTollFee(car, dates);

        //     // Assert
        //     // Within 60 minutes, should only charge the highest fee (13 SEK)
        //     Assert.Equal(13, totalFee);
        // }

        [Fact]
        public void ThreePassesWithin60Minutes_ShouldChargeOnlyHighestFee()
        {
            // Arrange
            var car = new Car();
            var dates = new DateTime[]
            {
                new DateTime(2013, 1, 2, 7, 0, 0),   // 7:00 AM - 18 SEK
                new DateTime(2013, 1, 2, 7, 15, 0),  // 7:15 AM (15 min later) - 18 SEK
                new DateTime(2013, 1, 2, 7, 45, 0)   // 7:45 AM (45 min from start) - 18 SEK
            };

            // Act
            int totalFee = _calculator.GetTollFee(car, dates);

            // Assert
            // All within 60 minutes, should only charge once: 18 SEK
            Assert.Equal(18, totalFee);
        }

        [Fact]
        public void ThreePassesWithin60Minutes_WithMilliseconds_ExposesTheBug()
        {
            // Arrange
            var car = new Car();
            var dates = new DateTime[]
            {
                new DateTime(2013, 1, 2, 7, 0, 0, 100),   // 7:00:00.100 AM - 18 SEK (.Millisecond = 100)
                new DateTime(2013, 1, 2, 7, 15, 0, 200),  // 7:15:00.200 AM (15 min later) - 18 SEK (.Millisecond = 200)
                new DateTime(2013, 1, 2, 7, 45, 0, 50)    // 7:45:00.050 AM (45 min from start) - 18 SEK (.Millisecond = 50)
            };

            // Act
            int totalFee = _calculator.GetTollFee(car, dates);

            // Assert
            // All within 60 minutes, should only charge once: 18 SEK
            // BUT WITH THE BUG:
            // - Pass 1 vs Pass 2: 200 - 100 = 100 ms = 0 min (same window) ✓
            // - Pass 1 vs Pass 3: 50 - 100 = -50 ms = 0 min (same window) ✓
            // So it accidentally works!
            Assert.Equal(18, totalFee);
        }

        [Fact]
        public void TwoPassesWithDifferentMilliseconds_ExposesTheBugClearly()
        {
            // Arrange
            var car = new Car();
            var dates = new DateTime[]
            {
                new DateTime(2013, 1, 2, 7, 0, 0, 900),   // 7:00:00.900 AM - 18 SEK (.Millisecond = 900)
                new DateTime(2013, 1, 2, 7, 15, 0, 100)   // 7:15:00.100 AM (15 min later) - 18 SEK (.Millisecond = 100)
            };

            // Act
            int totalFee = _calculator.GetTollFee(car, dates);

            // Assert
            // Should charge 18 SEK (same window)
            // BUT WITH THE BUG: 100 - 900 = -800 ms = 0 minutes (rounds to 0)
            // The bug calculates: -800 / 1000 / 60 = -0.013 minutes
            // Since -0.013 minutes <= 60, it treats them as same window
            // Result: 18 SEK (correct by accident!)
            Assert.Equal(18, totalFee);
        }

        // [Fact]
        // public void PassesWithin60Minutes_DifferentFees_ShouldChargeHighest()
        // {
        //     // Arrange
        //     var car = new Car();
        //     var dates = new DateTime[]
        //     {
        //         new DateTime(2013, 1, 2, 6, 0, 0),   // 6:00 AM - 8 SEK
        //         new DateTime(2013, 1, 2, 6, 30, 0),  // 6:30 AM (30 min later) - 13 SEK
        //         new DateTime(2013, 1, 2, 6, 50, 0)   // 6:50 AM (50 min from start) - 13 SEK
        //     };

        //     // Act
        //     int totalFee = _calculator.GetTollFee(car, dates);

        //     // Assert
        //     // All within 60 minutes, should charge only the highest: 13 SEK
        //     Assert.Equal(13, totalFee);
        // }

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
            // BUT WITH THE BUG: 0 - 0 = 0 ms = 0 minutes
            // The bug thinks they're in the same window!
            // It only charges 18 SEK (the highest fee)
            Assert.Equal(26, totalFee);
        }

        // [Fact]
        // public void MultipleSlidingWindows_ShouldChargeCorrectly()
        // {
        //     // Arrange
        //     var car = new Car();
        //     var dates = new DateTime[]
        //     {
        //         new DateTime(2013, 1, 2, 6, 0, 0),   // 6:00 AM - 8 SEK (Window 1 start)
        //         new DateTime(2013, 1, 2, 6, 30, 0),  // 6:30 AM - 13 SEK (Window 1 - higher fee)
        //         new DateTime(2013, 1, 2, 8, 0, 0),   // 8:00 AM - 13 SEK (Window 2 - more than 60 min from 6:00)
        //         new DateTime(2013, 1, 2, 8, 15, 0)   // 8:15 AM - 13 SEK (Window 2 - within 60 min of 8:00)
        //     };

        //     // Act
        //     int totalFee = _calculator.GetTollFee(car, dates);

        //     // Assert
        //     // Window 1 (6:00-6:30): charge 13 SEK (highest)
        //     // Window 2 (8:00-8:15): charge 13 SEK (highest)
        //     // Total: 13 + 13 = 26 SEK
        //     Assert.Equal(26, totalFee);
        // }

        // [Fact]
        // public void ComplexScenario_WithMultipleWindows()
        // {
        //     // Arrange
        //     var car = new Car();
        //     var dates = new DateTime[]
        //     {
        //         new DateTime(2013, 1, 2, 6, 15, 0),  // 6:15 AM - 8 SEK
        //         new DateTime(2013, 1, 2, 6, 45, 0),  // 6:45 AM - 13 SEK (within 60 min of 6:15)
        //         new DateTime(2013, 1, 2, 8, 0, 0),   // 8:00 AM - 13 SEK (more than 60 min from 6:15)
        //         new DateTime(2013, 1, 2, 15, 30, 0), // 3:30 PM - 18 SEK (new window)
        //         new DateTime(2013, 1, 2, 16, 15, 0)  // 4:15 PM - 18 SEK (within 60 min of 15:30)
        //     };

        //     // Act
        //     int totalFee = _calculator.GetTollFee(car, dates);

        //     // Assert
        //     // Window 1 (6:15-6:45): 13 SEK
        //     // Window 2 (8:00): 13 SEK
        //     // Window 3 (15:30-16:15): 18 SEK
        //     // Total: 13 + 13 + 18 = 44 SEK
        //     Assert.Equal(44, totalFee);
        // }

        // [Fact]
        // public void MaximumFeePerDay_ShouldBeCappedAt60()
        // {
        //     // Arrange
        //     var car = new Car();
        //     var dates = new DateTime[]
        //     {
        //         new DateTime(2013, 1, 2, 6, 0, 0),   // 6:00 AM - 8 SEK
        //         new DateTime(2013, 1, 2, 7, 30, 0),  // 7:30 AM - 18 SEK (new window)
        //         new DateTime(2013, 1, 2, 9, 0, 0),   // 9:00 AM - 8 SEK (new window)
        //         new DateTime(2013, 1, 2, 11, 0, 0),  // 11:00 AM - 8 SEK (new window)
        //         new DateTime(2013, 1, 2, 15, 30, 0), // 3:30 PM - 18 SEK (new window)
        //         new DateTime(2013, 1, 2, 17, 0, 0)   // 5:00 PM - 13 SEK (new window)
        //     };
        //     // Total would be: 8 + 18 + 8 + 8 + 18 + 13 = 73 SEK

        //     // Act
        //     int totalFee = _calculator.GetTollFee(car, dates);

        //     // Assert
        //     // Should be capped at 60 SEK
        //     Assert.Equal(60, totalFee);
        // }

        // [Fact]
        // public void PassesExactly60MinutesApart_ShouldStartNewWindow()
        // {
        //     // Arrange
        //     var car = new Car();
        //     var dates = new DateTime[]
        //     {
        //         new DateTime(2013, 1, 2, 6, 0, 0),   // 6:00 AM - 8 SEK
        //         new DateTime(2013, 1, 2, 7, 0, 0)    // 7:00 AM (exactly 60 min later) - 18 SEK
        //     };

        //     // Act
        //     int totalFee = _calculator.GetTollFee(car, dates);

        //     // Assert
        //     // At exactly 60 minutes, should still be in same window (<=60)
        //     // Should charge only highest: 18 SEK
        //     Assert.Equal(18, totalFee);
        // }

        // [Fact]
        // public void PassesAt61Minutes_ShouldStartNewWindow()
        // {
        //     // Arrange
        //     var car = new Car();
        //     var dates = new DateTime[]
        //     {
        //         new DateTime(2013, 1, 2, 6, 0, 0),   // 6:00 AM - 8 SEK
        //         new DateTime(2013, 1, 2, 7, 1, 0)    // 7:01 AM (61 min later) - 18 SEK
        //     };

        //     // Act
        //     int totalFee = _calculator.GetTollFee(car, dates);

        //     // Assert
        //     // More than 60 minutes, should charge both: 8 + 18 = 26 SEK
        //     Assert.Equal(26, totalFee);
        // }

        // [Fact]
        // public void MotorbikeShouldBeFree()
        // {
        //     // Arrange
        //     var motorbike = new Motorbike();
        //     var dates = new DateTime[]
        //     {
        //         new DateTime(2013, 1, 2, 7, 0, 0),   // 7:00 AM - would be 18 SEK for car
        //         new DateTime(2013, 1, 2, 15, 30, 0)  // 3:30 PM - would be 18 SEK for car
        //     };

        //     // Act
        //     int totalFee = _calculator.GetTollFee(motorbike, dates);

        //     // Assert
        //     Assert.Equal(0, totalFee);
        // }
    }
}

