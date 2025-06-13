namespace TollFeeCalculator;

public class Diplomat : IVehicle
{
    public string GetVehicleType()
    {
        return TollFreeVehicles.Diplomat.ToString();
    }
}
