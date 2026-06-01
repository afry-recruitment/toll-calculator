using TollCalculator.Vehicles;

namespace TollCalculator;

public class TollCalculator(TollFeeSchedule tollFeeSchedule)
{
    private const int MaxTotalFeePerDay = 60;

    public int GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        if (dates.Length == 0 || vehicle.IsTollFree())
            return 0;
        
        var tollDays = dates
            .OrderBy(x => x)
            .GroupBy(x => x.Date)
            .ToList();

        var totalTollFee = tollDays.Sum(day => 
            GetTollFeeForDay(day.ToList()));
        
        return totalTollFee;
    }
    
    private int GetTollFeeForDay(List<DateTime> timeStamps)
    {
        var totalFee = 0;
        var currentIntervalTimestamps = new List<DateTime>();
        var intervalStart = timeStamps.First();

        foreach (var timestamp in timeStamps)
        {
            var minutesPassed = (timestamp - intervalStart).TotalMinutes;
            if ((minutesPassed >= 60) is false)
            {
                currentIntervalTimestamps.Add(timestamp);
                continue;
            }

            totalFee += GetHighestTollFee(currentIntervalTimestamps);
            
            currentIntervalTimestamps.Clear();
            currentIntervalTimestamps.Add(timestamp);
            intervalStart = timestamp;
        }

        if (currentIntervalTimestamps.Count > 0)
            totalFee += GetHighestTollFee(currentIntervalTimestamps);

        return Math.Min(totalFee, MaxTotalFeePerDay);
    }

    private int GetHighestTollFee(List<DateTime> timestamps)
    {
        return timestamps
            .Select(tollFeeSchedule.GetTollFee)
            .DefaultIfEmpty()
            .Max();
    }

    public int GetTollFee(IVehicle vehicle, DateTime date)
    { 
        if (vehicle.IsTollFree())
            return 0;
        
        return tollFeeSchedule.GetTollFee(date);
    }
}