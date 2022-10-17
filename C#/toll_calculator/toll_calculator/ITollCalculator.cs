using toll_calculator.Vehicles;

namespace toll_calculator
{
    public interface ITollCalculator
    {
        int GetTollFeeForDates(IVehicle vehicle, IEnumerable<DateTime> dateTimes);
    }
}
