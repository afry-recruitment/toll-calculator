using Xunit;
using TollCalculator;
using TollFeeCalculator;

namespace TollCalculator.Tests
{
    public class VehicleTest
    {
        [Fact]
        public void TollFreeVehicles()
        {
            Vehicle motorbike = new Motorbike();
            Vehicle tractor = new Tractor();
            Vehicle emergency = new Emergency();
            Vehicle diplomat = new Diplomat();
            Vehicle foreign = new Foreign();
            Vehicle military = new Military();

            Assert.True(motorbike.IsTollFree);
            Assert.True(tractor.IsTollFree);
            Assert.True(emergency.IsTollFree);
            Assert.True(diplomat.IsTollFree);
            Assert.True(foreign.IsTollFree);
            Assert.True(military.IsTollFree);
        }

        [Fact]
        public void TollVehicles()
        {
            Vehicle car = new Car();
            Vehicle truck = new Truck();

            Assert.False(car.IsTollFree);
            Assert.False(truck.IsTollFree);
        }

        [Fact]
        public void isTollFreeVehicle()
        {
            Vehicle motorbike = new Motorbike();
            Vehicle tractor = new Tractor();
            Vehicle emergency = new Emergency();
            Vehicle diplomat = new Diplomat();
            Vehicle foreign = new Foreign();
            Vehicle military = new Military();

            Vehicle car = new Car();
            Vehicle truck = new Truck();

            Assert.Equal("Motorbike", motorbike.GetVehicleType());
            Assert.Equal("Tractor", tractor.GetVehicleType());
            Assert.Equal("Emergency", emergency.GetVehicleType());
            Assert.Equal("Diplomat", diplomat.GetVehicleType());
            Assert.Equal("Foreign", foreign.GetVehicleType());
            Assert.Equal("Military", military.GetVehicleType());

            Assert.Equal("Car", car.GetVehicleType());
            Assert.Equal("Truck", truck.GetVehicleType());
        }
    }
}

