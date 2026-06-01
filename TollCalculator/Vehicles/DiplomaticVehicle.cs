namespace TollCalculator.Vehicles;

public class DiplomaticVehicle : IVehicle
{
    public string GetVehicleType()
    {
        return "DiplomaticVehicle";
    }
    
    public bool IsTollFree() => true;
}