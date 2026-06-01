namespace TollCalculator.Vehicles;

public class EmergencyVehicle : IVehicle
{
    public string GetVehicleType()
    {
        return "EmergencyVehicle";
    }
    
    public bool IsTollFree() => true;
}