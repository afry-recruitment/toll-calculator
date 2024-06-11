using TollCalculator.TollFee;
namespace TollCalculator;

public class TollCalculator(TollFeeTable feeTable, decimal maxFee, Vehicle[] tollFreeVehicles,  DateTime[] countryHolidays)
{
    private readonly TollFeeTable _feeTable = feeTable;
    private readonly TimeSpan[] _startTimes = feeTable.TollFees.Select(p => p.StartTimeSpan).ToArray();
    private readonly decimal _maxFee = maxFee;
    private readonly Vehicle[] _tollFreeVehicles = tollFreeVehicles;
    private readonly DateTime[] _countryHolidays = countryHolidays;

    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        if (IsTollFreeVehicle(vehicle))
        {
            return 0;
        }

        var groupedByDate = dates.GroupBy(dt => dt.Date);
        return (int)groupedByDate.Sum(g => GetSumPerDay(g.ToList()));
    }
    
    private decimal GetSumPerDay(List<DateTime> timesDuringDay)
    {
        if (IsTollFreeDay(timesDuringDay[0]))
        {
            return 0;
        }

        var groupedByHour = timesDuringDay.GroupBy(dt => dt.Hour);
        var dayFee = groupedByHour.Sum(g => GetHighestFeePerHour(g.ToList()));

        return dayFee > _maxFee ? _maxFee : dayFee;
    }

    private decimal GetHighestFeePerHour(List<DateTime> timesDuringHour)
    {
        return timesDuringHour.Max(timeOfDay => GetFeeForTime(timeOfDay.TimeOfDay));
    }

    private decimal GetFeeForTime(TimeSpan timeOfDay)
    {
        var idx = Array.BinarySearch(_startTimes, timeOfDay);

        switch (idx)
        {
            case >= 0 when idx < _startTimes.Length && _feeTable.TollFees[idx].StartTimeSpan  == timeOfDay:
                return _feeTable.TollFees[idx].Fee;  // exact match
            case < 0:
                idx = ~idx;  // bitwise complement to get the index of the next larger element
                break;
        }

        if (idx > _startTimes.Length || idx == 0) return 0;
        
        var (start, end, fee) = _feeTable.TollFees[idx - 1];  // interval to the left
        if (start <= timeOfDay && timeOfDay <= end)
        {
            return fee;
        }

        return 0;

    }


    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        return _tollFreeVehicles.Contains(vehicle);
    }
    
    private bool IsTollFreeDay(DateTime date)
    {
        return date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday || _countryHolidays.Any(dt => dt.Date == date.Date);
    }
}