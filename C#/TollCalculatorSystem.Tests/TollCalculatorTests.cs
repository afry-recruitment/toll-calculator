using TollCalculatorSystem.Models;
using TollCalculatorSystem.Services;

namespace TollCalculatorSystem.Tests
{
    public class TollCalculatorTests
    {
        [Fact]
        public void Within60Minutes_TakesOnlyHighestFee()
        {
            var calculator = new TollCalculator();
            var car = new Car();

            var passes = new[]
            {
                new DateTime(2013, 9, 4, 6, 15, 0), // 8:-
                new DateTime(2013, 9, 4, 6, 45, 0) // 13 (same 60 min window)
            };

            var fee = calculator.GetTollFee(car, passes);
            Assert.Equal(13, fee);
        }

        [Fact]
        public void DailyMaxFee_Is60()
        {
            var calculator = new TollCalculator();
            var car = new Car();

            var passes = new[]
            {
                new DateTime(2013, 9, 4, 6, 0, 0), // 8:-
                new DateTime(2013, 9, 4, 7, 0, 0), // 18
                new DateTime(2013, 9, 4, 8, 0, 0), // 13
                new DateTime(2013, 9, 4, 9, 0, 0), // 8
                new DateTime(2013, 9, 4, 15, 30, 0), // 18
                new DateTime(2013, 9, 4, 17, 0, 0), // 13
            };

            var fee = calculator.GetTollFee(car, passes);
            Assert.Equal(60, fee);
        }
    }
}
