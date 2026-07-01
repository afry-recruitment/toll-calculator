namespace TollCalculator.Models;

/// <summary>
/// Represents a vehicle in the toll system.
/// Implement this interface to add a new vehicle type.
/// </summary>
public interface IVehicle
{
    VehicleType GetVehicleType();
}

public class Car : IVehicle
{
    public VehicleType GetVehicleType() => VehicleType.Car;
}

public class Bus : IVehicle
{
    public VehicleType GetVehicleType() => VehicleType.Bus;
}

public class Motorbike : IVehicle
{
    public VehicleType GetVehicleType() => VehicleType.Motorbike;
}

public class Tractor : IVehicle
{
    public VehicleType GetVehicleType() => VehicleType.Tractor;
}

public class Emergency : IVehicle
{
    public VehicleType GetVehicleType() => VehicleType.Emergency;
}

public class Diplomat : IVehicle
{
    public VehicleType GetVehicleType() => VehicleType.Diplomat;
}

public class Foreign : IVehicle
{
    public VehicleType GetVehicleType() => VehicleType.Foreign;
}

public class Military : IVehicle
{
    public VehicleType GetVehicleType() => VehicleType.Military;
}