namespace TollFeeCalculator.Policies;

public interface ITollFreeVehiclePolicy
{
    bool IsTollFree(Vehicle vehicle);
}
