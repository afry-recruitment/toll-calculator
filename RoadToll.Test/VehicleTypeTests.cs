using RoadToll.Application.Models;

namespace RoadToll.Test
{
    public class VehicleTypeTests
    {
        [Theory]
        [MemberData(nameof(GetVehicles))]
        public void IsTollFreeVehicle_ShouldReturnCorrectValue_ForGivenVehicleType(Vehicle vehicle, bool expected) => Assert.Equal(expected, vehicle.IsTollFreeVehicle);

        public static IEnumerable<object[]> GetVehicles()
        {
            yield return new object[] { Vehicle.Car, false };
            yield return new object[] { Vehicle.Motorbike, true };
            yield return new object[] { Vehicle.Tractor, true };
            yield return new object[] { Vehicle.Emergency, true };
            yield return new object[] { Vehicle.Diplomat, true };
            yield return new object[] { Vehicle.Foreign, true };
        }
    }
}
