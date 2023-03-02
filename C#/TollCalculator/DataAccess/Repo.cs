namespace TollCalculator.DataAccess;
public class Repo : IRepo
{
    public bool IsDayOff(DateTime date)
    {
        List<DateTime> daysOff = new List<DateTime>
        {
            new DateTime(2023, 1, 1),
            new DateTime(2023, 3, 28),
            new DateTime(2023, 3, 29),
            new DateTime(2023, 4, 29),
            new DateTime(2023, 5, 1),
            new DateTime(2023, 5, 8),
            new DateTime(2023, 5, 9),
            new DateTime(2023, 6, 5),
            new DateTime(2023, 6, 6),
            new DateTime(2023, 6, 29),
            new DateTime(2023, 11, 1),
            new DateTime(2023, 12, 24),
            new DateTime(2023, 12, 25),
            new DateTime(2023, 12, 26),
            new DateTime(2023, 12, 31),
        };

        return daysOff.Contains(date.Date);
    }

    public bool IsVehicleTollFree(string vehicleType)
    {
        if (Enum.IsDefined(typeof(TollFreeVehicles), vehicleType))
            return true;
        return false;
    }
}