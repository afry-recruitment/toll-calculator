namespace TollFeeCalculator;

public class Military : IVehicle
{
    public string GetVehicleType()
    {
        return TollFreeVehicles.Military.ToString();
    }
}