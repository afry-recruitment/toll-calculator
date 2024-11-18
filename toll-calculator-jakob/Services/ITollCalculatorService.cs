using toll_calculator_jakob.Models;

namespace toll_calculator_jakob.Services
{
    public interface ITollCalculatorService
    {
        int GetTollFeeTotal(IVehicle vehicle, List<DateTime> dates);
        int GetTollFee(DateTime date, IVehicle vehicle);
        bool IsTollFreeDate(DateTime date);
        bool IsWithinSameHour(DateTime start, DateTime date);
        bool IsHoliday(DateTime date);
    }
}
