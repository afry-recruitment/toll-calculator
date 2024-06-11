using CSharpFunctionalExtensions;

namespace TollCalculator;

public class Vehicle(string vehicleType): ValueObject<Vehicle>
{ 
    private string VehicleType { get; } = vehicleType;
    
    protected override bool EqualsCore(Vehicle other)
    {
        return VehicleType.Equals(other.VehicleType, StringComparison.InvariantCultureIgnoreCase);
    }

    protected override int GetHashCodeCore()
    {
        return VehicleType.GetHashCode();
    }
}