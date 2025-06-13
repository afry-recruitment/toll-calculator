namespace TollFeeCalculator;

public class Tractor : IVehicle
{
    public string GetVehicleType()
    {
        return TollFreeVehicles.Tractor.ToString();
    }
}
