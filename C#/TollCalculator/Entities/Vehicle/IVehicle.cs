namespace TollCalculator;

public interface IVehicle
{
    public string VehicleType { get; }
    public bool IsTollFree { get; }
}