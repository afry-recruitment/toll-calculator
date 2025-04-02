using CityTollFeeCalculator.Vehicle;

namespace CityTollFeeCalculator.Tolls.Policy;

public class CityTollFeePolicy : ITollFeePolicy
{

    public int MAX_FEE_PER_DAY => 60;

    public bool IsTollFreeDate(DateOnly date)
    {
        return TollFreeDates.IsTollFreeDate(date);
    }

    public bool IsTollFreeVehicle(IVehicle vehicle)
    {
        var tollFreeVehicleTypes = Enum.GetNames(typeof(TollFreeVehicles));
        return tollFreeVehicleTypes.Any(tollFreeVehicleTypes => tollFreeVehicleTypes.Equals(vehicle.GetVehicleType()));
    }
}
