using CityTollFeeCalculator.Vehicle;

namespace CityTollFeeCalculator.Tolls;

public interface ITollCalculator
{
    public int GetTollFee(IVehicle vehicle, IEnumerable<DateTime> dates);
}
