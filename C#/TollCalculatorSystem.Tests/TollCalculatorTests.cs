using TollCalculatorSystem.Models;
using TollCalculatorSystem.Services;

namespace TollCalculatorSystem.Tests
{
    public class TollCalculatorTests
    {
        private readonly TollCalculator _calc = new();

        [Fact]
        public void PassesWithin60Minutes_TakesOnlyHighestFee()
        {
            var car = new Car();

            var passes = new[]
            {
                new DateTime(2025, 8, 15, 6, 15, 0), // 8:-
                new DateTime(2025, 8, 15, 7, 14, 59) // 18 (same 60 min window with 1 second)
            };

            var fee = _calc.CalculateTollFee(car, passes);
            Assert.Equal(18, fee);
        }

        [Fact]
        public void Exactly60Minutes_StartNewWindow_SumFees()
        {
            var car = new Car();

            var passes = new[]
            {
                 new DateTime(2025, 8, 15, 6, 15, 0), // 8 :-
                new DateTime(2025, 8, 15, 7, 15, 0) // 18 (new 60 min window)
            };

            var fee = _calc.CalculateTollFee(car, passes);

            Assert.Equal(26, fee);
        }

        [Fact]
        public void DailyMaxFee_Is60()
        {
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

            var fee = _calc.CalculateTollFee(car, passes);
            Assert.Equal(60, fee);
        }

        [Fact]
        public void IfPassesIsNull_ReturnZero()
        {
            Car car = new Car();

            int result = _calc.CalculateTollFee(car, null);

            Assert.Equal(0, result);
        }

        [Fact]
        public void IfVehicleIsNull_THrowArgumentNullException()
        {
            var passes = new[]
            {
                new DateTime(2025, 8, 15, 7, 0, 0)
            };

            Assert.Throws<ArgumentNullException>(() => _calc.CalculateTollFee(null, passes));
        }

        [Fact]
        public void ShouldReturnZero_IfFirstDateIsTollFree()
        {
            var car = new Car();
            var passes = new[]
            {
                // saturday
                new DateTime(2025, 8, 16, 6, 0, 0),
            };

            var result = _calc.CalculateTollFee(car, passes);

            Assert.Equal(0, result);
        }

        [Fact]
        public void Motorbike_ShouldBeTollFree_ReturnZero()
        {
            var mc = new Motorbike();
            var passes = new[]
            {
                // Thursday
                new DateTime(2025, 8, 14, 14, 0, 0),
            };

            var result = _calc.CalculateTollFee(mc, passes);

            Assert.Equal(0, result);
        }

        [Fact]
        public void Midnight_Fallback_InFeeTable()
        {
            // 00:00 = 0 should always exist: 5:00 should return 0 and not throw
            var time = new DateTime(2025, 8, 15, 5, 0, 0);
            var fee = _calc.GetTollFeeBasedOnTimeOfDay(time);

            Assert.Equal(0, fee);
        }

        [Theory]
        [InlineData(6, 29, 59, 8)]   // Last second before 06:30
        [InlineData(6, 30, 00, 13)]  // Exactly 06:30
        [InlineData(18, 29, 59, 8)]  // Last second before 18:30
        [InlineData(18, 30, 00, 0)]  // Exactly 18:30
        public void FeeSwitchesCorrectly_At_IntervalEdges(int h, int m, int s, int expected)
        {
            var time = new DateTime(2025, 8, 15, h, m, s);
            var fee = _calc.GetTollFeeBasedOnTimeOfDay(time);
            Assert.Equal(expected, fee);
        }
    }
}
