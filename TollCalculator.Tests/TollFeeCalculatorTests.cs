using TollCalculator.Models;

namespace TollCalculator.Tests
{
    public class TollFeeCalculatorTests : IClassFixture<TollFeeCalculator>
    {
        [Fact]
        public void NewTollFee_WithTollFreeVehicle_ShouldReturnTollFeeAmountZero()
        {
            var calculator = new TollFeeCalculator();
            var motorbike = new Motorbike("ABC-123");
            var tollDate = new DateTime(2023, 1, 2, 6, 15, 0);

            var result = calculator.NewTollFee(motorbike, tollDate);

            Assert.Equal(0, result.TollFeeAmount);
        }

        [Fact]
        public void NewTollFee_WithTollFreeDate_ShouldReturnTollFeeAmountZero()
        {
            var calculator = new TollFeeCalculator();
            var car = new Car("ABC-123");
            var tollDate = new DateTime(2023, 1, 1, 6, 15, 0);

            var result = calculator.NewTollFee(car, tollDate);

            Assert.Equal(0, result.TollFeeAmount);
        }

        [Fact]
        public void NewTollFee_WithoutTollFreeVehicle_WithoutTollFreeDate_ShouldNotReturnTollFeeAmountZero()
        {
            var calculator = new TollFeeCalculator();
            var car = new Car("ABC-123");
            var tollDate = new DateTime(2023, 1, 2, 6, 15, 0);

            var result = calculator.NewTollFee(car, tollDate);

            Assert.NotEqual(0, result.TollFeeAmount);
        }

        [Fact]
        public void NewTollFee_WithNullVehicle_ShouldThrowArgumentNullException()
        {
            var calculator = new TollFeeCalculator();
            var tollDate = new DateTime(2023, 1, 2, 6, 15, 0);

            var exception = Assert.Throws<ArgumentNullException>(() => calculator.NewTollFee(null, tollDate));

            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}
