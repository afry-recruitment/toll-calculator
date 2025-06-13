namespace TollFeeCalculator;

public class Emergency : IVehicle
{
    public string GetVehicleType()
    {
        return TollFreeVehicles.Emergency.ToString();
    }
}
