using TollCalculator.DataAccess;
using TollCalculator.Models;

namespace TollCalculator;
public class TollCalculator
{
    private readonly Repo _repo;

    public TollCalculator(Repo repo)
    {
        _repo = repo;
    }

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        if (vehicle == null)
            throw new ArgumentNullException(nameof(vehicle));

        if (dates == null || dates.Length < 1)
            throw new ArgumentException("Unvalid dates");

        if (IsTollFreeVehicle(vehicle))
            return 0;

        var intervalStart = dates[0];
        var totalFee = GetTollFee(intervalStart);

        for (int i = 1; i < dates.Count(); i++)
        {
            TimeSpan timeDifference = dates[i] - intervalStart;

            if (timeDifference.TotalMinutes < 60)
            {
                var fee = GetTollFee(dates[i]);
                if (fee > totalFee)
                    totalFee = fee;
            }
            else if (timeDifference.TotalMinutes >= 60)
            {
                var fee = GetTollFee(dates[i]);
                totalFee += fee;
                intervalStart = dates[i];
            }
        }

        if (totalFee > 60)
            totalFee = 60;

        return totalFee;
    }

    public int GetTollFee(DateTime date)
    {
        int hour = date.Hour;
        int minute = date.Minute;

        if (hour == 6 && minute >= 0 && minute < 30)
            return 8;
        else if (hour == 6 && minute >= 30 && minute < 60)
            return 13;
        else if (hour == 7 && minute >= 0 && minute < 60)
            return 18;
        else if (hour == 8 && minute >= 0 && minute < 30)
            return 13;
        else if (hour >= 8 && hour < 15)
            return 8;
        else if (hour == 15 && minute >= 0 && minute < 30)
            return 13;
        else if (hour == 15 || hour == 16 && minute < 60)
            return 18;
        else if (hour == 17 && minute >= 0 && minute < 60)
            return 13;
        else if (hour == 18 && minute >= 0 && minute < 30)
            return 8;
        else
            return 0;
    }

    public bool IsTollFreeVehicle(Vehicle vehicle)
    {
        return _repo.IsVehicleTollFree(vehicle.GetVehicleType());
    }

    public bool IsTollFreeDate(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            return true;

        if (date.Month == 7)
            return true;

        return _repo.IsDayOff(date);
    }
}