using TollFeeCalculator;
using Nager.Date;

public class TollCalculator
{

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTotalTollFee(Vehicle vehicle, DateTime[] dates)
    {
        DateTime intervalStart = dates[0];
        int totalFee = 0;
        int tempFee = 0;
        int nextFee = 0;

        if (vehicle == null || dates == null || dates.Length == 0) return 0;

        foreach (DateTime date in dates)
        {
            if (date < intervalStart)
            {
                throw new ArgumentException($"Dates must be in chronological order. Found {date} before {intervalStart}.");
            }
            nextFee = GetFeeForPassage(date, vehicle);
            tempFee = GetFeeForPassage(intervalStart, vehicle);
            
            if (date.Date != intervalStart.Date)
            {
                totalFee = Math.Min(totalFee, 60);
                intervalStart = date;         // Start a new day    
                tempFee = 0;
            }

            double diffInMinutes = (date - intervalStart).TotalMinutes;

            if (diffInMinutes <= 60)
            {   
                if (totalFee > 0) totalFee -= tempFee;
                if (nextFee >= tempFee) tempFee = nextFee;
                    
                totalFee += tempFee;
            }
            else
            {
                totalFee += nextFee;
                intervalStart = date;
            }
        }
        return totalFee;
    }

    public int GetFeeForPassage(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        return (hour, minute) switch
        {
            (6, >= 0 and <= 29) => 8,
            (6, >= 30 and <= 59) => 13,
            (7, _) => 18,
            (8, >= 0 and <= 29) => 13,
            (8, >= 30 and <= 59) => 8,
            ( >= 9 and <= 14, _) => 8,
            (15, >= 0 and <= 30) => 13,
            (15, >= 30 and <= 59) => 18,
            (16, _) => 18,
            (17, _) => 13,
            (18, >= 0 and <= 29) => 8,
            _ => 0, 
        };
    }

    private bool IsTollFreeDate(DateTime date)
    {
        if (date.Month == 7)
        {
            return true;
        }

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
        {
            return true;
        }
        // List of included holidays based on year 2024
        var listOfHolidays = GetHolidays(date.Year);

        // Check if the date is a manually added holiday
        if (listOfHolidays.Contains(date.Date))
        {
            return true;
        }
        return false;
    }

    private HashSet<DateTime> GetHolidays(int year)
    {
        return new HashSet<DateTime>
        {
            new DateTime(year, 1, 1),
            new DateTime(year, 1, 6),
            new DateTime(year, 3, 29),
            new DateTime(year, 3, 30),
            new DateTime(year, 3, 31),
            new DateTime(year, 4, 1),
            new DateTime(year, 5, 1),
            new DateTime(year, 5, 9),
            new DateTime(year, 5, 19),
            new DateTime(year, 6, 6),
            new DateTime(year, 6, 21),
            new DateTime(year, 6, 22),
            new DateTime(year, 11, 2),
            new DateTime(year, 12, 24),
            new DateTime(year, 12, 25),
            new DateTime(year, 12, 26),
            new DateTime(year, 12, 31)
        };
    }
      
    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;
        return TollFreeVehicleTypes.Contains(vehicle.GetVehicleType());
    }

    private static readonly HashSet<string> TollFreeVehicleTypes = new HashSet<string>
    {
        "Motorbike",
        "Tractor",
        "Emergency",
        "Diplomat",
        "Foreign",
        "Military"
    };







    /*
    * The method below is inactive, it is an updated dynamic way of evaluating holidays for every year using the Nager.Date API. 
    * It however needs a license key to work.
    * */
    private bool IsTollFreeDateUpdated(DateTime date)
    {
        // Entire month of July is considered toll-free
        if (date.Month == 7)
        {
            return true;
        }

        // Saturdays and Sundays are toll-free
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
        {
            return true;
        }

        HolidaySystem.LicenseKey = "xxxx";

        // Fetch public holidays for Sweden
        var publicHolidays = HolidaySystem.GetHolidays(date.Year, CountryCode.SE);

        // Check if the date is a public holiday
        if (publicHolidays.Any(holiday => holiday.Date == date.Date))
        {
            return true;
        }

        // Manually include Valborg 
        if (date.Month == 4 && date.Day == 30) return true;

        return false;
    }

}