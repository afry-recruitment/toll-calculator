using TollFeeCalculator.Domain.Entities;
using TollFeeCalculator.Domain.Enums;

namespace TollFeeCalculator.Tests.Domain
{
    public class VehicleTests
    {
        [Theory]
        [InlineData(VehicleType.Car, false)]
        [InlineData(VehicleType.Motorbike, true)]
        [InlineData(VehicleType.Tractor, true)]
        [InlineData(VehicleType.Emergency, true)]
        [InlineData(VehicleType.Diplomat, true)]
        [InlineData(VehicleType.Foreign, true)]
        [InlineData(VehicleType.Military, true)]
        public void IsTollFree_ShouldReturnCorrectValue(VehicleType vehicleType, bool expectedTollFreeStatus)
        {
            var vehicle = new Vehicle(vehicleType);

            Assert.Equal(expectedTollFreeStatus, vehicle.IsTollFree);
        }

        [Fact]
        public void VehicleType_ShouldBeAssignedCorrectly()
        {
            var vehicle = new Vehicle(VehicleType.Car);

            Assert.Equal(VehicleType.Car, vehicle.VehicleType);
        }
    }
}
