using TollFeeCalculator.Services.Interfaces;

namespace TollFeeCalculator.Services;

public class TollFreeService(List<string> tollFreeVehicles, List<DateTime> tollFreeDates) : ITollFreeService
{
    private readonly List<string> _tollFreeVehicles = tollFreeVehicles;
    private readonly List<DateTime> _tollFreeDates = tollFreeDates;

    public bool IsTollFreeVehicle(string registrationNumber)
    {
        return _tollFreeVehicles.Contains(registrationNumber);
    }

    public bool IsTollFreeDate(DateTime date)
    {
        return _tollFreeDates.Contains(date.Date) || date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }
}
