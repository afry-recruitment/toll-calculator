using TollCalculatorSystem.Models;
using TollCalculatorSystem.Services;

namespace TollCalculatorSystem.Tests
{
    public class TollCalculatorTests
    {
        [Fact]
        public void PassesWithin60Minutes_TakesOnlyHighestFee()
        {
            var calculator = new TollCalculator();
            var car = new Car();

            var passes = new[]
            {
                new DateTime(2025, 8, 15, 6, 15, 0), // 8:-
                new DateTime(2025, 8, 15, 7, 14, 59) // 18 (same 60 min window with 1 second)
            };

            var fee = calculator.CalculateTollFee(car, passes);
            Assert.Equal(18, fee);
        }

        [Fact]
        public void Exactly60Minutes_StartNewWindow_SumFees()
        {
            var calculator = new TollCalculator();
            var car = new Car();

            var passes = new[]
            {
                 new DateTime(2025, 8, 15, 6, 15, 0), // 8 :-
                new DateTime(2025, 8, 15, 7, 15, 0) // 18 (new 60 min window)
            };

            var fee = calculator.CalculateTollFee(car, passes);

            Assert.Equal(26, fee);
        }

        [Fact]
        public void DailyMaxFee_Is60()
        {
            var calculator = new TollCalculator();
            var car = new Car();

            var passes = new[]
            {
                new DateTime(2025, 8, 15, 6, 0, 0), // 8:-
                new DateTime(2025, 8, 15, 7, 0, 0), // 18
                new DateTime(2025, 8, 15, 8, 0, 0), // 13
                new DateTime(2025, 8, 15, 9, 0, 0), // 8
                new DateTime(2025, 8, 15, 15, 30, 0), // 18
                new DateTime(2025, 8, 15, 17, 0, 0), // 13
            };

            var fee = calculator.CalculateTollFee(car, passes);
            Assert.Equal(60, fee);
        }

        [Fact]
        public void IfPassesIsNull_ReturnZero()
        {
            var calculator = new TollCalculator();
            Car car = new Car();

            int result = calculator.CalculateTollFee(car, null);

            Assert.Equal(0, result);
        }

        [Fact]
        public void IfVehicleIsNull_THrowArgumentNullException()
        {
            var calculator = new TollCalculator();

            var passes = new[]
            {
                new DateTime(2025, 8, 15, 7, 0, 0)
            };

            Assert.Throws<ArgumentNullException>(() => calculator.CalculateTollFee(null, passes));
        }

        [Fact]
        public void ShouldReturnZero_IfFirstDateIsTollFree()
        {
            var calculator = new TollCalculator();
            var car = new Car();
            var passes = new[]
            {
                // saturday
                new DateTime(2025, 8, 16, 6, 0, 0),
            };

            var result = calculator.CalculateTollFee(car, passes);

            Assert.Equal(0, result);
        }

        [Fact]
        public void Motorbike_ShouldBeTollFree_ReturnZero()
        {
            var calculator = new TollCalculator();
            var mc = new Motorbike();
            var passes = new[]
            {
                // Thursday
                new DateTime(2025, 8, 14, 14, 0, 0),
                new DateTime(2025, 8, 14, 8, 0, 0),
            };

            var result = calculator.CalculateTollFee(mc, passes);

            Assert.Equal(0, result);
        }
    }
}
