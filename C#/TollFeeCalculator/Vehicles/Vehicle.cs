namespace TollFeeCalculator.Vehicles;

public class Vehicle : IVehicle
{
    public Vehicle(VehicleType type)
    {
        Type = type;
    }

    public VehicleType Type { get; }
}
