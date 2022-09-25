// by Sam, King Sang Tang

using System.Globalization;
using TollFeeCalculator;
using System.Text.Json;
using CsvHelper;

public class HolidayDate
{
    public int month { get; set; }
    public int day { get; set; }
}
public class HolidayYears
{
    public Dictionary<string, List<HolidayDate>?>? holidays { get; set; }
}

public class TollCalculator
{
    private const short TOLL_VALID_MINUTES = 60;
    private const short MAX_FEE_PER_DAY = 60;
    private const int DEFAULT_MIN_TOLL = 8;
    // These 3 constants are from requirements
    // Thus, instead of picking up from JSON, I have made them constant (avoid hardcoding)
    private const string HOLIDAY_PATH = "holidays.json";
    private const string SCHEDULE_PATH = "tollSchedule.csv";
    public Dictionary<string, List<HolidayDate>?>? holidays;
    public List<Schedule>? schedules;

    public TollCalculator()
    {
        // get the holidays json and the schedule csv
        if (!File.Exists(HOLIDAY_PATH))
        {
            throw new Exception(HOLIDAY_PATH + " does not exist");
        }
        string jsonString = File.ReadAllText(HOLIDAY_PATH);
        HolidayYears holidayYear = JsonSerializer.Deserialize<HolidayYears>(jsonString)!;
        this.holidays = holidayYear.holidays;

        if (!File.Exists(SCHEDULE_PATH))
        {
            throw new Exception(SCHEDULE_PATH + " does not exist");
        }
        using (var reader = new StreamReader(SCHEDULE_PATH))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            this.schedules = csv.GetRecords<Schedule>().ToList();
        }
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
        DateTime intervalStart = dates[0];
        int totalFee = 0;
        int maxIntervalFee = 0;
        foreach (DateTime date in dates) // premise: sorted datetime and within 1 day
        {
            short diffInMinutes = Convert.ToInt16((date - intervalStart).TotalMinutes);
            int nextFee = GetTollFee(date, vehicle);

            if (diffInMinutes <= TOLL_VALID_MINUTES)
            {
                if (nextFee > maxIntervalFee) maxIntervalFee = nextFee;
            }
            else
            {
                totalFee += maxIntervalFee;
                maxIntervalFee = nextFee;
                intervalStart = date;
            }
        }
        totalFee += maxIntervalFee;

        if (totalFee > MAX_FEE_PER_DAY) totalFee = MAX_FEE_PER_DAY;
        return totalFee;
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;
        String vehicleType = vehicle.GetVehicleType();
        return Enum.IsDefined(typeof(TollFreeVehicles), vehicleType);
    }

    private int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (this.schedules is null)
            throw new Exception("The class is not initialized");

        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        // 1. As in requirement, the price ranging from 8 to 18, 0 has been replaced by 8
        // but flexible to change from the Csv file.
        // 2. for time comparsion, >= should be separated into > and = (2 cases) when minute is involved with.

        foreach (Schedule schedule in this.schedules)
        {
            if ((hour > schedule.StartHour) || (hour == schedule.StartHour && minute >= schedule.StartMinute))
            {
                if ((hour < schedule.EndHour) || (hour == schedule.EndHour && minute <= schedule.EndMinute))
                {
                    return schedule.Toll;
                }
            }
        }

        return DEFAULT_MIN_TOLL; // in case the csv does not cover all the hours and mins, which is not supposed to happen
    }

    private Boolean IsTollFreeDate(DateTime date)
    {
        string year = date.Year.ToString();
        int month = date.Month;
        int day = date.Day;
        if (this.holidays is null)
            throw new Exception("The class is not initialized");

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        if (this.holidays.ContainsKey(year))
        {
            foreach (HolidayDate hd in this.holidays[year])
            {
                if (hd.month == month && (hd.day == day || hd.day == 0)) return true;
                // day = 0 means entire month, this is for Summer (July)
            }
        }
        return false;
    }

    private enum TollFreeVehicles
    {
        Motorbike = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5
    }

// --------------------------------------------  Testing --------------------------------------------
    static DateTime[] PrepareDates(string[] dateStrings)
    {
        List<DateTime> result = new List<DateTime>();
        foreach (string s in dateStrings)
        {
            DateTime date = DateTime.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
            result.Add(date);
        }
        return result.ToArray();
    }
    
    static void Main(string[] args)
    {
        Console.WriteLine("Logging: Program Started");
        try
        {
            TollCalculator tc = new TollCalculator();
            int toPay = -1;
            Diplomat diplomat = new Diplomat();
            string[] dateString = new string[] { };

            List<List<string>> dateStringList = new List<List<string>>();

            // ------------------------------------------------------------------------
            // test1: free car, 2022/09/26 (Monday)
            int i = 1;

            Console.WriteLine("Logging: test0"+i.ToString());
            dateString = new string[]{
                "9/26/2022 8:30:52 AM" 
            };
            var dates = PrepareDates(dateString);
            toPay = tc.GetTollFee(diplomat, dates);
            Console.WriteLine("Logging: to pay: "+ toPay.ToString());
            Console.WriteLine("Logging: Expected answer: 0");
            i++;

            // ------------------------------------------------------------------------
            Car car = new Car();

            // test2: holiday, 2022/05/01
            Console.WriteLine("Logging: test0"+i.ToString());
            dateString = new string[]{
                "5/1/2022 8:30:52 AM"
            };
            dates = PrepareDates(dateString);
            toPay = tc.GetTollFee(car, dates);
            Console.WriteLine("Logging: to pay: "+ toPay.ToString());
            Console.WriteLine("Logging: Expected answer: 0");
            i++;

            // ------------------------------------------------------------------------
            // test3: Sunday, 2022/09/25
            Console.WriteLine("Logging: test0"+i.ToString());
            dateString = new string[]{
                "9/25/2022 8:30:00 AM"
            };
            dates = PrepareDates(dateString);
            toPay = tc.GetTollFee(car, dates);
            Console.WriteLine("Logging: to pay: "+ toPay.ToString());
            Console.WriteLine("Logging: Expected answer: 0");
            i++;

            // ------------------------------------------------------------------------
            // test4: normal price, 2022/09/26
            Console.WriteLine("Logging: test0"+i.ToString());
            dateString = new string[]{
                "9/26/2022 8:30:52 AM"
            };
            dates = PrepareDates(dateString);
            toPay = tc.GetTollFee(car, dates);
            Console.WriteLine("Logging: to pay: "+ toPay.ToString());
            Console.WriteLine("Logging: Expected answer: 8");
            i++;

            // ------------------------------------------------------------------------
            // test5: get highest from 60 mins, 2022/09/26
            Console.WriteLine("Logging: test0"+i.ToString());
            dateString = new string[]{
                "9/26/2022 15:00:00 PM", //13
                "9/26/2022 15:15:00 PM", //13
                "9/26/2022 15:29:00 PM", //13
                "9/26/2022 15:30:00 PM", //18
                "9/26/2022 15:45:00 PM", //18
                "9/26/2022 15:59:00 PM"  //18
            };
            dates = PrepareDates(dateString);
            toPay = tc.GetTollFee(car, dates);
            Console.WriteLine("Logging: to pay: "+ toPay.ToString());
            Console.WriteLine("Logging: Expected answer: 18");
            i++;

            // ------------------------------------------------------------------------
            // test6: hit daily limit, 2022/09/26
            Console.WriteLine("Logging: test0"+i.ToString());
            dateString = new string[]{
                "9/26/2022 6:00:00 AM", //8
                "9/26/2022 7:01:00 AM", //18
                "9/26/2022 8:02:00 AM", //13
                "9/26/2022 9:03:00 AM", //8
                "9/26/2022 10:04:00 AM", //8
                "9/26/2022 11:05:00 AM", //8
                "9/26/2022 12:06:00 PM", //8
                "9/26/2022 13:07:00 PM" //8
            };
            dates = PrepareDates(dateString);
            toPay = tc.GetTollFee(car, dates);
            Console.WriteLine("Logging: to pay: "+ toPay.ToString());
            Console.WriteLine("Logging: Expected answer: 60");
            i++;

            // ------------------------------------------------------------------------
            // test7: A worker, who works from 8 to 4
            Console.WriteLine("Logging: test0"+i.ToString());
            dateString = new string[]{
                "9/26/2022 07:30:00 AM", //18
                "9/26/2022 16:30:00 PM"  //18
            };
            dates = PrepareDates(dateString);
            toPay = tc.GetTollFee(car, dates);
            Console.WriteLine("Logging: to pay: "+ toPay.ToString());
            Console.WriteLine("Logging: Expected answer: 36");
            i++;
            // ------------------------------------------------------------------------
            // test8: Summer, Tuseday
            Console.WriteLine("Logging: test0"+i.ToString());
            dateString = new string[]{
                "7/26/2022 05:05:00 AM",
                "7/26/2022 06:06:00 AM", 
                "7/26/2022 07:07:00 AM", 
                "7/26/2022 08:08:00 AM", 
                "7/26/2022 09:09:00 AM", 
                "7/26/2022 10:10:00 AM"
            };
            dates = PrepareDates(dateString);
            toPay = tc.GetTollFee(car, dates);
            Console.WriteLine("Logging: to pay: "+ toPay.ToString());
            Console.WriteLine("Logging: Expected answer: 0");
            i++;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Logging: "+ ex.ToString());
        }
        Console.WriteLine("Logging: Program Ended");
    }
}