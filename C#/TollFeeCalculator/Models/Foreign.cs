namespace TollFeeCalculator;

public class Foreign : IVehicle
{
    public string GetVehicleType()
    {
        return TollFreeVehicles.Foreign.ToString();
    }
}
