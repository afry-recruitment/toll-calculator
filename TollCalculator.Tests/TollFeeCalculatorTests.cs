using TollCalculator.Models;

namespace TollCalculator.Tests
{
    public class TollFeeCalculatorTests
    {
        [Fact]
        public void NewTollFee_WithTollFreeVehicle_ShouldReturnTollFeeAmountZero()
        {
            var car = new Car("ABC-123", Vehicle.VehicleType.Military);
            var tollDate = new DateTime(2023, 1, 2, 6, 15, 0);

            var result = TollFeeCalculator.NewTollFee(car, tollDate);

            Assert.Equal(0, result.TollFeeAmount);
        }

        [Fact]
        public void NewTollFee_WithTollFreeDate_ShouldReturnTollFeeAmountZero()
        {
            var car = new Car("ABC-123", Vehicle.VehicleType.Car);
            var tollDate = new DateTime(2023, 1, 1, 6, 15, 0);

            var result = TollFeeCalculator.NewTollFee(car, tollDate);

            Assert.Equal(0, result.TollFeeAmount);
        }

        [Fact]
        public void NewTollFee_WithoutTollFreeVehicle_WithoutTollFreeDate_ShouldNotReturnTollFeeAmountZero()
        {
            var car = new Car("ABC-123", Vehicle.VehicleType.Car);
            var tollDate = new DateTime(2023, 1, 2, 6, 15, 0);

            var result = TollFeeCalculator.NewTollFee(car, tollDate);

            Assert.NotEqual(0, result.TollFeeAmount);
        }
    }
}
