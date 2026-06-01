namespace TollCalculator.Vehicles;

public interface IVehicle
{
    string GetVehicleType();
    bool IsTollFree();
}