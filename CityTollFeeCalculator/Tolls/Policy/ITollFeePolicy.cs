using CityTollFeeCalculator.Vehicle;

namespace CityTollFeeCalculator.Tolls.Policy;

public interface ITollFeePolicy
{
    int MAX_FEE_PER_DAY { get; }
    bool IsTollFreeDate(DateOnly date);
    bool IsTollFreeVehicle(IVehicle vehicle);
}
