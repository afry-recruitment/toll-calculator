
public class TollProcessor
{
    #region Exemptions, vehicle types and holidays

    List<VehicleType> exemptVehicleTypes = new List<VehicleType>()
    {
        VehicleType.Motorbike,
        VehicleType.Tractor,
        VehicleType.Emergency,
        VehicleType.Diplomat,
        VehicleType.Foreign,
        VehicleType.Military
    };

    List<DateTime> exemptDates = new List<DateTime>
{
    new DateTime(2023, 1, 1),  // New Year's Day
    new DateTime(2023, 12, 25), // Christmas Day
    // Add other holidays here
};

    #endregion

    Dictionary<int, int> HourlyTollFees = new Dictionary<int, int>()
    {
        //Hour, fee
        { 6, 8 },
        { 7, 18 },
        { 8, 13 },
        { 9, 8 },
        { 10, 8 },
        { 11, 8 },
        { 12, 8 },
        { 13, 8 },
        { 14, 8 },
        { 15, 13 },
        { 16, 18 },
        { 17, 13 },
        { 18, 8 }
    };

    public int ProcessToll(TollRecord tollRecord)
    {
        int totalToll = 0;
        int dailyToll = 0;
        var sortedRecord = tollRecord.instances.OrderBy(dt => dt.Day).ThenBy(dt => dt.Hour);
        DateTime previousInstance = DateTime.MinValue;
        int currentDay = -1;
        int rawDebugSum = 0;

        if (isVehicleTypeExempt(tollRecord.vehicle.vehicleType))
        {
            Console.WriteLine("Vehicle is fee-exempt and will not be processed");
            return 0;
        }

        foreach (var instance in sortedRecord)
        {
            if (isDayExempt(instance.Date))
                continue;

            rawDebugSum += GetHourlyToll(instance.Hour); // for double checking correct values

            if (instance.Day != currentDay)
            {
                if(dailyToll > 0)
                {
                    DateTime date = new DateTime(instance.Year, instance.Month, instance.Day);
                    Console.WriteLine($"On day {currentDay} The toll was: {dailyToll} \n");
                    totalToll += dailyToll;
                    dailyToll = 0;
                }

                currentDay = instance.Day;
            }

            Console.WriteLine($"- {instance.TimeOfDay} Fee: {GetHourlyToll(instance.Hour)}");

            dailyToll += CalculateHourlyToll(previousInstance, instance);
            dailyToll = Math.Clamp(dailyToll, 0, 60);

            previousInstance = instance;
        }

        totalToll += dailyToll;
        Console.WriteLine("\n Raw sum without deductions: " + rawDebugSum);
        return totalToll;
    }

    int CalculateHourlyToll(DateTime previous, DateTime current)
    {
        if (previous == DateTime.MinValue)
            return GetHourlyToll(current.Hour);

        TimeSpan timeDifference = current - previous;
        if (timeDifference.TotalMinutes <= 60)
        {
            int highestToll = Math.Max(GetHourlyToll(previous.Hour), GetHourlyToll(current.Hour));
            int lowestToll = Math.Min(GetHourlyToll(previous.Hour), GetHourlyToll(current.Hour));
            highestToll -= lowestToll;
            Console.WriteLine($"Two instances were within an hour of eachother. Deducting lowest toll: " + lowestToll);
            return highestToll;
        }

        return GetHourlyToll(current.Hour);
    }

    bool isDayExempt(DateTime time)
    {
        return exemptDates.Contains(time.Date) || time.DayOfWeek == DayOfWeek.Saturday || time.DayOfWeek == DayOfWeek.Sunday;
    }

    bool isVehicleTypeExempt(VehicleType vType)
    {
        return exemptVehicleTypes.Contains(vType);
    }

    int GetHourlyToll(int hour)
    {
        return HourlyTollFees.TryGetValue(hour, out int fee) == true ? fee : 0;
    }
}