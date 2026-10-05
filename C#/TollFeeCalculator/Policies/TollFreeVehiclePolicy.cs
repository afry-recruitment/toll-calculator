namespace TollFeeCalculator.Policies;

public class TollFreeVehiclePolicy : ITollFreeVehiclePolicy
{
    public bool IsTollFree(Vehicle vehicle)
    {
        if (vehicle == null) return false;

        return vehicle.GetVehicleType() switch
        {
            "Motorbike" or "Tractor" or "Emergency" or "Diplomat" or "Foreign" or "Military" => true,
            _ => false
        };
    }
}
