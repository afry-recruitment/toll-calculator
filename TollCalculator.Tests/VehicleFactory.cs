using TollCalculator.Vehicles;

namespace TollCalculator.Tests;

public static class VehicleFactory
{
    public static IVehicle Create(string vehicleType) => vehicleType switch
    {
        "Car" => new Car(),
        "Motorbike" => new Motorbike(),
        "Tractor" => new Tractor(),
        "EmergencyVehicle" => new EmergencyVehicle(),
        "DiplomaticVehicle" => new DiplomaticVehicle(),
        "ForeignVehicle" => new ForeignVehicle(),
        "MilitaryVehicle" => new MilitaryVehicle(),
        _ => throw new ArgumentException($"Unknown vehicle type: {vehicleType}", nameof(vehicleType))
    };
}