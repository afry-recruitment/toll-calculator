namespace TollCalculator.Vehicles;

public class MilitaryVehicle : IVehicle
{
    public string GetVehicleType()
    {
        return "MilitaryVehicle";
    }
    
    public bool IsTollFree() => true;
}