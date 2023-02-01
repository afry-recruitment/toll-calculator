using TollCalculator.Model;
namespace TollCalculator;
public class TollCalculatorFee
{
    public int GetTollFee(Vehicle vehicle, List<DateTime> dates)
    {

        Date date1 = new Date(dates[0]);
        date1.IsTollFreeDate();

        if (date1.IsTollFreeDate() || vehicle.ExceptionsFromCongestionTax()) return 0;

        DateTime intervalStart = dates[0];
        int tempFee = GetCongestionTax(intervalStart);
        int totalFee = 0;

        foreach (DateTime date in dates)
        {
            int nextFee = GetCongestionTax(date);

            if ((date - intervalStart).TotalMinutes <= 60)
            {
                if (totalFee > 0) totalFee -= tempFee;

                if (nextFee >= tempFee) tempFee = nextFee;

                totalFee += tempFee; 
            }
            else
            {
                intervalStart = date;
                totalFee += nextFee;
            }
        }
        if (totalFee > 60) totalFee = 60;

        return totalFee;
    }

    public class Date
    {
        private DateTime value;

        public Date(DateTime date)
        {
            value = date.Date;
        }

        public bool IsTollFreeDate()
        {
            var holidays = new List<DateTime>
            {
               new DateTime(2023, 1, 5),
               new DateTime(2023, 1, 6),
               new DateTime(2023, 4, 10),
               new DateTime(2023, 5, 1),
               new DateTime(2023, 5, 17),
               new DateTime(2023, 5, 18),
               new DateTime(2023, 6, 5),
               new DateTime(2023, 6, 6),
               new DateTime(2023, 6, 6),
               new DateTime(2023, 6, 23),
               new DateTime(2023, 11, 3),
               new DateTime(2023, 12, 25),
               new DateTime(2023, 12, 26)
            };

            return (value.DayOfWeek == DayOfWeek.Saturday || 
                    value.DayOfWeek == DayOfWeek.Sunday || 
                    value.Date.Month == 7 || 
                    holidays.Contains(value)) ? true : false;
        }
    }

    public int GetCongestionTax(DateTime date) => date switch
    {
        { Hour: 6, Minute: >= 0 } and { Minute: <= 29 } => 9,
        { Hour: 6, Minute: >= 30 } and { Minute: <= 59 } => 16,
        { Hour: 7, Minute: >= 0 } and { Minute: <= 59 } => 22,
        { Hour: 8, Minute: >= 0 } and { Minute: <= 29 } => 16,
        { Hour: 8, Minute: >= 30 } and { Hour: <= 14, Minute: <= 59 } => 9,
        { Hour: 15, Minute: >= 0 } and { Minute: <= 29 } => 16,
        { Hour: 15, Minute: >= 30 } and { Hour: <= 16, Minute: <= 59 } => 22,
        { Hour: 17, Minute: >= 0 } and { Minute: <= 59 } => 16,
        { Hour: 18, Minute: >= 0 } and { Minute: <= 29 } => 16,
        _ => 0,
    };
}
