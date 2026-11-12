namespace TollCalculator.Models;

public class Emergency : IVehicle
{
    public VehicleType VehicleType => VehicleType.Emergency;
}
