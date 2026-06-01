namespace TollCalculator.Vehicles;

public class ForeignVehicle : IVehicle
{
    public string GetVehicleType()
    {
        return "ForeignVehicle";
    }
    
    public bool IsTollFree() => true;
}