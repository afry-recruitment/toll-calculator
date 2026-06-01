namespace TollCalculator.Vehicles;

public class Tractor : IVehicle
{
    public string GetVehicleType()
    {
        return "Tractor";
    }
    
    public bool IsTollFree() => true;
}